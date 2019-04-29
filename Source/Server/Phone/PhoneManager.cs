using System;
using System.Collections.Generic;
using System.Linq;
using FactionLife.Server.Model;
using System.Data;
using FactionLife.Server.Base;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Shared;
using Newtonsoft.Json;
using FactionLife.Server.Services.FactionService;
using FactionLife.Server.Phone.Interfaces;
using FactionLife.Server.Services.VoiceService;

namespace FactionLife.Server.Phone {
    /// <summary>
    /// Логика телефона
    /// </summary>
    internal class PhoneManager : Script, IPhoneManager {
        private const string DATA_CALL_STATUS = "CallActive";
        private const string DATA_CALL_OPPONENT = "CallOpponent";
        private const string DATA_CALL_IS_CALLER = "CallCaller";
        private const int CALL_COST = 2;

        private static readonly Vector3 position = new Vector3(1, 0, 0);

        private readonly IPlayerInfoManager _playerInfoManager;

        public PhoneManager() {}
        public PhoneManager(IPlayerInfoManager playerInfoManager) {
            _playerInfoManager = playerInfoManager;
        }

        /// <summary>
        /// Проинициализировать голосовой сервер
        /// </summary>
        public void Initialize() {
            ClientEventHandler.Add(ClientEvent.GET_PHONE_INFO, (player, objects) => SendPhoneInfo(player));
            ClientEventHandler.Add(ClientEvent.START_CALL, StartCall);
            ClientEventHandler.Add(ClientEvent.ANSWER_CALL, AnswerCall);
            ClientEventHandler.Add(ClientEvent.HANGUP_CALL, HangupCall);
            ClientEventHandler.Add(ClientEvent.ADD_PHONE_CONTACT, AddNewContact);
            ClientEventHandler.Add(ClientEvent.TRIGGER_PHONE_VISIBLE, TriggerPhone);
        }

        /// <summary>
        /// Начать звонок
        /// </summary>
        private void StartCall(Client player, object[] args) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            if (playerInfo.PhoneBalance < CALL_COST) {
                CancelPhoneAction(player, "Du hast nicht genug Geld um den Anruf zu machen.");
                return;
            }
            var number = Convert.ToInt32(args[0]);
            var callee = _playerInfoManager.GetByNumber(number);
            if (callee == null) {
                CancelPhoneAction(player, "Der Angerufende wurde nicht im Netzwerk gefunden");
                return;
            }
            if (HasActiveCall(callee)) {
                CancelPhoneAction(player, "Besetzt");
                return;
            }
            player.setData(DATA_CALL_OPPONENT, callee);
            player.setData(DATA_CALL_STATUS, false);
            player.setData(DATA_CALL_IS_CALLER, true);
            callee.setData(DATA_CALL_OPPONENT, player);
            callee.setData(DATA_CALL_STATUS, false);
            API.sendPictureNotificationToPlayer(callee, $"Ein Anruf von {player.name}", "CHAR_CHAT_CALL", 0, 0, $"{player.name}", "Eingehender Anruf");
        }

        /// <summary>
        /// Ответ на звонок
        /// </summary>
        private void AnswerCall(Client player, object[] args) {
            if (!player.hasData(DATA_CALL_OPPONENT) || player.hasData(DATA_CALL_IS_CALLER) || HasActiveCall(player)) {
                return;
            }
            var decision = (bool) args[0];
            var caller = (Client) player.getData(DATA_CALL_OPPONENT);
            if (decision) {
                var calleeVoice = player.position;
                var callerVoice = caller.position;

                ResetCallData(caller, player);

                calleeVoice(callerVoice, caller.position);
                callerVoice.(calleeVoice, player.position);
                player.setData(DATA_CALL_STATUS, true);
                caller.setData(DATA_CALL_STATUS, true);
                var playerInfo = _playerInfoManager.GetInfo(player);
                playerInfo.PhoneBalance -= CALL_COST;
                _playerInfoManager.RefreshUI(player, playerInfo);
            }
            else {
                ResetCallData(caller, player);
                CancelPhoneAction(player);
                CancelPhoneAction(caller, "Der Anruf wurde abgelehnt");
            }
        }

        /// <summary>
        /// Повесить трубку
        /// </summary>
        private void HangupCall(Client player, object[] args) {
            if (!player.hasData(DATA_CALL_OPPONENT) || !HasActiveCall(player)) {
                return;
            }
            var opponent = (Client) player.getData(DATA_CALL_OPPONENT);
            var voiceClient = player.GetVoiceClient();
            var voiceClientOpponent = opponent.GetVoiceClient();
            voiceClient.ResetRelativeSpeakerPosition(voiceClientOpponent);
            voiceClientOpponent.ResetRelativeSpeakerPosition(voiceClient);
            ResetCallData(player, opponent);
            CancelPhoneAction(player);
            CancelPhoneAction(opponent);
        }

        /// <summary>
        /// Создать новый контакт
        /// </summary>
        private void AddNewContact(Client player, object[] args) {
            var name = args[0].ToString();
            var phone = Convert.ToInt32(args[1]);
            var playerInfo = _playerInfoManager.GetInfo(player);
            var contact = playerInfo.PhoneContacts.FirstOrDefault(e => e.Number == phone);
            if (contact == null) {
                var newContact = new PhoneContact {CharacterId = playerInfo.CharacterId, Name = name, Phone = phone};
                playerInfo.PhoneContacts.Add(newContact);
            }
            else {
                contact.Name = name;
            }
            SendPhoneInfo(player);
        }

        /// <summary>
        /// Отправляет информацю для телефона
        /// </summary>
        private void SendPhoneInfo(Client player) {
            var playerInfo = _playerInfoManager.GetInfo(player);
            string callerName = null;
            var callerNumber = 0;
            if (player.hasData(DATA_CALL_OPPONENT) && !player.hasData(DATA_CALL_IS_CALLER)) {
                var caller = (Client) player.getData(DATA_CALL_OPPONENT);
                var callerInfo = _playerInfoManager.GetInfo(caller);
                callerName = playerInfo.PhoneContacts.FirstOrDefault(e => e.Number == callerInfo.PhoneNumber)?.Name ?? "Unbekannt";
                callerNumber = callerInfo.PhoneNumber;
            }
            API.triggerClientEvent(player, ServerEvent.SET_PHONE_INFO, 
                JsonConvert.SerializeObject(playerInfo.PhoneContacts),
                callerName, callerNumber
            );
        }

        /// <summary>
        /// Переключить анимацию персонажа
        /// </summary>
        private void TriggerPhone(Client player, object[] args) {
            if (API.isPlayerInAnyVehicle(player)) return;
            var visible = (bool)args[0];
            if (visible) {
                API.playPlayerScenario(player, "WORLD_HUMAN_STAND_MOBILE");
            }
            else {
                API.stopPlayerAnimation(player);
            }
        }

        /// <summary>
        /// Проверяет, есть ли у игрока активный звонок
        /// </summary>
        private static bool HasActiveCall(Client player) {
            return player.hasData(DATA_CALL_STATUS) && (bool) player.getData(DATA_CALL_STATUS);
        }

        /// <summary>
        /// Отменяет действие в телефоне и возвращает на главный экран
        /// </summary>
        private void CancelPhoneAction(Client player, string message = null) {
            if (message != null) {
                API.sendNotificationToPlayer(player, $"~r~{message}", true);
            }
            API.triggerClientEvent(player, ServerEvent.SHOW_MAIN_DISPLAY);
        }

        /// <summary>
        /// Сбросить данные игроков
        /// </summary>
        private static void ResetCallData(Client player, Client opponent) {
            player.resetData(DATA_CALL_OPPONENT);
            player.resetData(DATA_CALL_STATUS);
            player.resetData(DATA_CALL_IS_CALLER);
            opponent.resetData(DATA_CALL_OPPONENT);
            opponent.resetData(DATA_CALL_STATUS);
            opponent.resetData(DATA_CALL_IS_CALLER);
        }
    }
}
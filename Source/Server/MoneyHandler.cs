using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;
using FactionLife.Server.Services.MoneyService;
using System;
using System.Linq;

namespace FactionLife.Server
{
	class MoneyHandler 
        : Script
	{
        public MoneyHandler()
		{
			API.onClientEventTrigger += OnClientEvent;
		}

		public void OnClientEvent(Client client, string eventName, params object[] arguments) //arguments param can contain multiple params
		{
			
			switch (eventName)
			{
			    case "KeyboardKey_E_Pressed":
                    API.delay(3000, true, () =>
                    {
                        API.consoleOutput("Server: Start Money");
                    });
                    if (!client.isInVehicle)
			        {
			            if (client.hasData("player"))
			            {
			                ATM atm = ATMService.ATMList.FirstOrDefault(x => x.Position.DistanceTo(client.position) <= 1);
			                if (atm != null)
			                {
			                    OpenATM(client);
			                    //API.sendChatMessageToPlayer(client, "ATM with ID: ~y~" + atm.Id + " ~w~found");
			                }
			            }
			        }
                    break;
			    case "ATM_Withdraw":
			        if (MoneyService.HasPlayerEnoughBank(client, Convert.ToDouble(arguments[0])))
			        {
			            MoneyService.WithdrawMoney(client, Convert.ToDouble(arguments[0]));
                        API.shared.sendPictureNotificationToPlayer(client, "~g~" + arguments[0] + " $~w~wurde von dem Konto abgehoben.", "CHAR_BANK_MAZE", 0, 8, "Maze Bank", "Geld abgehoben");
			            API.triggerClientEvent(client, "ATM_CloseMenu");
			        }
			        else
			        {
                        API.shared.sendPictureNotificationToPlayer(client, "~r~Sie haben nicht genug Geld auf Ihrem Konto.", "CHAR_BANK_MAZE", 0, 8, "Maze Bank", "Nicht genug Geld");
			        }
                    break;
			    case "ATM_Deposit":
			        if (MoneyService.HasPlayerEnoughCash(client, Convert.ToDouble(arguments[0])))
			        {
			            MoneyService.DepositMoney(client, Convert.ToDouble(arguments[0]));
                        API.shared.sendPictureNotificationToPlayer(client, "~b~Sie haben ~g~" + arguments[0] + " $~b~ ihrem Konto hinzugefügt.", "CHAR_BANK_MAZE", 0, 8, "Maze Bank", "Geld eingezahlt");
                        API.triggerClientEvent(client, "ATM_CloseMenu");
			        }
			        else
			        {
                        API.shared.sendPictureNotificationToPlayer(client, "~r~Sie haben nicht genug Geld in Ihrem Geldbeutel.", "CHAR_BANK_MAZE", 0, 8, "Maze Bank", "Nicht genug Geld");
			        }
                    break;
                case "ATM_Transfer":
                    if (MoneyService.HasPlayerEnoughCash(client, Convert.ToDouble(arguments[0])))
                    {
                        MoneyService.DepositMoney(client, Convert.ToDouble(arguments[0]));
                        API.shared.sendPictureNotificationToPlayer(client, "~g~Sie haben ~g~" + arguments[0] + " $~w~ überwiesen.", "CHAR_BANK_MAZE", 0, 8, "Maze Bank", "Überweisung");
                        API.triggerClientEvent(client, "ATM_CloseMenu");
                    }
                    else
                    {
                        API.shared.sendPictureNotificationToPlayer(client, "~r~Sie haben nicht genug Geld in Ihrem Geldbeutel.", "CHAR_BANK_MAZE", 0, 8, "Maze Bank", "Nicht genug Geld");
                    }
                    break;
            }
		}

		public void OpenATM(Client client)
		{
			// Open ATM Menu
			Player player = client.getData("player");
			API.triggerClientEvent(client, "ATM_OpenMenu", player.Character.Bank);
		}
	}
}

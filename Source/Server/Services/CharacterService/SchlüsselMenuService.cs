using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using Newtonsoft.Json;
using FactionLife.Server.Model;
using FactionLife.Server.Services.FactionService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLife.Server.Services.CharacterService
{
    class SchlüsselMenuService
    {



        public static void OpenSchlüsselMenu(Client client)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");
            if (player.Character.IsCuffed) { return; }
            API.shared.triggerClientEvent(client, "PlayerMenu_Open", JsonConvert.SerializeObject(BuildSchlüsselMenu(player, client)));
        }



        #region Player Schlüssel Menu
        public static List<MenuItem> BuildSchlüsselMenu(Player player, Client player123)
        {
            string note1;
            string note2;
            string note3;
            string note4;
            string note5;

            Player otherplayer1;

            List<MenuItem> menuItemList = new List<MenuItem>();

            Client nextplayer = player.Character.Player;
            otherplayer1 = CharacterService.GetNextPlayerInNearOfPlayer(player);

            #region Schlüssel 1
            if (player.Character.Key1 == 0) { note1 = "~y~Kein Schlüssel"; }
            else { note1 = player.Character.Key1.ToString(); }
            menuItemList.Add(new MenuItem
            {
                Title = "~g~Schlüssel 1 : ~y~" + note1 + " ",
                Value1 = "key1given"
            });

            #endregion Schlüssel 1

            #region Schlüssel 2
            if (player.Character.Key2 == 0) { note2 = "~y~Kein Schlüssel"; }
            else { note2 = player.Character.Key2.ToString(); }
            menuItemList.Add(new MenuItem
            {
                Title = "~g~Schlüssel 2 : ~y~" + note2 + " ",
                Value1 = "key2given"
            });

            #endregion Schlüessel 2

            #region Schlüssel 3
            if (player.Character.Key3 == 0) { note3 = "~y~Kein Schlüssel"; }
            else { note3 = player.Character.Key3.ToString(); }
            menuItemList.Add(new MenuItem
            {
                Title = "~g~Schlüssel 3 : ~y~" + note3 + " ",
                Value1 = "key3given"
            });

            #endregion Schlüessel 3

            #region Schlüssel 4
            if (player.Character.Key4 == 0) { note4 = "~y~Kein Schlüssel"; }
            else { note4 = player.Character.Key4.ToString(); }
            menuItemList.Add(new MenuItem
            {
                Title = "~g~Schlüssel 4 : ~y~" + note4 + " ",
                Value1 = "key4given"
            });

            #endregion Schlüessel 2

            #region Schlüssel 5
            if (player.Character.Key5 == 0) { note5 = "~y~Kein Schlüssel"; }
            else { note5 = player.Character.Key5.ToString(); }
            menuItemList.Add(new MenuItem
            {
                Title = "~g~Schlüssel 5 : ~y~" + note5 + " ",
                Value1 = "key5given"
            });

            #endregion Schlüessel 5

            #region Schließen


            if (otherplayer1 != null)
            {

                menuItemList.Add(new MenuItem
                {
                    Title = "Schließen",
                    Value1 = "nonevaluegiven"
                });
            }
            else
            {

            }

            #endregion Schließen

            return menuItemList;
        }
        #endregion Player Schlüssel Menu


        public static void CloseMenu(Client client)
        {
            API.shared.triggerClientEvent(client, "PlayerMenu_Close");
        }

        #region Schlüssel Menu Processing
        public static void ProcessSchlüsselMenu(Client client, string itemvalue)
        {
            if (!client.hasData("player")) { return; }
            Player player = client.getData("player");


            API.shared.triggerClientEvent(client, "PlayerMenu_Close");
            switch (itemvalue)
            {

                #region nonevaluegiven

                case "nonevaluegiven":
                    break;

                #endregion nonevaluegiven

                #region schlüssel1

                case "key1given":
                    RemoveKey(client, player.Character.Key1);
                    break;

                #endregion schlüssel1

                #region schlüssel2

                case "key2given":
                    RemoveKey(client, player.Character.Key2);
                    break;

                #endregion schlüssel2

                #region schlüssel3

                case "key3given":
                    RemoveKey(client, player.Character.Key3);
                    break;

                #endregion schlüssel3

                #region schlüssel4

                case "key4given":
                    RemoveKey(client, player.Character.Key4);
                    break;

                #endregion schlüssel4

                #region schlüssel5

                case "key5given":
                    RemoveKey(client, player.Character.Key5);
                    break;

                    #endregion schlüssel5



            }
        }
        #endregion Schlüssel Menu Processing



        public static void RemoveKey(Client client, int IDofKeyToGiveAway)
        {
            Player player = client.getData("player");

            int keydata1 = player.Character.Key1;
            int keydata2 = player.Character.Key2;
            int keydata3 = player.Character.Key3;
            int keydata4 = player.Character.Key4;
            int keydata5 = player.Character.Key5;

            if ((keydata1 == IDofKeyToGiveAway) && (keydata1 == player.Character.Key1)) { player.Character.Key1 = 0; RunCommandForKeys(client, client, player.Character.Id, 0, 1); API.shared.sendChatMessageToPlayer(client, "~g~Sie haben den Schlüssel ~y~" + IDofKeyToGiveAway + " ~g~weggeworfen!"); return; }
            if ((keydata2 == IDofKeyToGiveAway) && (keydata1 == player.Character.Key2)) { player.Character.Key2 = 0; RunCommandForKeys(client, client, player.Character.Id, 0, 2); API.shared.sendChatMessageToPlayer(client, "~g~Sie haben den Schlüssel ~y~" + IDofKeyToGiveAway + " ~g~weggeworfen!"); return; }
            if ((keydata3 == IDofKeyToGiveAway) && (keydata1 == player.Character.Key3)) { player.Character.Key3 = 0; RunCommandForKeys(client, client, player.Character.Id, 0, 3); API.shared.sendChatMessageToPlayer(client, "~g~Sie haben den Schlüssel ~y~" + IDofKeyToGiveAway + " ~g~weggeworfen!"); return; }
            if ((keydata4 == IDofKeyToGiveAway) && (keydata1 == player.Character.Key4)) { player.Character.Key4 = 0; RunCommandForKeys(client, client, player.Character.Id, 0, 4); API.shared.sendChatMessageToPlayer(client, "~g~Sie haben den Schlüssel ~y~" + IDofKeyToGiveAway + " ~g~weggeworfen!"); return; }
            if ((keydata5 == IDofKeyToGiveAway) && (keydata1 == player.Character.Key5)) { player.Character.Key5 = 0; RunCommandForKeys(client, client, player.Character.Id, 0, 5); API.shared.sendChatMessageToPlayer(client, "~g~Sie haben den Schlüssel ~y~" + IDofKeyToGiveAway + " ~g~weggeworfen!"); return; }

        }

        public static void RunCommandForKeys(Client client, Client target, int targetid, int KeyID, int IntoKeyRow)
        {
            Player player = target.getData("player");

            switch (IntoKeyRow)
            {
                case 1:

                    Dictionary<string, string> parameters = new Dictionary<string, string>
                    {
                    {"@Id", targetid.ToString() },
                    {"@Key", KeyID.ToString() }
                    };

                    DataTable result = DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Key1 = @Key WHERE Id = @Id LIMIT 1", parameters);
                    if (KeyID != 0) { API.shared.sendChatMessageToPlayer(target, "~g~Sie haben einen Schlüssel für das Fahrzeug ~y~" + KeyID + "~g~ erhalten!"); }

                    break;

                case 2:

                    Dictionary<string, string> parameters2 = new Dictionary<string, string>
                    {
                    {"@Id", targetid.ToString() },
                    {"@Key", KeyID.ToString() }
                    };

                    DataTable result2 = DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Key2 = @Key WHERE Id = @Id LIMIT 1", parameters2);
                    if (KeyID != 0) { API.shared.sendChatMessageToPlayer(target, "~g~Sie haben einen Schlüssel für das Fahrzeug ~y~" + KeyID + "~g~ erhalten!"); }

                    break;
                case 3:

                    Dictionary<string, string> parameters3 = new Dictionary<string, string>
                    {
                    {"@Id", targetid.ToString() },
                    {"@Key", KeyID.ToString() }
                    };

                    DataTable result3 = DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Key3 = @Key WHERE Id = @Id LIMIT 1", parameters3);
                    if (KeyID != 0) { API.shared.sendChatMessageToPlayer(target, "~g~Sie haben einen Schlüssel für das Fahrzeug ~y~" + KeyID + "~g~ erhalten!"); }

                    break;
                case 4:

                    Dictionary<string, string> parameters4 = new Dictionary<string, string>
                    {
                    {"@Id", targetid.ToString() },
                    {"@Key", KeyID.ToString() }
                    };

                    DataTable result4 = DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Key4 = @Key WHERE Id = @Id LIMIT 1", parameters4);
                    if (KeyID != 0) { API.shared.sendChatMessageToPlayer(target, "~g~Sie haben einen Schlüssel für das Fahrzeug ~y~" + KeyID + "~g~ erhalten!"); }

                    break;
                case 5:

                    Dictionary<string, string> parameters5 = new Dictionary<string, string>
                    {
                    {"@Id", targetid.ToString() },
                    {"@Key", KeyID.ToString() }
                    };

                    DataTable result5 = DatabaseHandler.ExecutePreparedStatement("UPDATE characters SET Key5 = @Key WHERE Id = @Id LIMIT 1", parameters5);
                    if (KeyID != 0) { API.shared.sendChatMessageToPlayer(target, "~g~Sie haben einen Schlüssel für das Fahrzeug ~y~" + KeyID + "~g~ erhalten!"); }

                    break;
            }

            API.shared.delay(1000, true, () =>
            {
                CharacterService.UpdateCharacter(player.Character);

            });
        }
    }
}
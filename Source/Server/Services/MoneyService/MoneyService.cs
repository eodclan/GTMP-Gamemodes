using GrandTheftMultiplayer.Server.Elements;
using FactionLife.Server.Model;

namespace FactionLife.Server.Services.MoneyService
{
	public class MoneyService
	{
        public static bool HasPlayerEnoughCash(Client client, double neededCash)
        {
            if (!client.hasData("player"))
                return false;
            Player player = client.getData("player");
            return player.Character.Cash >= neededCash;
        }

        public static bool HasPlayerEnoughBank(Client client, double neededBank)
        {
            if (!client.hasData("player"))
                return false;
            Player player = client.getData("player");
            return player.Character.Bank >= neededBank;
        }

        public static void AddPlayerCash(Client client, double cashAmount)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            player.Character.Cash += cashAmount;
            CharacterService.CharacterService.UpdateCharacter(player.Character);
            player.Account.Player.setSyncedData("cash", player.Character.Cash);
        }

        public static void RemovePlayerCash(Client client, double cashAmount)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            player.Character.Cash -= cashAmount;
            CharacterService.CharacterService.UpdateCharacter(player.Character);
            player.Account.Player.setSyncedData("cash", player.Character.Cash);
        }

        public static void AddPlayerBank(Client client, double bankAmount)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            player.Character.Bank += bankAmount;
            CharacterService.CharacterService.UpdateCharacter(player.Character);
        }

        public static void RemovePlayerBank(Client client, double bankAmount)
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            player.Character.Bank -= bankAmount;
            CharacterService.CharacterService.UpdateCharacter(player.Character);
        }

        public static void WithdrawMoney(Client client, double amount) // Bank => Cash
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            player.Character.Bank -= amount;
            player.Character.Cash += amount;
            CharacterService.CharacterService.UpdateCharacter(player.Character);
            player.Account.Player.setSyncedData("cash", player.Character.Cash);
        }

        public static void DepositMoney(Client client, double amount) // Cash => Bank
        {
            if (!client.hasData("player"))
                return;
            Player player = client.getData("player");
            player.Character.Bank += amount;
            player.Character.Cash -= amount;
            CharacterService.CharacterService.UpdateCharacter(player.Character);
            player.Account.Player.setSyncedData("cash", player.Character.Cash);
        }

        public static void MoneyTransfer(Client client, Client targetclient, double amount) // Bank Transfer => Target Player
        {
                Player player = client.getData("player");
                Player target = targetclient.getData("player");
                player.Character.Bank -= amount;
                target.Character.Bank += amount;
                player.Account.Player.setSyncedData("bank", player.Character.Bank);
                CharacterService.CharacterService.UpdateCharacter(player.Character);
                target.Account.Player.setSyncedData("bank", target.Character.Bank);
                CharacterService.CharacterService.UpdateCharacter(target.Character);
        }

        public static void MoneyTransferCash(Client client, Client targetclient, double amount) // Hand Transfer => Target Player
        {
                Player player = client.getData("player");
                Player target = targetclient.getData("player");
                player.Character.Cash -= amount;
                target.Character.Cash += amount;
                player.Account.Player.setSyncedData("cash", player.Character.Cash);
                CharacterService.CharacterService.UpdateCharacter(player.Character);
                target.Account.Player.setSyncedData("cash", target.Character.Cash);
                CharacterService.CharacterService.UpdateCharacter(target.Character);
        }
    }
}

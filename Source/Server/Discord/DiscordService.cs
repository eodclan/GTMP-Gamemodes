using System;
using System.Drawing;

namespace FactionLife.Server.Discord
{
	internal class DiscordService
	{
		public static readonly string WebHookString = "https://discordapp.com/api/webhooks/491940655808118805/uftMGCeEz98eGOAIJ3v7YkHvh-09u-nb1rBo9gk5PcP5oUQ76_t78HXbKg_COkQgQIgN";

		public static async void SendInfoMessage(string message)
		{
			Webhook webhook = new Webhook(WebHookString);
			Embed embed = new Embed();
			embed.Title = "Server Information";
			embed.Description = message;
			embed.Color = Color.LightBlue.ToRgb();
			webhook.Embeds.Add(embed);
			try
			{
				await webhook.Send();
			}
			catch (Exception) { }
		}

		public static async void SendWarningMessage(string message)
		{
			Webhook webhook = new Webhook(WebHookString);
			Embed embed = new Embed();
			embed.Title = "Server Warnung";
			embed.Description = message;
			embed.Color = Color.Orange.ToRgb();
			webhook.Embeds.Add(embed);
			try
			{
				await webhook.Send();
			}
			catch (Exception) { }
		}

		public static async void SendErrorMessage(string message)
		{
			Webhook webhook = new Webhook(WebHookString);
			Embed embed = new Embed();
			embed.Title = "Server Fehler";
			embed.Description = message;
			embed.Color = Color.Red.ToRgb();
			webhook.Embeds.Add(embed);
			try
			{
				await webhook.Send();
			}
			catch (Exception) { }
		}
	}
}
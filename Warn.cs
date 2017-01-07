using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Terraria;
using TShockAPI;

namespace SummonLimit
{
	public partial class SummonLimit
	{
		internal const ushort WarnMinutes = 20;

		internal static readonly Dictionary<string, DateTime> Warned = new Dictionary<string, DateTime>();

		private static bool IsWarned(TSPlayer player) => Warned.ContainsKey(player.IP);

		private static void Warn(TSPlayer player)
		{
			Warned.Add(player.IP, DateTime.UtcNow);

			player.Disable("Minion amount exceeded");

			foreach (var proj in Main.projectile.Where(p => p.owner == player.Index))
				player.RemoveProjectile(proj.identity, proj.owner);
		}

		private static void CleanWarned(object sender, ElapsedEventArgs e)
		{
			Warned.RemoveAll((ip, time) => (DateTime.UtcNow - time).Minutes >= WarnMinutes);
		}
	}
}
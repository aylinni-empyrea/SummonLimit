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
		/// <summary>
		///		Minutes before the instant-kick period expires.
		/// </summary>
		internal const ushort WarnMinutes = 20;

		/// <summary>
		/// 	Contains the IP addresses and the time they were warned.
		/// </summary>
		internal static readonly Dictionary<string, DateTime> Warned = new Dictionary<string, DateTime>();

		/// <summary>
		///		Checks the warn list for a warned player's IP address.
		/// </summary>
		/// <param name="player"><see cref="TSPlayer"/> to be checked.</param>
		/// <returns>
		/// 	True if the player's IP address is in the warned list,
		/// 	otherwise false.
		/// </returns>
		private static bool IsWarned(TSPlayer player) => Warned.ContainsKey(player.IP);

		/// <summary>
		/// 	Adds the player's IP to the warned list,
		/// 	disables the player and removes their summons.
		/// </summary>
		/// <param name="player">The <see cref="TSPlayer"/> to warn.</param>
		private static void Warn(TSPlayer player)
		{
			Warned.Add(player.IP, DateTime.UtcNow);

			player.Disable("Minion amount exceeded");

			foreach (var proj in Main.projectile.Where(p => p.owner == player.Index))
				player.RemoveProjectile(proj.identity, proj.owner);
		}

		/// <summary>
		/// 	Runs every elapse of <see cref="Metronome"/> to clean the
		/// 	expired warn entries.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void CleanWarned(object sender, ElapsedEventArgs e)
		{
			Warned.RemoveAll((ip, time) => (DateTime.UtcNow - time).Minutes >= WarnMinutes);
		}
	}
}
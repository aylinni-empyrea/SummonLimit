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
		///   Checks if the given <see cref="Projectile" />
		///   is a summon minion.
		/// </summary>
		/// <param name="p"><see cref="Projectile" /> to check.</param>
		/// <returns>True if it's a summon projectile, otherwise false.</returns>
		private static bool IsMinion(Projectile p) => p?.type != null && Enum.IsDefined(typeof(Summons), p.type);

		/// <summary>
		///   Runs every elapse of <see cref="Metronome" />,
		///   checks current summon projectiles and executes
		///   <see cref="WarnOrKick" /> where appropriate.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void Check(object sender, ElapsedEventArgs e)
		{
			var players = new Dictionary<TSPlayer, ushort>();

			foreach (
				var projectile in Main.projectile.Where(p => p != null &&
				                                             p.active &&
				                                             IsMinion(p)))
			{
				var player = TShock.Players[projectile.owner];

				if (player == null || !player.Active)
					continue;

				// TODO: Check for rogue retinamini/spazmamini and abusing of stardust dragon heads/tails
				var val =
					(ushort) (projectile.type == (int) Summons.Retinamini || projectile.type == (int) Summons.Spazmamini ? 1 : 2);

				if (!players.ContainsKey(player))
					players.Add(player, val);
				else
					players[player] += val;
			}

			foreach (var player in players)
				if (!CheckPermission(player.Key, player.Value))
					WarnOrKick(player.Key);

			players.Clear();
		}

		/// <summary>
		///   Warns or kicks the player based on their
		///   status in <see cref="Warned" />.
		/// </summary>
		/// <param name="player">TSPlayer to warn or kick.</param>
		private static void WarnOrKick(TSPlayer player)
		{
			int max = player.Group.GetDynamicPermission(Permission);

			if (!IsWarned(player))
			{
				TShock.Log.Info($"{player.Name} was warned for exceeding minion limit of {max} minions.");
				Warn(player);
			}
			else
			{
				TShock.Log.Info($"{player.Name} was kicked for repeatedly exceeding minion limit of {max} minions.");
				TShock.Utils.Kick(player, $"{KickMessage} ({max})", true);
			}
		}

		/// <summary>
		///   Checks if the given <see cref="Group" /> is allowed to
		///   contain given amount of minions.
		/// </summary>
		/// <param name="player">TSPlayer to check.</param>
		/// <param name="amount">Amount of minion projectiles.</param>
		/// <returns></returns>
		private static bool CheckPermission(TSPlayer player, ushort amount)
		{
			int max = player.Group.GetDynamicPermission(Permission) * 2;

			if (max == short.MaxValue * 2)
				return true;

			return amount <= max;
		}

		/// <summary>
		///   <see cref="Terraria.ID.ProjectileID" />s of
		///   known summon minions.
		/// </summary>
		private enum Summons
		{
			BabySlime = 266,

			Hornet = 373,

			Imp = 375,

			Retinamini = 387,
			Spazmamini,

			Spider1 = 390,
			Spider2,
			Spider3,

			Pirate1 = 393,
			Pirate2,
			Pirate3,

			Pygmy1 = 191,
			Pygmy2,
			Pygmy3,
			Pygmy4,

			UFO = 423,

			Raven = 317,

			Tempest = 407,

			DeadlySphere = 533,

			StardustCell1 = 613,
			//StardustCell2,

			//StardustDragon1 = 625,
			StardustDragon2 = 626,
			//StardustDragon3,
			//StardustDragon4
		}
	}
}
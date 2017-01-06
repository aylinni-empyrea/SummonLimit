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
			StardustCell2,

			StardustDragon1 = 625,
			StardustDragon2,
			StardustDragon3,
			StardustDragon4
		}

		private static void Check(object sender, ElapsedEventArgs e)
		{
			Dictionary<TSPlayer, ushort> players = new Dictionary<TSPlayer, ushort>();

			foreach (
				var projectile in Main.projectile.Where(p => p != null &&
				                                             p.active &&
				                                             Enum.IsDefined(typeof(Summons), p.type)))
			{
				var player = TShock.Players[projectile.owner];

				if (player == null || !player.Active)
					continue;

				// TODO: Check for rogue retinamini/spazmamini
				var val =
					(ushort) (projectile.type == (int) Summons.Retinamini || projectile.type == (int) Summons.Spazmamini ? 1 : 2);

				if (!players.ContainsKey(player))
					players.Add(player, val);
				else
					players[player] += val;
			}

			foreach (var player in players)
			{
				if (!CheckPermission(player.Key, player.Value))
					TShock.Utils.Kick(player.Key, $"{KickMessage} ({player.Key.Group.GetDynamicPermission(Permission)})", true);
			}

			players.Clear();
		}

		private static bool CheckPermission(TSPlayer player, ushort amount)
		{
			var max = player.Group.GetDynamicPermission(Permission) * 2;

			if (max == short.MaxValue * 2)
				return true;

			return amount <= max;
		}
	}
}
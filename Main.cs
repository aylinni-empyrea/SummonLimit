using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Terraria;
using TerrariaApi.Server;
using Timer = System.Timers.Timer;

namespace SummonLimit
{
	[ApiVersion(2, 00)]
	public partial class SummonLimit : TerrariaPlugin
	{
		public override string Name => "SummonLimit";
		public override string Description => "Prevents summon hacking.";
		public override string Author => "Newy";
		public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

		public SummonLimit(Main game) : base(game)
		{
		}

		internal static readonly Timer Metronome = new Timer(4000);

		// As of 1.3.4.4
		internal const ushort MaxSummons = 11;

		// A "dynamic" permission
		internal const string Permission = "summonlimit";

		internal const string KickMessage = "You've exceeded the amount of allowed summons.";

		public override void Initialize()
		{
			ServerApi.Hooks.GamePostInitialize.Register(this, OnPostInitialize);
		}

		private static void OnPostInitialize(EventArgs e)
		{
			Metronome.Elapsed += Check;
			Metronome.Start();
		}
	}
}
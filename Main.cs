using System;
using System.Reflection;
using System.Timers;
using Terraria;
using TerrariaApi.Server;

namespace SummonLimit
{
	[ApiVersion(2, 00)]
	public partial class SummonLimit : TerrariaPlugin
	{
		// As of 1.3.4.4
		internal const ushort MaxSummons = 11;

		// A "dynamic" permission
		internal const string Permission = "summonlimit";

		internal const string KickMessage = "You've exceeded the amount of allowed summons.";

		internal static readonly Timer Metronome = new Timer(3000);

		public SummonLimit(Main game) : base(game)
		{
		}

		public override string Name => "SummonLimit";
		public override string Description => "Prevents summon hacking.";
		public override string Author => "Newy";
		public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

		public override void Initialize()
		{
			ServerApi.Hooks.GamePostInitialize.Register(this, OnPostInitialize);
		}

		protected override void Dispose(bool disposing)
		{
			Warned.Clear();

			Metronome.Elapsed -= Check;
			Metronome.Elapsed -= CleanWarned;
			Metronome.Stop();
			Metronome.Dispose();

			ServerApi.Hooks.GamePostInitialize.Deregister(this, OnPostInitialize);

			base.Dispose(disposing);
		}

		private static void OnPostInitialize(EventArgs e)
		{
			Metronome.Elapsed += Check;
			Metronome.Elapsed += CleanWarned;
			Metronome.Start();
		}
	}
}
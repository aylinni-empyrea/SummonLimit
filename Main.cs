using System;
using System.Reflection;
using System.Timers;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace SummonLimit
{
	[ApiVersion(2, 00)]
	public partial class SummonLimit : TerrariaPlugin
	{
		/// <summary>
		///   <para>
		///     The amount of maximum summons available without
		///     hacking or modding.
		///   </para>
		///   This value is
		///   <value>11</value>
		///   as of Terraria 1.3.4.4.
		/// </summary>
		internal const ushort MaxSummons = 11;

		/// <summary>
		///   The root permission for the plugin.
		/// </summary>
		internal const string Permission = "summonlimit";

		/// <summary>
		///   Message used for kicking the player.
		/// </summary>
		internal const string KickMessage = "You've exceeded the amount of allowed summons.";

		/// <summary>
		///		How often the cleanup <see cref="Timer"/> fires.
		/// </summary>
		internal static TimeSpan CleanupInterval = new TimeSpan(0, 30, 0);

		/// <summary>
		///   <see cref="Timer" /> used for performing
		///		cleanup operations.
		/// </summary>
		internal static readonly Timer Metronome = new Timer(CleanupInterval.TotalMilliseconds);

		public SummonLimit(Main game) : base(game)
		{
		}

		public override void Initialize()
		{
			ServerApi.Hooks.GamePostInitialize.Register(this, OnPostInitialize);
			GetDataHandlers.NewProjectile += OnNewProjectile;
		}

		private static void OnNewProjectile(object sender, GetDataHandlers.NewProjectileEventArgs e)
		{
			if (IsMinion(e.Type))
				Check();
		}

		protected override void Dispose(bool disposing)
		{
			Warned.Clear();

			Metronome.Elapsed -= CleanWarned;
			Metronome.Stop();
			Metronome.Dispose();

			ServerApi.Hooks.GamePostInitialize.Deregister(this, OnPostInitialize);

			base.Dispose(disposing);
		}

		private static void OnPostInitialize(EventArgs e)
		{
			Metronome.Elapsed += CleanWarned;
			Metronome.Start();
		}

		#region Meta

		public override string Name => "SummonLimit";
		public override string Description => "Prevents summon hacking.";
		public override string Author => "Newy";
		public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

		#endregion
	}
}
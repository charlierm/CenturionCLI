using System;
using System.Threading;

namespace CenturionCLI
{
	class CenturionCLI
	{
		public static void Main (string[] args)
		{
			Centurion.ResetConsoleColours (true);
			Centurion centurion = new Centurion ();
			centurion.Start ();
		}
	}

	class Centurion
	{
		private int Round;

		public Centurion()
		{
			this.Round = 0;
		}

		public void Start ()
		{
			while (this.Round < 100) 
			{
				PrintRound ();

				CountdownTimer timer = new CountdownTimer (55);
				timer.StartCountdown ();

				TerminalColor tc = new TerminalColor (ConsoleColor.Yellow);
				tc.StartFlashing ();
				System.Threading.Thread.Sleep (5000);
				tc.StopFlashing();

				this.Round++;
			}
		}

		public void PrintRound()
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Green;
			Console.WriteLine (String.Format("Round: {0}", this.Round));
			Console.WriteLine ();
			ResetConsoleColours (false);
		}

		public static void ResetConsoleColours(bool clear)
		{
			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;

			if (clear) {
				Console.Clear ();
			}
		}
	}

	class CountdownTimer
	{
		private int Seconds;
		private int Passed;

		public CountdownTimer(int seconds)
		{
			if (seconds == 0) {
				throw new Exception ("Countdown must be greater than 0");
			}
			this.Seconds = seconds;
			this.Passed = 0;
		}

		public void StartCountdown()
		{
			while(this.Passed < this.Seconds)
			{
				Console.Write (String.Format("{0}... ", this.Seconds - this.Passed));
				this.Passed++;
				System.Threading.Thread.Sleep (1000);
			}
		}

	}

	class TerminalColor
	{
		private ConsoleColor Color;
		private Thread Thread;

		public bool IsFlashing
		{
			get
			{
				if (this.Thread.IsAlive) {
					return true;
				} else {
					return false;
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CenturionCLI.TerminalColor"/> class.
		/// </summary>
		/// <param name="color">Color.</param>
		public TerminalColor(ConsoleColor color)
		{
			this.Color = color;
		}

		/// <summary>
		/// Starts the flashing.
		/// </summary>
		public void StartFlashing()
		{
			this.Thread = new Thread (() => Flash());
			this.Thread.Start ();
		}

		/// <summary>
		/// Stops the flashing.
		/// </summary>
		public void StopFlashing()
		{
			if (this.Thread.IsAlive) {
				this.Thread.Abort ();
			}
			Centurion.ResetConsoleColours (true);
		}

		/// <summary>
		/// Run in a thread, carries out the actual flashing
		/// </summary>
		private void Flash()
		{
			while (true) {
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.White;
				Console.Clear();
				PrintMessage ("DRINK!");
				System.Threading.Thread.Sleep (100);
				Console.BackgroundColor = this.Color;
				Console.Clear();
				System.Threading.Thread.Sleep (100);
			}

		}

		private void PrintMessage(string message)
		{
			for (int i=0; i<Console.BufferHeight; i++) {
				for (int j=0; j<10; j++) {
					Console.Write(String.Format("{0} \t", message));
				}
			}
		}
	}
}

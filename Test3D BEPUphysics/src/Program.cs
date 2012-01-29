using System;

namespace EngineTest {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args) {
			using (Game3D game = new Game3D()) {
				game.Run();
			}
		}
	}
}


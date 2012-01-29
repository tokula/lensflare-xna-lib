using System;

namespace Test2D
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game2D game = new Game2D())
            {
                game.Run();
            }
        }
    }
#endif
}


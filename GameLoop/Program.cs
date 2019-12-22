using System;
using System.Threading;
using System.Collections.Concurrent;

namespace GameLoop
{
    class Program
    {
        private static BlockingCollection<ConsoleKey> input;

        static void Main(string[] args)
        {
            GameLoop game = new GameLoop();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace GameLoop
{
    class GameLoop 
    {
        private BlockingCollection<ConsoleKey> input = new BlockingCollection<ConsoleKey>();
        private enum dir { up, down, left, right, none }
        private dir direct = new dir();
        Snake snake = new Snake();
        Buffer<char> db = new Buffer<char>(30, 30);
        public GameLoop()
        {
            Thread producer = new Thread(ReadKey);
            producer.Start();
            Console.CursorVisible = false;
            Loop();
        }

        private void Loop()
        {
            int MS_PER_UPDATE = 300000;
            long previous = DateTime.Now.Ticks; 
            long lag = 0L;
            while (true)
            {
                long current = DateTime.Now.Ticks;
                long elapsed = current - previous;
                previous = current;
                lag += elapsed;// Quanto tempo  real  passou 
                ProcessInput();
                while(lag >= MS_PER_UPDATE)
                {
                    Update ();
                    lag -= MS_PER_UPDATE;
                }
                Render ();
            }
        }
        private void ProcessInput()
        {
            ConsoleKey key;
            if (input.TryTake(out key))
            {
                switch (key)
                {
                    case ConsoleKey.W:
                        direct = dir.up;
                        break;
                    case ConsoleKey.A:
                        direct = dir.left;
                        break;
                    case ConsoleKey.S:
                        direct = dir.down;
                        break;
                    case ConsoleKey.D:
                        direct = dir.right;
                        break;
                }
            }
        }
        private void Update()
        {
            if (direct != dir.none)
            {
                switch (direct)
                {
                    case dir.up:
                        snake.posY = Math.Max(0, snake.posY - 1);
                        break;
                    case dir.left:
                        snake.posX = Math.Max(0, snake.posX - 1);
                        break;
                    case dir.down:
                        snake.posY = Math.Min(db.YDim -1, snake.posY + 1);
                        break;
                    case dir.right:
                        snake.posX = Math.Min(db.XDim -1, snake.posX + 1);
                        break;
                }
            }
        }
        private void Render()
        {
            db[snake.posX,snake.posY] = snake.visuals;

            db.Swap();

            for (int y = 0; y < db.YDim; y++)
            {
                for (int x = 0; x < db.XDim; x++)
                {
                    Console.Write(db[x,y]);
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(0, 0);
            db.Clear();
        }
        private void ReadKey()
        {
            ConsoleKey key;

            do
            {
                key = Console.ReadKey(true).Key;
                input.Add(key);
            } while (key != ConsoleKey.Escape);
        }
    }
}

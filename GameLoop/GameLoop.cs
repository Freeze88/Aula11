using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace GameLoop
{
    class GameLoop
    {
        private enum Dir { up, down, left, right, none }
        private Dir direction = new Dir();
        private Dir lastDirection = new Dir();

        private readonly BlockingCollection<ConsoleKey> input = new BlockingCollection<ConsoleKey>();
        private readonly char[,] mapVisuals = new char[28, 23];

        private readonly Buffer<char> db = new Buffer<char>(28, 23);
        private readonly List<MapPiece> n = new List<MapPiece>();
        private readonly PhysicsCollision col;
        private readonly Map map = new Map(-1, -1, -1, -1);
        private readonly Pellet pellet = new Pellet(-1, -1);
        private readonly Teleporter teleporter = new Teleporter(-1, -1);
        private readonly Snake snake = new Snake();

        public int Score { get; set; }

        private readonly string mapBuilder =
    " OOOOOOOOOOOOOOOOOOOOOOOOOOO" +
    " O............O............O" +
    " O.OOOO.OOOOO.O.OOOOO.OOOO.O" +
    " O.........................O" +
    " O.OOOO.OO.OOOOOOO.OO.OOOO.O" +
    " O......OO....O....OO......O" +
    " OOOOOO.OOOOO.O.OOOOO.OOOOOO" +
    "      O.OO         OO.O     " +
    "      O.OO         OO.O     " +
    " OOOOOO.OO         OO.OOOOOO" +
    " T     .             .     T" +
    " OOOOOO.OO         OO.OOOOOO" +
    "      O.OO         OO.O     " +
    "      O.OO         OO.O     " +
    " OOOOOO.OO OOOOOOO OO.OOOOOO" +
    " O............O............O" +
    " O.OOOO.OOOOO.O.OOOOO.OOOO.O" +
    " O...OO...............OO...O" +
    " OOO.OO.OO.OOOOOOO.OO.OO.OOO" +
    " O......OO....O....OO......O" +
    " O.OOOOOOOOOO.O.OOOOOOOOOO.O" +
    " O.........................O" +
    " OOOOOOOOOOOOOOOOOOOOOOOOOOO";


        public GameLoop()
        {
            n.Add(snake);
            ConvertMapToDoubleArray();
            GenerateMap();

            col = new PhysicsCollision(n);

            Console.SetWindowSize(50, 50);
            Thread producer = new Thread(ReadKey);
            producer.Start();
            Console.CursorVisible = false;
            Loop();
        }

        private void Loop()
        {
            int MS_PER_UPDATE = 54000;
            long previous = DateTime.Now.Ticks;
            long lag = 0L;
            while (true)
            {
                long current = DateTime.Now.Ticks;
                long elapsed = current - previous;
                previous = current;
                lag += elapsed;// Quanto tempo  real  passou
                ProcessInput();
                while (lag >= MS_PER_UPDATE)
                {
                    //Console.WriteLine(lag);
                    Update();
                    lag -= MS_PER_UPDATE;
                }
                Render();
            }
        }

        private void ProcessInput()
        {
            if (input.TryTake(out ConsoleKey key))
            {
                switch (key)
                {
                    case ConsoleKey.W:
                        direction = Dir.up;
                        break;

                    case ConsoleKey.A:
                        direction = Dir.left;
                        break;

                    case ConsoleKey.S:
                        direction = Dir.down;
                        break;

                    case ConsoleKey.D:
                        direction = Dir.right;
                        break;
                }
            }
            else
                direction = lastDirection;
        }

        private void Update()
        {
            snake.UpdatePhysics(snake);

            if (direction != Dir.none)
            {
                switch (direction)
                {
                    case Dir.up:
                        if (col.CheckCollisions(snake, 0, -1) != map.GetType())
                        {
                            snake.PosY = Math.Max(0, snake.PosY - 1);
                            lastDirection = direction;
                        }

                        break;

                    case Dir.left:
                        if (col.CheckCollisions(snake, -1, 0) != map.GetType())
                        {
                            snake.PosX = Math.Max(0, snake.PosX - 1);
                            lastDirection = direction;
                        }

                        break;

                    case Dir.down:
                        if (col.CheckCollisions(snake, 0, 1) != map.GetType())
                        {
                            snake.PosY = Math.Min(db.YDim - 1, snake.PosY + 1);
                            lastDirection = direction;
                        }

                        break;

                    case Dir.right:
                        if (col.CheckCollisions(snake, 1, 0) != map.GetType())
                        {
                            snake.PosX = Math.Min(db.XDim - 1, snake.PosX + 1);
                            lastDirection = direction;
                        }

                        break;
                }
            }

            if (col.CheckCollisions(snake) == pellet.GetType())
            {
                for (int i = 0; i < n.Count; i++)
                {
                    if (n[i].PosX == snake.PosX && n[i].PosY == snake.PosY)
                    {
                        n.RemoveAt(i);
                        mapVisuals[snake.PosX, snake.PosY] = ' ';
                    }
                }
                Score++;
            }
            if (col.CheckCollisions(snake) == teleporter.GetType())
            {
                snake.PosX = snake.PosX == 0 ? db.XDim - 2 : 1;
            }

            direction = Dir.none;
        }

        private void Render()
        {
            Console.WriteLine(Score);

            for (int y = 0; y < 23; y++)
            {
                for (int x = 0; x < 28; x++)
                {
                    db[x, y] = mapVisuals[x, y];
                }
            }

            db[snake.PosX, snake.PosY] = snake.Visuals;

            db.Swap();

            for (int y = 0; y < db.YDim; y++)
            {
                for (int x = 0; x < db.XDim; x++)
                {
                    if (db[x, y] == 'O')
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    if (db[x, y] == '#')
                    {
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    }
                    Console.Write(db[x, y]);
                    Console.ResetColor();
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

        private void GenerateMap()
        {
            int charcount = 0;

            for (int y = 0; y < 23; y++)
            {
                for (int x = 0; x < 28; x++)
                {
                    if (mapVisuals[x, y] == 'O')
                    {
                        n.Add(new Map(x, y, x + 1, y + 1));
                    }
                    if (mapVisuals[x, y] == '.')
                    {
                        n.Add(new Pellet(x, y));
                    }
                    if (mapVisuals[x, y] == 'T')
                    {
                        n.Add(new Teleporter(x, y));
                    }
                    charcount++;
                }
            }
        }

        private void ConvertMapToDoubleArray()
        {
            int charcount = 0;

            for (int y = 0; y < 23; y++)
            {
                for (int x = 0; x < 28; x++)
                {
                    mapVisuals[x, y] = mapBuilder[charcount];
                    charcount++;
                }
            }
        }
    }
}
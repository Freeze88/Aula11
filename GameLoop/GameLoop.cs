using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Text;

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
        private readonly List<MapPiece> physicsObjects = new List<MapPiece>();
        private readonly PhysicsCollision col;
        private readonly Snake snake = new Snake();
        private readonly Ghost ghost;
        public int Score { get; set; }

        private readonly string mapBuilder =
        //" OOOOOOOOOOOOOOOOOOOOOOOOOOO" +
        //" O............O............O" +
        //" O.OOOO.OOOOO.O.OOOOO.OOOO.O" +
        //" O.........................O" +
        //" O.OOOO.OO.OOOOOOO.OO.OOOO.O" +
        //" O......OO....O....OO......O" +
        //" OOOOOO.OOOOO.O.OOOOO.OOOOOO" +
        //"      O.OO         OO.O     " +
        //"      O.OO         OO.O     " +
        //" OOOOOO.OO         OO.OOOOOO" +
        //" T     .             .     T" +
        //" OOOOOO.OO         OO.OOOOOO" +
        //"      O.OO         OO.O     " +
        //"      O.OO         OO.O     " +
        //" OOOOOO.OO OOOOOOO OO.OOOOOO" +
        //" O............O............O" +
        //" O.OOOO.OOOOO.O.OOOOO.OOOO.O" +
        //" O...OO...............OO...O" +
        //" OOO.OO.OO.OOOOOOO.OO.OO.OOO" +
        //" O......OO....O....OO......O" +
        //" O.OOOOOOOOOO.O.OOOOOOOOOO.O" +
        //" O.........................O" +
        //" OOOOOOOOOOOOOOOOOOOOOOOOOOO";
        " OOOOOOOOOOOOOOOOOOOOOOOOOOO" +
    " O............O............O" +
    " O.OOOO.OOOOO.O.OOOOO.OOOO.O" +
    " O.........................O" +
    " O.OOOO.OO.OOOOOOO.OO.OOOO.O" +
    " O......OO....O....OO......O" +
    " OOOOOO.OOOOO.O.OOOOO.OOOOOO" +
    "      O.OO.........OO.O     " +
    "      O.OO.........OO.O     " +
    " OOOOOO.OO.........OO.OOOOOO" +
    " T........................ T" +
    " OOOOOO.OO.........OO.OOOOOO" +
    "      O.OO.........OO.O     " +
    "      O.OO.........OO.O     " +
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
            physicsObjects.Add(snake);
            ConvertMapToDoubleArray();
            GenerateMap();
            ghost = new Ghost(2, 1, physicsObjects);
            col = new PhysicsCollision(physicsObjects);
            direction = Dir.none;

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
        List<MapPiece> path = new List<MapPiece>();
        int counter = 0;
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

            snake.Visuals = snake.Visuals == 'o' ? 'c' : 'o';
        }
        int timer = 0;
        private void Update()
        {
            if (direction != Dir.none)
            {
                timer++;
                if (path != null && timer > 10)
                {
                    timer = 0;
                    if (counter < path.Count)
                    {
                        ghost.PosX = path[counter].PosX;
                        ghost.PosY = path[counter].PosY;

                        counter++;
                    }
                    else
                        counter = 0;
                }
                switch (direction)
                {
                    case Dir.up:
                        if (col.CheckCollisions(snake, 0, -1) != typeof(Map))
                        {
                            snake.PosY = Math.Max(0, snake.PosY - 1);
                            lastDirection = direction;
                        }

                        break;

                    case Dir.left:
                        if (col.CheckCollisions(snake, -1, 0) != typeof(Map))
                        {
                            snake.PosX = Math.Max(0, snake.PosX - 1);
                            lastDirection = direction;
                        }

                        break;

                    case Dir.down:
                        if (col.CheckCollisions(snake, 0, 1) != typeof(Map))
                        {
                            snake.PosY = Math.Min(db.YDim - 1, snake.PosY + 1);
                            lastDirection = direction;
                        }

                        break;

                    case Dir.right:
                        if (col.CheckCollisions(snake, 1, 0) != typeof(Map))
                        {
                            snake.PosX = Math.Min(db.XDim - 1, snake.PosX + 1);
                            lastDirection = direction;
                        }

                        break;
                }

                path = ghost.CalcuatePath(snake);
            }

            snake.UpdatePhysics(snake);
            ghost.UpdatePhysics(ghost);

            if (col.CheckCollisions(snake) == typeof(Pellet))
            {
                for (int i = 0; i < physicsObjects.Count; i++)
                {
                    if (physicsObjects[i].PosX == snake.PosX && physicsObjects[i].PosY == snake.PosY)
                    {
                        //physicsObjects.RemoveAt(i);
                        mapVisuals[snake.PosX, snake.PosY] = ' ';
                    }
                }
                Score++;
            }
            if (col.CheckCollisions(snake) == typeof(Teleporter))
            {
                snake.PosX = snake.PosX == 1 ? db.XDim - 2 : 2;
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
            db[ghost.PosX, ghost.PosY] = ghost.Visuals;

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
                    if (db[x, y] == 'c' || db[x, y] == 'o')
                    {
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
                        physicsObjects.Add(new Map(x, y, x + 1, y + 1));
                    }
                    if (mapVisuals[x, y] == '.')
                    {
                        physicsObjects.Add(new Pellet(x, y));
                    }
                    if (mapVisuals[x, y] == 'T')
                    {
                        physicsObjects.Add(new Teleporter(x, y));
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
using System;

namespace DoubleBuffer2D
{
    class Program
    {
        private static void Main(string[] args)
        {
            DoubleBuffer2D<char> buffer = new DoubleBuffer2D<char>(5, 5);

            buffer[0, 0] = ' ';
            buffer[1, 0] = 'x';
            buffer[2, 0] = 'X';
            buffer[3, 0] = 'x';
            buffer[4, 0] = ' ';

            buffer[0, 1] = 'x';
            buffer[1, 1] = 'X';
            buffer[2, 1] = 'X';
            buffer[3, 1] = 'X';
            buffer[4, 1] = 'x';

            buffer[0, 2] = 'x';
            buffer[1, 2] = 'X';
            buffer[2, 2] = 'X';
            buffer[3, 2] = 'X';
            buffer[4, 2] = 'x';

            buffer[0, 3] = ' ';
            buffer[1, 3] = 'x';
            buffer[2, 3] = 'X';
            buffer[3, 3] = 'x';
            buffer[4, 3] = ' ';

            buffer[0, 4] = ' ';
            buffer[1, 4] = ' ';
            buffer[2, 4] = 'x';
            buffer[3, 4] = ' ';
            buffer[4, 4] = ' ';

            buffer.Swap();

            for (int y = 0; y < buffer.YDim; y++)
            {
                for (int x = 0; x < buffer.XDim; x++)
                {
                    Console.Write(buffer[x, y]);
                }
                Console.WriteLine();
            }
        }
    }
}

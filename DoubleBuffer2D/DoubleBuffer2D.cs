using System;

namespace DoubleBuffer2D
{
    class DoubleBuffer2D<T>
    {
        private T[,] _current;
        private T[,] _next;

        public int XDim => _next.GetLength(0);
        public int YDim => _next.GetLength(0);

        public T this[int x, int y]
        { get => _current[x, y]; set => _next[x, y] = value; }

        public DoubleBuffer2D(int x, int y)
        {
            _current = new T[x, y];
            _next = new T[x, y];

            Clear();
        }

        public void Clear()
        {
            Array.Clear(_next, 0, (XDim * YDim) - 1);
        }

        public void Swap()
        {
            T[,] aux = _current;
            _current = _next;
            _next = aux;
        }
    }
}

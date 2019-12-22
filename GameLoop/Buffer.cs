using System;
using System.Collections.Generic;
using System.Text;

namespace GameLoop
{
    class Buffer<T>
    {
        private T[,] _current;
        private T[,] _next;

        public int XDim { get => _next.GetLength(0); }
        public int YDim { get => _next.GetLength(0); }

        public T this[int x, int y]
        { get => _current[x, y]; set => _next[x, y] = value; }

        public Buffer(int x, int y)
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

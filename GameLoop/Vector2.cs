namespace GameLoop
{
    internal struct Vector2
    {
        public int PosX { get; }
        public int PosY { get; }

        public Vector2(int x, int y)
        {
            PosX = x;
            PosY = y;
        }

        public override bool Equals(object obj)
        {
            Vector2? compared = obj as Vector2?;
            return PosX == compared.Value.PosX && PosY == compared.Value.PosY;
        }
    }
}

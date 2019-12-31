namespace GameLoop
{
    internal class EmptySpace : MapPiece
    {
        public EmptySpace(int x, int y)
        {
            position = new Vector2(x, y);
            Visuals = 'E';
            BoxCollider = new int[4] { x, y, x + 1, y + 1 };
        }
    }
}

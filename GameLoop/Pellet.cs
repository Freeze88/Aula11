namespace GameLoop
{
    class Pellet : MapPiece
    {
        public Pellet(int x, int y)
        {
            position = new Vector2(x, y);
            Visuals = '.';
            BoxCollider = new int[4] { x, y, x + 1, y + 1 };
        }
    }
}

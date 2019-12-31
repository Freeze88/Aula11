namespace GameLoop
{
    internal class Teleporter : MapPiece
    {
        public Teleporter(int x, int y)
        {
            position = new Vector2(x, y);
            Visuals = 'T';
            BoxCollider = new int[4] { x, y, x + 1, y + 1 };
        }
    }
}

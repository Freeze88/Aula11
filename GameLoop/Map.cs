namespace GameLoop
{
    internal class Map : MapPiece
    {
        public Map(int x, int y, int l, int w)
        {
            position = new Vector2(x, y);
            Visuals = 'O';
            BoxCollider = new int[4] { x, y, l, w };
        }
    }
}

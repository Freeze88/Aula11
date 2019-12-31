namespace GameLoop
{
    internal class MapPiece
    {
        public int[] BoxCollider;

        public Vector2 position;
        public char Visuals { get; set; }

        public int g_Cost = 0;

        public int h_Cost = 0;
        public int f_Cost => g_Cost + h_Cost;

        public MapPiece parent;
        public void UpdatePhysics()
        {
            BoxCollider[0] = position.PosX;
            BoxCollider[1] = position.PosY;
            BoxCollider[2] = position.PosX + 1;
            BoxCollider[3] = position.PosY + 1;
        }
    }
}

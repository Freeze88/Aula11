namespace GameLoop
{
    class MapPiece
    {
        public int[] BoxCollider;
        
        public Vector2 position;
        public char Visuals { get; set; }

        public int g_Cost = 0;

        public int h_Cost = 0;
        public int f_Cost { get => g_Cost + h_Cost; }

        public MapPiece parent;
        public void UpdatePhysics(MapPiece s)
        {
            BoxCollider[0] = s.position.PosX;
            BoxCollider[1] = s.position.PosY;
            BoxCollider[2] = s.position.PosX + 1;
            BoxCollider[3] = s.position.PosY + 1;
        }
    }
}

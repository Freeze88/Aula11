using System;
using System.Collections.Generic;

namespace GameLoop
{
    internal class PhysicsCollision
    {

        private readonly List<MapPiece> colliders;
        public PhysicsCollision(List<MapPiece> newColliders)
        {
            colliders = newColliders;
        }
        //public bool CheckCollisions()
        //{

        //    for (int b = 0; b < colliders.Count; b++)
        //    {
        //        for (int i = 1; i < colliders.Count -1; i++)
        //        {
        //            if (i != b &&
        //                colliders[b].BoxCollider[0] ==
        //                colliders[i].BoxCollider[0] &&
        //                colliders[b].BoxCollider[1] ==
        //                colliders[i].BoxCollider[1])
        //                return true;
        //        }
        //    }
        //    return false;
        //}
        public Type CheckCollisions(MapPiece col, int x = 0, int y = 0)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                if (colliders[i].GetType() != col.GetType())
                {
                    if (col.BoxCollider[0] + (x) >= colliders[i].BoxCollider[0] && col.BoxCollider[2] + (x) <= colliders[i].BoxCollider[2] &&
                        col.BoxCollider[1] + (y) >= colliders[i].BoxCollider[1] && col.BoxCollider[3] + (y) <= colliders[i].BoxCollider[3])
                    {
                        return colliders[i].GetType();
                    }
                }
            }
            return null;
        }
    }
}

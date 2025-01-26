using UnityEngine;

namespace SakugaEngine.Collision
{
    public struct Collider
    {
        public Vector2Int Center;
        public Vector2Int Size;
        public bool Active => Size != Vector2Int.zero;

        public bool IsOverlapping(Collider other)
        {
            if (!Active) return false;
            if (!other.Active) return false;

            bool collisionX = Center.x - (Size.x / 2) <= other.Center.x + (other.Size.x / 2) &&
                    Center.x + (Size.x / 2) >= other.Center.x - (other.Size.x / 2);

            bool collisionY = Center.y - (Size.y / 2) <= other.Center.y + (other.Size.y / 2) &&
                Center.y + (Size.y / 2) >= other.Center.y - (other.Size.y / 2);

            return collisionX && collisionY;
        }

        public void UpdateCollider(Vector2Int center, Vector2Int size)
        {
            Center = center;
            Size = size;
        }
    };
}

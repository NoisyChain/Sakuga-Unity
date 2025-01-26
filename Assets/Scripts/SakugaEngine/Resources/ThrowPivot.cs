using UnityEngine;

namespace SakugaEngine.Resources
{
    public class ThrowPivot
    {
        public int Frame;
        public Vector2Int PivotPosition = Vector2Int.zero;
        public int ThrowState = -1;
        public bool Dettach;
        public bool DettachInvertSide;
        public Global.HitstunType DettachHitstunType;
        public Vector2Int DettachHitKnockback;
        public int DettachHitKnockbackGravity = 0;
        public int DettachHitKnockbackTime = 8;
        public bool DettachHitKnockbackInertia;
        public int DettachHitstun = 8;
    }
}
//TODO: Considering to add interpolation support for this thing
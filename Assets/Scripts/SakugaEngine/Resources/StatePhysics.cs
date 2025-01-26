using UnityEngine;

namespace SakugaEngine.Resources
{
    [System.Serializable]
    public class StatePhysics
    {
        public int Frame;
        public bool UseLateralSpeed;
        public int LateralSpeed = 0;
        public bool UseVerticalSpeed;
        public int VerticalSpeed = 0;
        public bool UseGravity;
        public int Gravity = 0;
        public bool UseLateralInertia;
        public bool UseVerticalInertia;
        public bool UseHorizontalInput;
        public bool UseVerticalInput;
    }
}

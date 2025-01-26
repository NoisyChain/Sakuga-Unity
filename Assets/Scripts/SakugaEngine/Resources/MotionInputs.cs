using UnityEngine;

namespace SakugaEngine.Resources
{
    public class MotionInputs : ScriptableObject
    {
        public InputOption[] ValidInputs;
        public bool AbsoluteDirection;
        public int InputBuffer = 8;
        public int DirectionalChargeLimit = 30;
        public int ButtonChargeLimit = 30;
    }
}

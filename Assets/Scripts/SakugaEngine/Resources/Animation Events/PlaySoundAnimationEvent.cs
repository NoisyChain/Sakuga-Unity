namespace SakugaEngine.Resources
{
    [System.Serializable]
    public class PlaySoundAnimationEvent : AnimationEvent
    {
        public Global.SoundType SoundType;
        public int Source;
        public int Index;
        public bool IsRandom;
        public int Range;
        public int FromExtraVariable;
    }
}

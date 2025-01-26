namespace SakugaEngine.Resources
{
    public class ApplyDamageAnimationEvent : AnimationEvent
    {
        public int Index;
        public Global.ExtraVariableChange HealthChange;
        public int Value;
        public bool AffectedByModifiers;
        public bool AffectDamageTracker;
        public bool KillingBlow;
    }
}

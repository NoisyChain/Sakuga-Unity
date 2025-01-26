using UnityEngine;

namespace SakugaEngine.Resources
{
    [System.Serializable]
    public class HitboxElement
    {
        // Hitbox settings
        [Header("Box Settings")]
        public Vector2Int Center = Vector2Int.zero;
        public Vector2Int Size = Vector2Int.zero;
        public Global.HitboxType HitboxType;
        public Global.HitType HitType;

        // Damage settings
        [Header("Damage Settings")]
        public int BaseDamage = 150;
        public int ChipDamage;
        public bool ChipDeath;
        public bool KillingBlow = true;
        public bool GuardCrush;
        public bool Uncrouch;
        public int GuardCrushDamage = 100;
        public int SelfSuperGaugeGain = 250;
        public int OpponentSuperGaugeGain = 100;
        public int ArmorDamage = 1;
        public int Priority = 0;
        public int SelfHitStopDuration = 12;
        public int OpponentHitStopDuration = 12;
        public int BlockStopDuration = 12;
        public int CounterHitStopDuration = 20;
        public int ClashHitStopDuration = 20;
        public int DamageScalingSubtract = 15;
        
        //VFX settings
        [Header("VFX Settings")]
        public int HitEffectIndex = 0;
        public int BlockEffectIndex = 1;
        public int ClashEffectIndex = 2;
        public int ArmorHitEffectIndex = 3;
        public int GuardCrushEffectIndex = 0;

        //Ground Hitstun settings
        [Header("Ground Hit Settings")]
        public Global.HitstunType GroundHitstunType;
        public int GroundHitReaction;
        public int CrouchHitReaction;
        public Vector2Int GroundHitKnockback;
        public int GroundHitKnockbackGravity = 0;
        public int GroundHitKnockbackTime = 8;
        public int GroundHitstun = 8;
        public Vector2Int GroundBlockKnockback;
        public int GroundBlockKnockbackGravity = 0;
        public int GroundBlockKnockbackTime = 8;
        public int GroundBlockstun = 8;

        //Air Hitstun settings
        [Header("Air Hit Settings")]
        public Global.HitstunType AirHitstunType;
        public int AirHitReaction;
        public Vector2Int AirHitKnockback;
        public int AirHitKnockbackGravity = 200000;
        public int AirHitKnockbackTime = 8;
        public int AirHitstun = 8;
        public Vector2Int AirBlockKnockback;
        public int AirBlockKnockbackGravity = 200000;
        public int AirBlockKnockbackTime = 8;
        public int AirBlockstun = 8;
        

        //Throw settings
        [Header("Throw Hit Settings")]
        public bool GroundThrow;
        public bool AirThrow;
        public int ThrowHitStopDuration = 8;
        public int ThrowHitReaction = -1;

        //Pushback settings
        [Header("Corner Pushback Settings")]
        public bool AllowSelfPushback = true;
        public int SelfPushbackForce = -30000;
        public int SelfPushbackDuration = 8;

        //Extra settings
        [Header("Extra Settings")]
        public int HitConfirmState = -1;
        public bool AllowInertia = true;
        public int BounceXTime = 0;
        public int BounceXIntensity = 100;
        public int BounceXState = -1;
        public int BounceYTime = 0;
        public int BounceYIntensity = 100;
        public int BounceYState = -1;
    }
}

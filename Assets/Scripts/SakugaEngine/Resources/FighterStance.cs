using UnityEngine;
using System;

namespace SakugaEngine.Resources
{
    [CreateAssetMenu(fileName = "New Fighter Stance", menuName = "Sakuga Engine/Fighter Stance", order = 0)]
    public partial class FighterStance : ScriptableObject
    {
        [Header("Settings")]
        public bool IsDamagePersistent;
        public bool IsRoundPersistent;
        public int NeutralState = 0;
        public MoveSettings[] Moves;
        public int[] HitReactions;

        [Header("Blocking")]
        public int GroundBlockInitialState = -1;
        public int CrouchBlockInitialState = -1;
        public int AirBlockInitialState = -1;

        [Header("Recovery")]
        public int GroundForwardRecoveryState = -1;
        public int GroundBackwardsRecoveryState = -1;
        public int AirForwardRecoveryState = -1;
        public int AirBackwardsRecoveryState = -1;
        public int OffTheGroundRecoveryState = -1;

        [Header("Throw Escape")]
        public int GroundThrowEscapeState = -1;
        public int AirThrowEscapeState = -1;
    }
}

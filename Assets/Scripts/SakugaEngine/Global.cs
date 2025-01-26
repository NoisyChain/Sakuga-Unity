using System;
using UnityEngine;

namespace SakugaEngine
{
    public static class Global
    {
        //Global settings
        public const int TicksPerSecond = 60;
        public const int SubSteps = 4;
        public const int Delta = TicksPerSecond * SubSteps;
        public const int SimulationScale = 10000;
        public const int WallLimit = 75000;
        public const int CeilingLimit = 120000;
        public const int StartingPosition = 15000;
        public const int MaxPlayersDistance = 70000;
        public const int InputHistorySize = 16;
        public const int KaraCancelWindow = 3;
        //public const int RoundsToWin = 2; //<= Move this to the GameMonitor
        //public const int GameTimer = 99;  //<= Move this to the GameMonitor
        public const int ThrowHitStunAdditional = 8;
        public const int GravityDecay = 2500;
        public const int HitstunDecayMinCombo = 5;
        public const int MinHitstun = 8;
        public const int RecoveryJumpVelocity = 60000;
        public const int RecoveryGravity = 200000;
        public const int RecoveryHorizontalSpeed = 45000;
        public const int ThrowEscapePushForce = 45000;
        public const int GuardCrushHitstun = 40;

        //Global inputs
        public const int INPUT_UP = 1 << 0;
        public const int INPUT_DOWN = 1 << 1;
        public const int INPUT_LEFT = 1 << 2;
        public const int INPUT_RIGHT = 1 << 3;
        public const int INPUT_FACE_A = 1 << 4;
        public const int INPUT_FACE_B = 1 << 5;
        public const int INPUT_FACE_C = 1 << 6;
        public const int INPUT_FACE_D = 1 << 7;
        //Free space
        public const int INPUT_MACRO_DASH = 1 << 12;
        public const int INPUT_TAUNT = 1 << 13;
        public const int INPUT_MENU = 1 << 14;
        public const int INPUT_BACK = 1 << 15;
        public const int INPUT_ANY_DIRECTION = INPUT_UP | INPUT_DOWN | INPUT_LEFT | INPUT_RIGHT;
        public const int INPUT_ANY_BUTTON = INPUT_FACE_A | INPUT_FACE_B | INPUT_FACE_C | INPUT_FACE_D;

        //Global enumerators
        public enum AnimationStage { STARTUP, ACTIVE, RECOVERY }
        public enum ButtonMode { PRESS, HOLD, RELEASE, UNPRESSED }
        public enum MoveEndCondition { STATE_END, RELEASE_BUTTON, STATE_TYPE_CHANGE }
        public enum SideChangeMode { NONE, CHANGE_SIDE, INTERRUPT }
        public enum StateType { NULL, MOVEMENT, COMBAT, BLOCKING, HIT_REACTION, LOCKED }
        public enum HitboxType{ HURTBOX, HITBOX, PROXIMITY_BLOCK, PROJECTILE, THROW, COUNTER, DEFLECT }
        public enum HitType{ HIGH, MEDIUM, LOW, UNBLOCKABLE }
        public enum HitstunType { BASIC = 1, KNOCKDOWN = 2, HARD_KNOCKDOWN = 3, DIZZINESS = 4, STAGGER = 5 }
        public enum ExtraVariableMode { IDLE, INCREASE, DECREASE }
        public enum ExtraVariableChange { SET, ADD, SUBTRACT }
        public enum ExtraVariableCompareMode { EQUAL, GREATER, GREATER_EQUAL, LESS, LESS_EQUAL }
        public enum FadeScreenMode { NONE, FADE_IN, FADE_OUT }
        public enum RelativeTo{ WORLD, SELF, OPPONENT, FIGHTER, SPAWNABLE }
        public enum SoundType{ SFX, VOICE }
        public enum ObjectType{ SPAWNABLE, VFX }
        public enum SpawnableHitCheck{ OPPONENT, OWNER, BOTH }
        public enum PauseMode { PRESS, HOLD, LOCK }
        //...
        public enum DirectionalInputs
        {
            DOWN_LEFT = 1,
            DOWN = 2,
            DOWN_RIGHT = 3,
            LEFT = 4,
            NEUTRAL = 5,
            RIGHT = 6,
            UP_LEFT = 7,
            UP = 8,
            UP_RIGHT = 9,
            HORIZONTAL_CHARGE = 10,
            VERTICAL_CHARGE = 11,
            DIAGIONAL_CHARGE_UP = 12,
            DIAGIONAL_CHARGE_DOWN = 13
        }

        public enum ButtonInputs
        {
            NULL = 0,
            FACE_A = 1,
            FACE_B = 2,
            FACE_C = 3,
            FACE_D = 4,
            FACE_AB = 5,
            FACE_AC = 6,
            FACE_BC = 7,
            FACE_ABC = 8,
            FACE_ABCD = 9,
            FACE_ANY = 10,
            TAUNT = 11
        }

        //Global bit flags
        [Flags] public enum FrameProperties : byte
        {
            DAMAGE_IMUNITY = 1 << 0,
            THROW_IMUNITY = 1 << 1,
            PROJECTILE_IMUNITY = 1 << 2,
            FORCE_MOVE_CANCEL = 1 << 3
        }
        [Flags] public enum TransitionCondition : byte
        {
            STATE_END = 1 << 0,
            AT_FRAME = 1 << 1,
            ON_GROUND = 1 << 2,
            ON_WALLS = 1 << 3,
            ON_FALL = 1 << 4,
            ON_LIFE_END = 1 << 5,
            ON_INPUT_COMMAND = 1 << 6,
            ON_DISTANCE = 1 << 7,
        }

        //Random Number Generator
        public static System.Random RNG;
        public static string baseSeed = "Sakuga Engine"; //You can change this if you want
        public static void UpdateRNG(int seed)
        {
            RNG = new System.Random(seed);
        }

        public static Vector2 ToScaledVector2(Vector2Int vector)
        {
            return new Vector2
            (
                vector.x / (float)SimulationScale,
                vector.y / (float)SimulationScale
            );
        }
        public static Vector3 ToScaledVector3(Vector2Int vector, float zScale = 0f)
        {
            return new Vector3
            (
                vector.x / (float)SimulationScale,
                vector.y / (float)SimulationScale,
                zScale
            );
        }
        public static Vector3 ToScaledVector3(Vector3Int vector)
        {
            return new Vector3
            (
                vector.x / (float)SimulationScale,
                vector.y / (float)SimulationScale,
                vector.z / (float)SimulationScale
            );
        }
        public static float ToScaledFloat(int value)
        {
            return value / (float)SimulationScale;
        }

        public static int IntLerp(int from, int to, int numberOfSteps, int currentStep)
        {
            return ((to - from) * currentStep) / numberOfSteps;
        }

        public static bool IsOnRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        public static int HorizontalDistance(Vector2Int a, Vector2Int b)
        {
            int ax = a.x - b.x;
            if (ax == 0) return 0;
            
            int dx = ax / ax;
            //int dy = a.Y - b.Y;
            int dy = 0;
            return dx * dx + dy * dy;
        }

        public static int SignInt(int v)
        {
            if (v > 0) return 1;
            else if (v < 0) return -1;
            else return 0;
        }
    }
}
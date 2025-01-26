using UnityEngine;
using System.IO;
using SakugaEngine.Resources;

namespace SakugaEngine.Collision
{
    public class PhysicsBody : MonoBehaviour
    {
        public bool IsStatic;
        public bool StayOnBounds;
        public bool IgnoreWalls;
        public int FixedAcceleration;
        public int FixedDeceleration;
        public int FixedFriction;
        public int HitboxesLimit = 16;
        public HitboxSettings[] HitboxPresets;
        [HideInInspector] public IDamage Parent;
        [HideInInspector] public uint ID;
        [HideInInspector] public Collider Pushbox;
        [HideInInspector] public Collider[] Hitboxes;

        [HideInInspector] public Vector2Int FixedPosition;
        [HideInInspector] public Vector2Int FixedVelocity;
        [HideInInspector] public bool IsLeftSide;
        [HideInInspector] public int PlayerSide;
        [HideInInspector] public bool IsMovable = true;
        [HideInInspector] public bool HitConfirmed;
        [HideInInspector] public bool ProximityBlocked;
        [HideInInspector] public int CurrentHitbox = -1;
        [HideInInspector] public byte FrameProperties = 0;
        
        public bool IsOnGround => FixedPosition.y <= 0;
        public bool IsOnLeftWall => FixedPosition.x <= -Global.WallLimit;
        public bool IsOnRightWall => FixedPosition.x >= Global.WallLimit;
        public bool IsFalling => !IsOnGround && FixedVelocity.y <= 0;
        public bool JustLanded => IsOnGround && FixedVelocity.y < 0;
        public bool IsOnWall => IsOnLeftWall || IsOnRightWall;
        

        public void Initialize(IDamage owner)
        {
            FixedPosition = Vector2Int.zero;
            FixedVelocity = Vector2Int.zero;
            Parent = owner;
            Hitboxes = new Collider[HitboxesLimit];
        }
        public void SetID(uint newID)
        {
            ID = newID;
        }
        public void SetVelocity(Vector2Int newVelocity)
        {
            FixedVelocity = newVelocity;
        }
        public void SetLateralVelocity(int newVelocity)
        {
            FixedVelocity.x = newVelocity;
        }
        public void SetVerticalVelocity(int newVelocity)
        {
            FixedVelocity.y = newVelocity;
        }
        public void AddLateralAcceleration(int newVelocity)
        {
            int absVelocity = Mathf.Abs(newVelocity);
            if (newVelocity != 0 && Mathf.Sign(newVelocity) > 0)
            {
                if (FixedVelocity.x < 0)
                    FixedVelocity.x += FixedFriction / Global.TicksPerSecond;
                else if (FixedVelocity.x < absVelocity)
                    FixedVelocity.x += FixedAcceleration / Global.TicksPerSecond;
                    
            }
            else if (newVelocity != 0 && Mathf.Sign(newVelocity) < 0)
            {
                if (FixedVelocity.x > 0)
                    FixedVelocity.x -= FixedFriction / Global.TicksPerSecond;
                else if (FixedVelocity.x > -absVelocity)
                    FixedVelocity.x -= FixedAcceleration / Global.TicksPerSecond;
            }
            else
                FixedVelocity.x -= Mathf.Min(Mathf.Abs(FixedVelocity.x), FixedDeceleration / Global.TicksPerSecond) * Global.SignInt(FixedVelocity.x);
        }
        public void AddVerticalAcceleration(int newVelocity)
        {
            int absVelocity = Mathf.Abs(newVelocity);
            if (newVelocity != 0 && Mathf.Sign(newVelocity) > 0)
            {
                if (FixedVelocity.y < 0)
                    FixedVelocity.y += FixedFriction / Global.TicksPerSecond;
                else if (FixedVelocity.y < absVelocity)
                    FixedVelocity.y += FixedAcceleration / Global.TicksPerSecond;
                    
            }
            else if (newVelocity != 0 && Mathf.Sign(newVelocity) < 0)
            {
                if (FixedVelocity.y > 0)
                    FixedVelocity.y -= FixedFriction / Global.TicksPerSecond;
                else if (FixedVelocity.y > -absVelocity)
                    FixedVelocity.y -= FixedAcceleration / Global.TicksPerSecond;
            }
            else
                FixedVelocity.y -= Mathf.Min(Mathf.Abs(FixedVelocity.y), FixedDeceleration / Global.TicksPerSecond) * Global.SignInt(FixedVelocity.y);
        }
        public void AddGravity(int gravity)
        {
            FixedVelocity.y -= gravity / Global.TicksPerSecond;
        }
        public void MoveTo(Vector2Int destination)
        {
            if (IsStatic) return;
            //if (!IsMovable) return;
            
            FixedPosition = destination;
            UpdateColliders();
        }
        public void Resolve(Vector2Int depth)
        {
            FixedPosition -= depth;
            UpdateColliders();
        }
        public void Move()
        {
            if (IsStatic) return;
            if (!IsMovable) return;

            FixedPosition += FixedVelocity / Global.Delta;
            UpdateColliders();
        }

        public void SetHitbox(int index)
        {
            CurrentHitbox = index;
            UpdateColliders();
        }

        public bool ContainsFrameProperty(byte CompareTo)
        {
            return (FrameProperties & CompareTo) != 0;
        }

        public void UpdateColliders()
        {
            //Lock collider on bounds
            if (StayOnBounds)
            {
                if (!IgnoreWalls)
                    FixedPosition.x = Mathf.Clamp(FixedPosition.x, -Global.WallLimit, Global.WallLimit);
                FixedPosition.y = Mathf.Clamp(FixedPosition.y, 0, Global.CeilingLimit);
            }

            if (CurrentHitbox < 0) //If there's no hitbox active, remove all boxes
            {
                Pushbox.UpdateCollider(FixedPosition, Vector2Int.zero);

                for(int i = 0; i < Hitboxes.Length; i++)
                    Hitboxes[i].UpdateCollider(FixedPosition, Vector2Int.zero);
            }
            else //Update hitboxes
            {
                Vector2Int Side = new Vector2Int(PlayerSide, 1);

                Pushbox.UpdateCollider(FixedPosition + (GetCurrentHitbox().PushboxCenter * Side), GetCurrentHitbox().PushboxSize);
                
                for(int i = 0; i < Hitboxes.Length; i++)
                    if (i < GetCurrentHitbox().Hitboxes.Length)
                        Hitboxes[i].UpdateCollider(FixedPosition + (GetCurrentHitbox().Hitboxes[i].Center * Side), GetCurrentHitbox().Hitboxes[i].Size);
                    else
                        Hitboxes[i].UpdateCollider(FixedPosition, Vector2Int.zero);
            }
        }

        public HitboxSettings GetCurrentHitbox() => HitboxPresets[CurrentHitbox];

        public void Serialize(BinaryWriter bw)
        {
            bw.Write(FixedPosition.x);
            bw.Write(FixedPosition.y);
            bw.Write(FixedVelocity.x);
            bw.Write(FixedVelocity.y);
            bw.Write(IsLeftSide);
            bw.Write(PlayerSide);
            bw.Write(IsMovable);
            bw.Write(HitConfirmed);
            bw.Write(ProximityBlocked);
            bw.Write(CurrentHitbox);
            bw.Write(FrameProperties);
        }

        public void Deserialize(BinaryReader br)
        {
            FixedPosition.x = br.ReadInt32();
            FixedPosition.y = br.ReadInt32();
            FixedVelocity.x = br.ReadInt32();
            FixedVelocity.y = br.ReadInt32();
            IsLeftSide = br.ReadBoolean();
            PlayerSide = br.ReadInt32();
            IsMovable = br.ReadBoolean();
            HitConfirmed = br.ReadBoolean();
            ProximityBlocked = br.ReadBoolean();
            CurrentHitbox = br.ReadInt32();
            FrameProperties = br.ReadByte();
        }
    }
}

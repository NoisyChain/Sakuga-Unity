using UnityEngine;
using SakugaEngine.Resources;
using System.IO;

namespace SakugaEngine
{
    //[Icon("res://Sprites/Icons/Icon_Spawnable.png")]
    public partial class SakugaSpawnable : SakugaActor, IDamage
    {
        [Header("Timers")]
        public FrameTimer LifeTime;

        [Header("Variables")]
        public int InitialState;
        public int DeathState;
        public int DeflectState = -1;
        public bool DieOnGround;
        public bool DieOnWalls;
        public bool DieOnHit;
        public bool Deflectable;
        public bool CountLifeTimeOnSpawn = true;
        public Global.SpawnableHitCheck HitCheck;

        [HideInInspector] public bool IsActive;
        private byte CurrentHitCheck;

        private SakugaFighter _owner;

        public SakugaFighter GetFighterOwner() => _owner;
        public void SetFighterOwner(SakugaFighter owner) { _owner = owner; }
        public override SakugaFighter FighterReference() { return GetFighterOwner(); }

        private bool AllowHitCheck(SakugaActor other)
        {
            if (CurrentHitCheck == 1 && other != _owner) return false;
            if (CurrentHitCheck == 0 && other != _owner.GetOpponent()) return false;

            return true;
        }

        protected override void UpdateView()
        {
            base.UpdateView();
            for (int i = 0; i < Graphics.Length; ++i)
                Graphics[i].gameObject.SetActive(IsActive);
        }

        protected override bool LifeEnded() { return !LifeTime.IsRunning(); }

        public void Initialize(SakugaFighter owner)
        {
            IsActive = false;
            CurrentHitCheck = (byte)HitCheck;
            SetFighterOwner(owner);
            Body.Initialize(this);
            Body.CurrentHitbox = -1;
            Body.IsLeftSide = GetFighterOwner().Body.IsLeftSide;
            Inputs = _owner.Inputs;
            Animator.PlayState(InitialState);
            Animator.Frame = -1;
        }

        public void Spawn(Vector2Int origin)
        {
            CurrentHitCheck = (byte)HitCheck;
            Body.MoveTo(origin);
            Body.IsLeftSide = GetFighterOwner().Body.IsLeftSide;
            Animator.PlayState(InitialState);
            Animator.Frame = -1;
            LifeTime.Play();
            if (!CountLifeTimeOnSpawn) LifeTime.Pause();

            IsActive = true;
        }

        public void Reset()
        {
            IsActive = false;
            CurrentHitCheck = (byte)HitCheck;
            Body.IsLeftSide = GetFighterOwner().Body.IsLeftSide;
            Body.FixedVelocity = Vector2Int.zero;
            Body.FixedPosition = Vector2Int.zero;
            Animator.PlayState(InitialState);
            Animator.Frame = -1;
            Body.SetHitbox(-1);
        }

        public override void PreTick()
        {
            if (!IsActive) return;
            EventExecuted = false;

            Body.IsMovable = !GetFighterOwner().HitStop.IsRunning();
            
            if (!GetFighterOwner().HitStop.IsRunning())
            {
                LifeTime.Run();
                Animator.RunState();
            }
        }

        public override void Tick()
        {
            if (!IsActive) return;

            if (Variables != null) Variables.UpdateExtraVariables();
            CheckDeathConditions();
            
            if (!LifeTime.IsRunning() && Animator.Frame >= Animator.GetCurrentState().Duration - 1)
            {
                IsActive = false;
                Animator.Frame = -1;
                Body.SetHitbox(-1);
            }

            UpdateFighterPhysics();
        }

        public override void LateTick()
        {
            if (!IsActive) return;

            Body.PlayerSide = Body.IsLeftSide ? 1 : -1;

            UpdateFrameProperties();
            StateTransitions();
            AnimationEvents();
            UpdateHitboxes();
        }

        private void CheckDeathConditions()
        {
            if ((DieOnWalls && Body.IsOnWall) || (DieOnGround && Body.IsOnGround) || !LifeTime.IsRunning())
            { LifeTime.Stop(); Animator.PlayState(DeathState); }
        }

        public void HitConfirm(int superGaugeGain, uint hitStopDuration, int hitConfirmAnimation, int hitEffect, Vector2Int VFXSpawn)
        {
            Debug.Log(hitEffect);
            GetFighterOwner().LayerSorting = 1;
            GetFighterOwner().Body.IsMovable = false;
            Body.HitConfirmed = true;
            Body.IsMovable = false;
            GetFighterOwner().Variables.AddSuperGauge(superGaugeGain);
            GetFighterOwner().HitStop.Play(hitStopDuration);
            if (hitEffect >= 0)
            {
                GetFighterOwner().SpawnVFX(hitEffect, VFXSpawn);
            }

            if (DieOnHit)
            { LifeTime.Stop(); Animator.PlayState(DeathState); }
            else if (hitConfirmAnimation >= 0)
                Animator.PlayState(hitConfirmAnimation, false);
            
            if (Variables != null) Variables.ExtraVariablesOnHit();
        }

        public void BaseDamage(SakugaActor target, HitboxElement box, Vector2Int contact)
        {
            if (!AllowHitCheck(target)) return;

            if (target != GetFighterOwner()) 
                GetFighterOwner().SetOpponent(target.FighterReference());

            SakugaFighter Opp = GetFighterOwner().GetOpponent();

            bool isHitAllowed = !Opp.Body.ContainsFrameProperty((byte)Global.FrameProperties.PROJECTILE_IMUNITY);
            if (!isHitAllowed) return;

            bool HitPosition = box.HitType == Global.HitType.UNBLOCKABLE || 
                                box.HitType == Global.HitType.HIGH && Opp.IsCrouching() || 
                                box.HitType == Global.HitType.LOW && !Opp.IsCrouching();
            if (Opp.Variables.SuperArmor > 0)
            {
                Opp.ArmorHit(box);
                HitConfirm(box.SelfSuperGaugeGain, (uint)box.ClashHitStopDuration, -1, box.ArmorHitEffectIndex, contact);
                Debug.Log("Spawnable: Armor Hit");
            }
            else
            {
                int hitFX;
                if (Opp.IsBlocking() && !HitPosition)
                {
                    hitFX = box.BlockEffectIndex;
                    Opp.BlockHit(box);
                    Debug.Log("Spawnable: Blocked!");
                }
                else
                {
                    hitFX = box.HitEffectIndex;
                    Opp.HitDamage(box, false);
                    Debug.Log("Spawnable: Hit!");
                }
                HitConfirm(box.SelfSuperGaugeGain, (uint)box.SelfHitStopDuration, box.HitConfirmState, hitFX, contact);
            }
        }
        public void HitTrade(HitboxElement box, Vector2Int contact){}
        public void ThrowDamage(SakugaActor target, HitboxElement box, Vector2Int contact){}
        public void ThrowTrade(){}
        public void HitboxClash(HitboxElement box, Vector2Int contact){}
        public void ProjectileClash(HitboxElement box, Vector2Int contact)
        {
            if (DieOnHit)
            { LifeTime.Stop(); Animator.PlayState(DeathState); }
        }
        public void ProjectileDeflect(SakugaActor target, HitboxElement box, Vector2Int contact)
        {
            if (!Deflectable) return;

            if (CurrentHitCheck != 2)
                if (CurrentHitCheck == 1)
                    CurrentHitCheck = 0;
                else
                    CurrentHitCheck = 1;
        
            LifeTime.Play();

            if (DeflectState < 0)
                Animator.PlayState(Animator.CurrentState, true);
            else Animator.PlayState(DeflectState, true);
        
            Body.IsLeftSide = !Body.IsLeftSide;
            Body.PlayerSide = Body.IsLeftSide ? 1 : -1;
        }
        public void CounterHit(SakugaActor target, HitboxElement box, Vector2Int contact){}
        public void ProximityBlock(){}
        public void OnHitboxExit(){}

        public override void Serialize(BinaryWriter bw)
        {
            bw.Write(IsActive);
            bw.Write(CurrentHitCheck);
            Body.Serialize(bw);
            if (Variables != null) Variables.Serialize(bw);
            Animator.Serialize(bw);
            LifeTime.Serialize(bw);

            bw.Write(EventExecuted);
        }
		public override void Deserialize(BinaryReader br)
        {
            IsActive = br.ReadBoolean();
            CurrentHitCheck = br.ReadByte();
            Body.Deserialize(br);
            if (Variables != null) Variables.Deserialize(br);
            Animator.Deserialize(br);
            LifeTime.Deserialize(br);

            EventExecuted = br.ReadBoolean();

            Body.UpdateColliders();
        }
    }
}

using UnityEngine;
using SakugaEngine.Resources;
using System.IO;

namespace SakugaEngine
{
    public class FighterVariables : SakugaVariables
    {
        [SerializeField] private ushort BaseAttack = 100;
        [SerializeField] private ushort BaseDefense = 100;
        [SerializeField] private ushort BaseMinDamageScaling = 35;
        [SerializeField] private ushort BaseMaxDamageScaling = 100;
        [SerializeField] private ushort CornerMinDamageScaling = 45;
        [SerializeField] private ushort CornerMaxDamageScaling = 120;
        
        [HideInInspector] public ushort CurrentAttack;
        [HideInInspector] public ushort CurrentDefense;
        [HideInInspector] public ushort CurrentDamageScaling;
        [HideInInspector] public ushort CurrentBaseDamageScaling;
        [HideInInspector] public ushort CurrentCornerDamageScaling;
        [HideInInspector] public ushort CurrentDamageProration;
        [HideInInspector] public ushort CurrentGravityProration;
        
        [HideInInspector] public int LostHealth;

        public override void Initialize()
        {
            base.Initialize();
            LostHealth = CurrentHealth;

            CurrentAttack = BaseAttack;
            CurrentDefense = BaseDefense;

            CurrentBaseDamageScaling = BaseMaxDamageScaling;
            CurrentCornerDamageScaling = CornerMaxDamageScaling;
            CurrentDamageScaling = CurrentBaseDamageScaling;

            CurrentDamageProration = 100;
            CurrentGravityProration = 100;
        }

        public override void TakeDamage(int damage, int meterGain, bool isKilingBlow)
        {
            base.TakeDamage(damage, meterGain, isKilingBlow);
            if (CurrentHealth == 0)
                LostHealth = 0;
        }

        public void RemoveDamageScaling(ushort value)
        {
            if (CurrentBaseDamageScaling - value < BaseMinDamageScaling)
                CurrentBaseDamageScaling = BaseMinDamageScaling;
            else CurrentBaseDamageScaling -= value;

            if (CurrentCornerDamageScaling - value < CornerMinDamageScaling)
                CurrentCornerDamageScaling = CornerMinDamageScaling;
            else CurrentCornerDamageScaling -= value;
        }

        public void ResetDamageStatus()
        {
            CurrentBaseDamageScaling = BaseMaxDamageScaling;
            CurrentCornerDamageScaling = CornerMaxDamageScaling;
            CurrentDamageProration = 100;
            CurrentGravityProration = 100;
            UpdateLostHealth();
        }

        public void UpdateLostHealth()
        {
            if (LostHealth > CurrentHealth)
                LostHealth -= MaxHealth / 200;
            else if (LostHealth < CurrentHealth)
                LostHealth = CurrentHealth;
        }

        public void CalculateDamageScaling(bool changeCondition)
        {
            if (changeCondition)
                CurrentDamageScaling = CurrentCornerDamageScaling;
            else
                CurrentDamageScaling = CurrentBaseDamageScaling;
        }

        public int CalculateCompleteDamage(int damage, int attackValue)
        {
            var damageFactor = attackValue - (CurrentDefense - 100);
            var scaledDamage = damage * CurrentDamageScaling / 100;
            return scaledDamage * damageFactor / 100;
        }

        public override void Serialize(BinaryWriter bw)
        {
            base.Serialize(bw);

            bw.Write(LostHealth);
            bw.Write(CurrentAttack);
            bw.Write(CurrentDefense);
            bw.Write(CurrentDamageScaling);
            bw.Write(CurrentBaseDamageScaling);
            bw.Write(CurrentCornerDamageScaling);
            bw.Write(CurrentDamageProration);
            bw.Write(CurrentGravityProration);
        }

        public override void Deserialize(BinaryReader br)
        {
            base.Deserialize(br);

            LostHealth = br.ReadInt32();
            CurrentAttack = br.ReadUInt16();
            CurrentDefense = br.ReadUInt16();
            CurrentDamageScaling = br.ReadUInt16();
            CurrentBaseDamageScaling = br.ReadUInt16();
            CurrentCornerDamageScaling = br.ReadUInt16();
            CurrentDamageProration = br.ReadUInt16();
            CurrentGravityProration = br.ReadUInt16();
        }
    }
}

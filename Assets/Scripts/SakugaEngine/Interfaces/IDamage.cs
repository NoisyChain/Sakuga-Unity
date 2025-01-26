using UnityEngine;
using SakugaEngine.Resources;

namespace SakugaEngine
{
    public partial interface IDamage
    {
        void BaseDamage(SakugaActor target, HitboxElement box, Vector2Int contact);
        void HitTrade(HitboxElement box, Vector2Int contact);
        void ThrowDamage(SakugaActor target, HitboxElement box, Vector2Int contact);
        void ThrowTrade();
        void HitboxClash(HitboxElement box, Vector2Int contact);
        void ProjectileClash(HitboxElement box, Vector2Int contact);
        void ProjectileDeflect(SakugaActor target, HitboxElement box, Vector2Int contact);
        void CounterHit(SakugaActor target, HitboxElement box, Vector2Int contact);
        //void ParryHit(SakugaActor target, HitboxElement box, Vector2I contact);
        void ProximityBlock();
        void OnHitboxExit();
    }
}
using UnityEngine;
using SakugaEngine.Collision;
using UnityEngine.UI;

namespace SakugaEngine.Utils
{
    public partial class HitboxViewer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] hitboxGraphics;
        [SerializeField] private PhysicsBody body;

        public void Update()
        {
            if (body == null) return;

            UpdateHitboxes();
            UpdatePushbox();
        }

        public void UpdateHitboxes()
        {
            for(int j = 0; j < hitboxGraphics.Length; j++)
            {
                if (body.CurrentHitbox < 0 || j >= body.GetCurrentHitbox().Hitboxes.Length)
                    hitboxGraphics[j].gameObject.SetActive(false);
                else
                {
                    hitboxGraphics[j].gameObject.SetActive(body.Hitboxes[j].Active);

                    switch (body.GetCurrentHitbox().Hitboxes[j].HitboxType)
                    {
                        case Global.HitboxType.HURTBOX:
                            hitboxGraphics[j].sortingOrder = 1;
                            hitboxGraphics[j].color = new Color(0.0f, 1.0f, 0.0f);
                            break;
                        case Global.HitboxType.HITBOX:
                            hitboxGraphics[j].sortingOrder = 2;
                            hitboxGraphics[j].color = new Color(1.0f, 0.0f, 0.0f);
                            break;
                        case Global.HitboxType.PROJECTILE:
                            hitboxGraphics[j].sortingOrder = 2;
                            hitboxGraphics[j].color = new Color(1.0f, 0.64f, 0.0f);
                            break;
                        case Global.HitboxType.PROXIMITY_BLOCK:
                            hitboxGraphics[j].sortingOrder = 4;
                            hitboxGraphics[j].color = new Color(1.0f, 0.0f, 1.0f);
                            break;
                        case Global.HitboxType.THROW:
                            hitboxGraphics[j].sortingOrder = 2;
                            hitboxGraphics[j].color = new Color(0.0f, 0.0f, 1.0f);
                            break;
                        case Global.HitboxType.COUNTER:
                            hitboxGraphics[j].sortingOrder = 2;
                            hitboxGraphics[j].color = new Color(0.5f, 0.5f, 0.5f);
                            break;
                        case Global.HitboxType.DEFLECT:
                            hitboxGraphics[j].sortingOrder = 2;
                            hitboxGraphics[j].color = new Color(1.0f, 0.0f, 0.5f);
                            break;
                        /*case Global.HitboxType.PARRY:
                            hitboxGraphics[j].SortingOffset = 2;
                            hitboxGraphics[j].Modulate = new Color(0.5f, 0.5f, 0.5f);
                            break;*/
                    }
                    //hitboxGraphics[j].material.SetColor("_Tint", hitboxGraphics[j].color);
                    hitboxGraphics[j].transform.position = Global.ToScaledVector3(body.Hitboxes[j].Center);
                    hitboxGraphics[j].size = Global.ToScaledVector2(body.Hitboxes[j].Size);
                }
            }
        }

        public void UpdatePushbox()
        {
            int collisionViewer = hitboxGraphics.Length - 1;
            hitboxGraphics[collisionViewer].gameObject.SetActive(body.Pushbox.Active);
            hitboxGraphics[collisionViewer].sortingOrder = 3;
            hitboxGraphics[collisionViewer].color = new Color(1.0f, 1.0f, 0.0f);
            //hitboxGraphics[collisionViewer].material.SetColor("_Tint", hitboxGraphics[collisionViewer].color);
            hitboxGraphics[collisionViewer].transform.position = Global.ToScaledVector3(body.Pushbox.Center);
            hitboxGraphics[collisionViewer].size = Global.ToScaledVector2(body.Pushbox.Size);
        }
    }
}

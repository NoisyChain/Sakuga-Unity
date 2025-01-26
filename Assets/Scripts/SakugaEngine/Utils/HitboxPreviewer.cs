using UnityEngine;
using SakugaEngine.Resources;
using Unity.VisualScripting;

namespace SakugaEngine.Utils
{
    public partial class HitboxPreviewer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] hitboxGraphics;
        [SerializeField] private HitboxSettings previewSettings;
        
        public void Update()
        {
            if (previewSettings == null)
            {
                for(int j = 0; j < hitboxGraphics.Length; j++)
                    hitboxGraphics[j].gameObject.SetActive(false);
                return;
            }

            PreviewHitboxes();
            PreviewPushbox();
        }

        public void PreviewHitboxes()
        {
            for(int j = 0; j < hitboxGraphics.Length; j++)
            {
                if (j >= previewSettings.Hitboxes.Length)
                    hitboxGraphics[j].gameObject.SetActive(false);
                else
                {
                    hitboxGraphics[j].gameObject.SetActive(previewSettings.Hitboxes[j].Size != Vector2Int.zero);

                    switch (previewSettings.Hitboxes[j].HitboxType)
                    {
                        case Global.HitboxType.HURTBOX:
                            hitboxGraphics[j].sortingOrder = 1;
                            hitboxGraphics[j].color = new Color(0.0f, 1.0f, 0.0f);
                            break;
                        case Global.HitboxType.HITBOX:
                            hitboxGraphics[j].sortingOrder = 2;
                            hitboxGraphics[j].color = new Color(1.0f, 0.0f, 0.0f);
                            break;
                        case Global.HitboxType.PROXIMITY_BLOCK:
                            hitboxGraphics[j].sortingOrder = 4;
                            hitboxGraphics[j].color = new Color(1.0f, 0.0f, 1.0f);
                            break;
                        case Global.HitboxType.PROJECTILE:
                            hitboxGraphics[j].sortingOrder = 2;
                            hitboxGraphics[j].color = new Color(1.0f, 0.64f, 0.0f);
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
                    hitboxGraphics[j].transform.position = Global.ToScaledVector3(previewSettings.Hitboxes[j].Center);
                    hitboxGraphics[j].transform.localScale = Global.ToScaledVector3(previewSettings.Hitboxes[j].Size, 1f);
                }
            }
        }

        public void PreviewPushbox()
        {
            int collisionViewer = hitboxGraphics.Length - 1;
            hitboxGraphics[collisionViewer].gameObject.SetActive(previewSettings.PushboxSize != Vector2Int.zero);
            hitboxGraphics[collisionViewer].sortingOrder = 3;
            hitboxGraphics[collisionViewer].color = new Color(1.0f, 1.0f, 0.0f);
            hitboxGraphics[collisionViewer].transform.position = Global.ToScaledVector3(previewSettings.PushboxCenter);
            hitboxGraphics[collisionViewer].transform.localScale = Global.ToScaledVector3(previewSettings.PushboxSize, 1f);
        }
    }
}

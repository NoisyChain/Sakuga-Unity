using UnityEngine;
using System.IO;

namespace SakugaEngine
{
    public partial class SakugaVFX : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int Duration;
        [SerializeField] private Animator Player;
        [SerializeField] private Transform Graphics;
        [SerializeField] private string AnimationName;
        [SerializeField] private SoundQueue Sound;
        [HideInInspector] public bool IsActive;
        [HideInInspector] public Vector2Int FixedPosition;
        [HideInInspector] public int Frame;
        [HideInInspector] public int Side;

        public void Update()
        {
            transform.position = Global.ToScaledVector3(FixedPosition);
            Graphics.localScale = new Vector3(Side, 1, 1);
            Graphics.gameObject.SetActive(IsActive);
            Player.Play(AnimationName, 0, Frame / (float)Duration);
        }
        
        public void Initialize()
        {
            FixedPosition = Vector2Int.zero;
            Side = 0;
            Frame = -1;
            IsActive = false;
        }
        public void Spawn(Vector2Int origin, int side)
        {
            FixedPosition = origin;
            Side = side;
            Frame = -1;
            //Sound.Stop();
            Sound.SimpleQueueSound();
            IsActive = true;
        }
        public void Tick()
        {
            if (!IsActive) return;

            Frame++;
            if (Frame >= Duration - 1) IsActive = false;
        }

        public void Serialize(BinaryWriter bw)
        {
            bw.Write(FixedPosition.x);
            bw.Write(FixedPosition.y);
            bw.Write(IsActive);
            bw.Write(Frame);
            bw.Write(Side);
        }

        public void Deserialize(BinaryReader br)
        {
            FixedPosition.x = br.ReadInt32();
            FixedPosition.y = br.ReadInt32();
            IsActive = br.ReadBoolean();
            Frame = br.ReadInt32();
            Side = br.ReadInt32();
        }
    }
}

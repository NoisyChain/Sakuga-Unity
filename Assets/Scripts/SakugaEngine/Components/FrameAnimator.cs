using UnityEngine;
using SakugaEngine.Resources;
using System.IO;

namespace SakugaEngine
{
    public class FrameAnimator : MonoBehaviour
    {
        [SerializeField] private Animator[] players;
        [SerializeField] private string[] prefix;
        [SerializeField] public FighterState[] States;

        [HideInInspector] public int CurrentState;
        [HideInInspector] public int Frame;

        public void Update()
        {
            for (int a = 0; a < players.Length; a++)
            {
                if (GetCurrentState().StateName != "")
                    players[a].Play(prefix[a] + GetCurrentState().StateName, 0, Frame / (float)GetCurrentState().Duration);
            }
        }

        public void PlayState(int state, bool reset = false)
        {
            if (CurrentState != state)
            {
                CurrentState = state;
                Frame = 0;
                
            }
            else if (reset)
                Frame = 0;
        }

        public void RunState()
        {
            Frame++;
            LoopState();
        }

        public void LoopState()
        {
            bool canLoop = GetCurrentState().Loop && GetCurrentState().LoopFrames.x >= 0 && GetCurrentState().LoopFrames.y > 0;
            int frameLimit = canLoop ? GetCurrentState().LoopFrames.y : GetCurrentState().Duration - 1;
            if (Frame > frameLimit)
            {
                if (canLoop)
                    Frame = GetCurrentState().LoopFrames.x;
                else
                    Frame = frameLimit;
            }
        }

        public void Serialize(BinaryWriter bw)
        {
            bw.Write(Frame);
            bw.Write(CurrentState);

        }

        public void Deserialize(BinaryReader br)
        {
            Frame = br.ReadInt32();
            CurrentState = br.ReadInt32();
        }

        public FighterState GetCurrentState() => States[CurrentState];
        public int StateType() => (int)GetCurrentState().Type;
    }
}

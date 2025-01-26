using UnityEngine;
using System.IO;
using PleaseResync;
using SakugaEngine.Collision;
using SakugaEngine.UI;
using System.Text;
using TMPro;

namespace SakugaEngine.Game
{
    public partial class GameManager : MonoBehaviour, IGameState
    {
        [SerializeField] private GameMonitor Monitor;
        [SerializeField] public GameObject[] Spawns;
        //[SerializeField] private CanvasLayer FighterUI;
        [SerializeField] private FighterCamera Camera;
        [SerializeField] TextMeshProUGUI SeedViewer;
        public uint InputSize;

        private SakugaFighter[] Fighters;
        private PhysicsWorld World;
        
        [SerializeField] private HealthHUD healthHUD;
        [SerializeField] private MetersHUD metersHUD;

        private int Frame = 0;
        private int generatedSeed = 0;
        private int finalSeed = 0;

        Vector3Int randomTest = new Vector3Int();

        public void Awake()
        {
            //healthHUD = (HealthHUD)FighterUI.GetNode("GameHUD_Background");
            //metersHUD = (MetersHUD)FighterUI.GetNode("GameHUD_Foreground");
        }

        public void Update()
        {
            if (Fighters == null) return;
            if (Monitor == null) return;

            healthHUD.UpdateHealthBars(Fighters, Monitor);
            metersHUD.UpdateMeters(Fighters);
            Camera.UpdateCamera(Fighters[0].transform, Fighters[1].transform);
            SeedViewer.text = finalSeed.ToString();
        }

        /// <summary>
        /// Generates the base seed to be used for generating the PRNG. The base seed is a non-random 
        /// number generated with a default string and both characters' names.
        /// </summary>
        private void GenerateBaseSeed()
        {
            string seedText = Global.baseSeed + Fighters[0].Profile.FighterName + Fighters[1].Profile.FighterName;
            byte[] seedArray = Encoding.ASCII.GetBytes(seedText);
            generatedSeed = (int)Platform.GetChecksum(seedArray);
        }

        /// <summary>
        /// Generate the PRNG seed with a bunch of everchanging values. 
        /// If the values used are deterministic, the generated seed will be deterministic.
        /// </summary>
        /// <returns>a 32-bit seed number</returns>
        private int CalculateSeed()
        {
            int posX = Fighters[0].Body.FixedPosition.x + Fighters[1].Body.FixedPosition.x;
            int posY = Fighters[0].Body.FixedPosition.y + Fighters[1].Body.FixedPosition.y;
            int stateFrame = Fighters[0].Animator.Frame + Fighters[0].Animator.CurrentState + Fighters[1].Animator.Frame + Fighters[1].Animator.CurrentState;
            return generatedSeed + posX + posY + stateFrame + (Frame * Global.SimulationScale) + Monitor.Clock;
        }

        public void Setup()
        {
            if (transform.childCount > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i));
                }
            }

            Frame = 0;

            World = new PhysicsWorld();

            Fighters = new SakugaFighter[2];
            for (int i = 0; i < Spawns.Length; i++)
            {
                GameObject temp = Instantiate(Spawns[i], transform);
                Fighters[i] = temp.GetComponent<SakugaFighter>();
                World.AddBody(Fighters[i].Body);
                Fighters[i].Initialize(i);
                Fighters[i].SpawnablesSetup(transform, World);
                Fighters[i].VFXSetup(transform);
            }

            Fighters[0].SetOpponent(Fighters[1]);
            Fighters[1].SetOpponent(Fighters[0]);

            GenerateBaseSeed();
            Monitor.Initialize(Fighters);

            healthHUD.Setup(Fighters);
            metersHUD.Setup(Fighters);
        }

        public void GameLoop(byte[] playerInput)
        {
            finalSeed = CalculateSeed();
            Global.UpdateRNG(finalSeed);
            Frame++;
            Monitor.Tick();

            randomTest = new Vector3Int(
                Global.RNG.Next(),
                Global.RNG.Next(),
                Global.RNG.Next()
            );

            int center = (Fighters[0].Body.FixedPosition.x + Fighters[1].Body.FixedPosition.x) / 2;

            for (int i = 0; i < Fighters.Length; i++)
            {
                ushort combinedInput = 0;
                combinedInput |= playerInput[i * InputSize];
                combinedInput |= (ushort)(playerInput[(i * InputSize) + 1] << 8);
                Fighters[i].ParseInputs(combinedInput);
            }

            for (int i = 0; i < Fighters.Length; i++)
                Fighters[i].PreTick();
            
            for (int i = 0; i < Fighters.Length; i++)
                Fighters[i].Tick();
            
            World.Simulate();

            if (Fighters[0].Body.FixedPosition.x < Fighters[1].Body.FixedPosition.x)
            { Fighters[0].UpdateSide(true); Fighters[1].UpdateSide(false); }
            else if (Fighters[0].Body.FixedPosition.x > Fighters[1].Body.FixedPosition.x)
            { Fighters[0].UpdateSide(false); Fighters[1].UpdateSide(true); }

            for (int i = 0; i < Fighters.Length; i++)
                Fighters[i].LateTick();

            for (int i = 0; i < Fighters.Length; i++)
            {
                Fighters[i].Body.FixedPosition.x = Mathf.Clamp(
                    Fighters[i].Body.FixedPosition.x,
                    center - Global.MaxPlayersDistance / 2,
                    center + Global.MaxPlayersDistance / 2
                );
                Fighters[i].Body.UpdateColliders();
            }
        }

        // Generate inputs for your game
        //NOTICE: for every 8 inputs you need to change the index
        public byte[] ReadInputs(int id, uint inputSize)
        {
            byte[] input = new byte[inputSize];
            string prexif = "";

            switch (id)
            {
                case 0:
                    prexif = "k1";
                    break;
                case 1:
                    prexif = "k2";
                    break;
            }

            if (Input.IsActionPressed(prexif + "_up") && !Input.IsActionPressed(prexif + "_down"))
                    input[0] |= Global.INPUT_UP;

                if (!Input.IsActionPressed(prexif + "_up") && Input.IsActionPressed(prexif + "_down"))
                    input[0] |= Global.INPUT_DOWN;

                if (Input.IsActionPressed(prexif + "_left") && !Input.IsActionPressed(prexif + "_right"))
                    input[0] |= Global.INPUT_LEFT;

                if (!Input.IsActionPressed(prexif + "_left") && Input.IsActionPressed(prexif + "_right"))
                    input[0] |= Global.INPUT_RIGHT;

                if (Input.IsActionPressed(prexif + "_face_a"))
                    input[0] |= Global.INPUT_FACE_A;

                if (Input.IsActionPressed(prexif + "_face_b"))
                    input[0] |= Global.INPUT_FACE_B;

                if (Input.IsActionPressed(prexif + "_face_c"))
                    input[0] |= Global.INPUT_FACE_C;

                if (Input.IsActionPressed(prexif + "_face_d"))
                    input[0] |= Global.INPUT_FACE_D;

                /*if (Input.IsActionPressed(prexif + "_macro_ab"))
                    input |= Global.INPUT_FACE_A | Global.INPUT_FACE_B;

                if (Input.IsActionPressed(prexif + "_macro_ac"))
                    input |= Global.INPUT_FACE_A | Global.INPUT_FACE_C;
                
                if (Input.IsActionPressed(prexif + "_macro_bc"))
                    input |= Global.INPUT_FACE_B | Global.INPUT_FACE_C;

                if (Input.IsActionPressed(prexif + "_macro_abc"))
                    input |= Global.INPUT_FACE_A | Global.INPUT_FACE_B | Global.INPUT_FACE_C;

                if (Input.IsActionPressed(prexif + "_macro_abcd"))
                    input |= Global.INPUT_FACE_A | Global.INPUT_FACE_B | Global.INPUT_FACE_C | Global.INPUT_FACE_D;*/
            
            
            return input;
        }

        public void SaveState(BinaryWriter bw)
        {
            bw.Write(Frame);
            Monitor.Serialize(bw);
            for (int i = 0; i < Fighters.Length; i++)
                Fighters[i].Serialize(bw);
            
            bw.Write(randomTest.x);
            bw.Write(randomTest.y);
            bw.Write(randomTest.z);
        }

        public void LoadState(BinaryReader br)
        {
            Frame = br.ReadInt32();
            Monitor.Deserialize(br);
            for (int i = 0; i < Fighters.Length; i++)
                Fighters[i].Deserialize(br);
            
            randomTest.x = br.ReadInt32();
            randomTest.y = br.ReadInt32();
            randomTest.z = br.ReadInt32();
        }

        public byte[] GetLocalInput(int PlayerID)
        {
            byte[] result = ReadInputs(PlayerID, InputSize);

            return result;
        }
    }
}

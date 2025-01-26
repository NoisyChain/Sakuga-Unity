namespace SakugaEngine.Resources
{
    [System.Serializable]
    public partial class InputSequence
    {
        public Global.DirectionalInputs Directional = Global.DirectionalInputs.NEUTRAL;
        public Global.ButtonInputs Buttons = Global.ButtonInputs.NULL;
        public Global.ButtonMode DirectionalMode;
        public Global.ButtonMode ButtonMode;
    }
}

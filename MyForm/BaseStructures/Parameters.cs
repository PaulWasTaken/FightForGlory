namespace Game
{
    public class BlockState
    {
        public bool Blocking;
        public BlockSide Side;

        public BlockState()
        {
            Blocking = false;
        }
    }
    public enum ComboName
    {
        Default,
        ThrowSpear,
        HolyLight,
        LightningAttack,
        Teleport,
        DevastatingCharge
    }

    public enum PlayerNumber
    {
        FirstPlayer,
        SecondPlayer
    }
    public enum BattleStance
    {
        Attack = 0,
        Block = 1
    }

    public enum BlockSide
    {
        Left,
        Right
    }

    public enum FighterMotionState
    {
        NotMoving,
        MovingLeft,
        MovingRight
    }
}
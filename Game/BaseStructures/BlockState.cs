using Game.BaseStructures.Enums;

namespace Game.BaseStructures
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
}
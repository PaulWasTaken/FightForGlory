using Game.Fighters;
using Game.GameInformation;

namespace Game.Factories
{
    public class GameStateFactory
    {
        public GameState Create(Fighter firstFighter, Fighter secondFighter)
        {
            return new GameState(firstFighter, secondFighter);
        }
    }
}

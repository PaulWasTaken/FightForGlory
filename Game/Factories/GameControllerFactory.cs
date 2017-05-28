using Game.Controllers;
using Game.GameInformation;

namespace Game.Factories
{
    public class GameControllerFactory
    {
        public GameController Create(GameSettings settings, GameState state)
        {
            return new GameController(settings, state);
        }
    }
}

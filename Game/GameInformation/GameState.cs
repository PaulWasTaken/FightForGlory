using System;
using System.Collections.Generic;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;

namespace Game.GameInformation
{
    public class GameState
    {
        public GameState(Fighter first, Fighter second)
        {
            FirstPlayer = first;
            SecondPlayer = second;
            Lost = Tuple.Create(false, "");
            GameObjects = new List<GameObject>();
            SpecialStrikes = new List<SpecialStrike>();

            FirstPlayer.KnowYourEnemy(SecondPlayer);
            FirstPlayer.Number = PlayerNumber.FirstPlayer;
            SecondPlayer.KnowYourEnemy(FirstPlayer);
            SecondPlayer.Number = PlayerNumber.SecondPlayer;
            Fighters = new List<Fighter> { FirstPlayer, SecondPlayer };
        }
        public Fighter FirstPlayer { get; set; }
        public Fighter SecondPlayer { get; set; }
        public List<GameObject> GameObjects { get; set; }
        public List<SpecialStrike> SpecialStrikes { get; set; }

        public List<Fighter> Fighters;

        public Tuple<bool, string> Lost;
        public bool Finished;
    }
}

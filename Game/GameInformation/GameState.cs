using System;
using System.Collections.Generic;
using Game.BaseStructures;
using Game.BaseStructures.Enums;
using Game.Fighters;
using Game.GameObjects;

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
            SpecialStrikes = new List<ISpecialStrike>();

            Opponent = new Dictionary<PlayerNumber, Fighter>
            {
                {PlayerNumber.FirstPlayer, SecondPlayer},
                {PlayerNumber.SecondPlayer, FirstPlayer}
            };


            Fighters = new List<Fighter> {FirstPlayer, SecondPlayer};
        }

        public Fighter GetOpponent(PlayerNumber number)
        {
            return Opponent[number];
        }

        public Fighter FirstPlayer { get; set; }
        public Fighter SecondPlayer { get; set; }
        public List<GameObject> GameObjects { get; set; }
        public List<ISpecialStrike> SpecialStrikes { get; set; }

        private Dictionary<PlayerNumber, Fighter> Opponent { get; }

        public List<Fighter> Fighters;

        public Tuple<bool, string> Lost;
        public bool Finished;
    }
}

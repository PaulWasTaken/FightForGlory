using System;
using System.Collections.Generic;
using System.Linq;
using Game.BaseStructures.ComboWorker;
using Game.BaseStructures.Enums;
using Game.Commands;
using Game.GameObjects;

namespace Game.Controllers
{
    public class ComboController
    {
        private readonly AutomatNode<Command> defaultState = new AutomatNode<Command>(ComboName.NotACombo);
        public AutomatNode<Command> CurrentState { get; private set; }
        private readonly Dictionary<ComboName, Func<GameObject>> comboResults;
        public bool ComboCompleted => comboResults.ContainsKey(CurrentState.Name);

        public ComboController(Dictionary<ComboName, Func<GameObject>> comboResults)
        {
            this.comboResults = comboResults;
            CurrentState = defaultState;
        }
        public void AddCombo(Command[] combo, ComboName name)
        {
            var current = defaultState;
            for(var i = 0; i < combo.Length - 1; i++)
            {
                current.NextStates.Add(new AutomatNode<Command>(combo[i], ComboName.NotACombo));
                current = current.NextStates.Last();
            }
            current.NextStates.Add(new AutomatNode<Command>(combo.Last(), name));
        }

        public void UpdateState(Command move)
        {
            var nextState = CurrentState.NextStates.FirstOrDefault(n => n.Value.Equals(move));
            CurrentState = nextState ?? defaultState;
        }

        public GameObject PerformCombo()
        {
            var obj = comboResults[CurrentState.Name]();
            CurrentState = defaultState;
            return obj;
        }
    }
}

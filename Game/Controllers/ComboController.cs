using System;
using System.Collections.Generic;
using System.Linq;
using Game.BaseStructures;
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
            var position = 0;
            SetBaseValues(combo, ref current, ref position);
            for (; position < combo.Length - 1; position++)
            {
                current.NextStates.Add(new AutomatNode<Command>(combo[position], ComboName.NotACombo));
                current = current.NextStates.Last();
            }
            current.NextStates.Add(new AutomatNode<Command>(combo.Last(), name));
        }

        private void SetBaseValues(Command[] combo, ref AutomatNode<Command> current, ref int start)
        {
            foreach (var command in combo)
            {
                var next = current.NextStates.FirstOrDefault(node => node.Value.Equals(command));
                if (next == null) return;
                current = next;
                start++;
            }
        }

        public void UpdateState(Command move)
        {
            var nextState = CurrentState.NextStates.FirstOrDefault(n => n.Value.Equals(move));
            if (nextState == null)
                CurrentState = defaultState.NextStates.FirstOrDefault(n => n.Value.Equals(move)) ?? defaultState;
            else
                CurrentState = nextState;
        }

        public GameObject PerformCombo()
        {
            var obj = comboResults[CurrentState.Name]();
            CurrentState = defaultState;
            return obj;
        }
    }
}

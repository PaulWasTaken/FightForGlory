using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Game.BaseStructures.Enums;

namespace Game.BaseStructures.ComboDetector
{
    public class ComboDetector
    {
        public string Name { get; set; }
        public Node DefaultState { get; set; }
        public Node CurrentState { get; set; }
        private readonly HashSet<Keys> includedTops = new HashSet<Keys>();
        public HashSet<Keys> IncludedValues { get; }

        public ComboDetector(string name)
        {
            DefaultState = new Node { Name = ComboName.Default };
            CurrentState = DefaultState;
            IncludedValues = new HashSet<Keys>();
        }

        public void Add(Keys[] combo, ComboName name)
        {
            DefaultState.NextState.Add(new Node(combo[0], name));
            includedTops.Add(combo[0]);
            IncludedValues.Add(combo[0]);
            var current = DefaultState.NextState.Last();
            for (var i = 1; i < combo.Length; i++)
            {
                IncludedValues.Add(combo[i]);
                current.NextState.Add(new Node(combo[i], name));
                current = current.NextState.Last();
            }
        }

        public bool CheckState(Keys value)
        {
            foreach (var state in CurrentState.NextState)
            {
                CurrentState = state.Value.CompareTo(value) == 0 ? state : DefaultState;
            }
            return value.CompareTo(CurrentState.Value) == 0 && CurrentState.NextState.Count == 0;
        }

        public void FindValue(Keys value)
        {
            if (!includedTops.Contains(value))
                return;
            foreach (var state in DefaultState.NextState)
            {
                if (state.Value.CompareTo(value) == 0)
                    CurrentState = state;
            }
        }
    }
}

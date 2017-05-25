using System;
using System.Collections.Generic;
using System.Linq;
using Game.BaseStructures.Enums;

namespace Game.BaseStructures.ComboWorker
{
    public class ComboDetector<T>
    {
        public string Name { get; set; }
        public Node<T> DefaultState { get; set; }
        public Node<T> CurrentState { get; set; }
        private readonly HashSet<T> includedTops = new HashSet<T>();
        public HashSet<T> IncludedValues { get; }

        public ComboDetector()
        {
            DefaultState = new Node<T> { Name = ComboName.Default };
            CurrentState = DefaultState;
            IncludedValues = new HashSet<T>();
        }

        public void Add(T[] combo, ComboName name)
        {
            DefaultState.NextState.Add(new Node<T>(combo[0], name));
            includedTops.Add(combo[0]);
            IncludedValues.Add(combo[0]);
            var current = DefaultState.NextState.Last();
            for (var i = 1; i < combo.Length; i++)
            {
                IncludedValues.Add(combo[i]);
                current.NextState.Add(new Node<T>(combo[i], name));
                current = current.NextState.Last();
            }
        }

        public bool CheckState(T value)
        {
            foreach (var state in CurrentState.NextState)
            {
                CurrentState = state.Value.Equals(value) ? state : DefaultState;
            }
            return value.Equals(CurrentState.Value) && CurrentState.NextState.Count == 0;
        }

        public void FindValue(T value)
        {
            if (!includedTops.Contains(value))
                return;
            foreach (var state in DefaultState.NextState)
            {
                if (state.Value.Equals(value))
                    CurrentState = state;
            }
        }
    }
}

using System.Collections.Generic;
using Game.BaseStructures.Enums;

namespace Game.BaseStructures.ComboWorker
{
    public class AutomatNode<T>
    {
        public ComboName Name { get; }
        public List<AutomatNode<T>> NextStates { get; }
        public T Value { get; }

        public AutomatNode(T value, ComboName name)
        {
            Value = value;
            NextStates = new List<AutomatNode<T>>();
            Name = name;
        }

        public AutomatNode(ComboName name)
        {
            Name = name;
            NextStates = new List<AutomatNode<T>>();
        }
    }
}

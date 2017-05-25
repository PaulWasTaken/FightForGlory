using System.Collections.Generic;
using Game.BaseStructures.Enums;

namespace Game.BaseStructures.ComboWorker
{
    public class Node<T>
    {
        public ComboName Name { get; set; }
        public List<Node<T>> NextState { get; set; }
        public T Value { get; set; }

        public Node(T value, ComboName name)
        {
            Value = value;
            NextState = new List<Node<T>>();
            Name = name;
        }

        public Node()
        {
            NextState = new List<Node<T>>();
        }
    }
}

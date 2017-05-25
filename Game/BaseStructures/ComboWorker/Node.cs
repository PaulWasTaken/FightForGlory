using System.Collections.Generic;
using System.Windows.Forms;
using Game.BaseStructures.Enums;

namespace Game.BaseStructures.ComboWorker
{
    public class Node
    {
        public ComboName Name { get; set; }
        public List<Node> NextState { get; set; }
        public Keys Value { get; set; }

        public Node(Keys value, ComboName name)
        {
            Value = value;
            NextState = new List<Node>();
            Name = name;
        }

        public Node()
        {
            NextState = new List<Node>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Game.BaseStructures.Enums;
using Game.Controllers;
using Game.GameObjects;

namespace Game.Fighters
{
    public class Unicorn : Fighter
    {
        public Unicorn(string name, PointF location) : base(name, location) { }

        public override ComboController GetComboController()
        {
            return new ComboController(new Dictionary<ComboName, Func<GameObject>>());
        }

        protected override int AttackCooldownValue => 400;
        protected override int BlockCooldownValue => 600;
        protected override float ManaRegenerationAmount => 0.2f;
    }
}

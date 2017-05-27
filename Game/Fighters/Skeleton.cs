using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Game.BaseStructures.Enums;
using Game.Commands;
using Game.Controllers;
using Game.GameObjects;

namespace Game.Fighters
{
    public class Skeleton : Fighter
    {
        public Skeleton(string name, PointF location) : base(name, location) { }
        public override ComboController GetComboController()
        {
            var comboResults = new Dictionary<ComboName, Func<GameObject>>
            {
                [ComboName.ThrowSpear] = () =>
                {
                    if (ManaPoints < 40) return null;
                    ManaPoints -= 40;
                    return new Spear(Body, LookingRight, Number);
                }
            };

            var controller = new ComboController(comboResults);
            controller.AddCombo(new[] { Command.NormalAttack, Command.Down, Command.StrongAttack }, ComboName.ThrowSpear);

            return controller;
        }

        protected override int AttackCooldownValue => 250;
        protected override int BlockCooldownValue => 500;
        protected override float ManaRegenerationAmount => 0.2f;
    }
}

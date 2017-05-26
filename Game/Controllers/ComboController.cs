using System;
using System.Collections.Generic;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.ComboWorker;
using Game.BaseStructures.Enums;
using Game.Commands;

namespace Game.Controllers
{
    public class ComboController
    {
        private readonly ComboDetector<Command> comboDetector;
        private readonly Dictionary<ComboName, Func<GameObject>> comboPerformer;

        public ComboController(ComboDetector<Command> comboDetector, Dictionary<ComboName, Func<GameObject>> comboPerformer)
        {
            this.comboDetector = comboDetector;
            this.comboPerformer = comboPerformer;
        }
        public bool CheckForCombo(Command command)
        {
            if (comboDetector.CurrentState.Name != ComboName.Default)
                return comboDetector.CheckState(command);
            comboDetector.FindValue(command);
            return false;
        }

        public GameObject PerformCombo()
        {
            var obj = comboPerformer[comboDetector.CurrentState.Name]();
            comboDetector.CurrentState = comboDetector.DefaultState;
            return obj;
        }
    }
}

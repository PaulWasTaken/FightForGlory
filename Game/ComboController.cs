using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.ComboWorker;
using Game.BaseStructures.Enums;

namespace Game
{
    public class ComboController
    {
        private readonly ComboDetector comboDetector;
        private readonly Dictionary<ComboName, Func<GameObject>> comboPerformer;

        public ComboController(ComboDetector comboDetector, Dictionary<ComboName, Func<GameObject>> comboPerformer)
        {
            this.comboDetector = comboDetector;
            this.comboPerformer = comboPerformer;
        }
        public bool CheckForCombo(KeyEventArgs e)
        {
            if (comboDetector.CurrentState.Name == ComboName.Default)
            {
                comboDetector.FindValue(e.KeyData);
                return false;
            }
            if (comboDetector.CheckState(e.KeyData))
                return true;
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

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;

namespace Game
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestTree()
        {
            var automat = new ComboDetector("die");
            automat.Add(new[] { Keys.A, Keys.B, Keys.C }, ComboName.HolyLight);
            automat.Add(new[] { Keys.B, Keys.B, Keys.C }, ComboName.HolyLight);
            automat.Add(new[] { Keys.C, Keys.B, Keys.C }, ComboName.HolyLight);
            automat.FindValue(Keys.A);
            Assert.AreEqual(automat.CheckState(Keys.B), false);
            Assert.AreEqual(automat.CheckState(Keys.C), true);
            Assert.AreEqual(automat.CurrentState.Value, "Default");
        }
    }
}

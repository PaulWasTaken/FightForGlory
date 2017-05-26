using System.Collections.Generic;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;

namespace Game.Controllers
{
    public class CombatController
    {
        private readonly Fighter first;
        private readonly Fighter second;

        public CombatController(Fighter first, Fighter second)
        {
            this.first = first;
            this.second = second;
        }

        public void CheckForCombat(List<GameObject> gameObjects)
        {
            if (first.Attack)
                HandleDamage(first, second);
            if (second.Attack)
                HandleDamage(second, first);
            foreach (var obj in gameObjects)
            {
                
            }   
        }

        private void HandleDamage(Fighter attacker, Fighter defender)
        {
            if (defender.Body.Bottom < attacker.Body.Bottom - attacker.Body.Height / 2)
                return;

            if (attacker.LookRight)
            {
                if (defender.Block.Blocking && defender.Block.Side == BlockSide.Left) return;
                if (defender.Body.Contains(attacker.Body.Right + attacker.AttackRange, attacker.Body.Y + attacker.Body.Height / 4))
                    defender.HealthPoints -= attacker.AttackDamage;
            }
            else
            {
                if (defender.Block.Blocking && defender.Block.Side == BlockSide.Right) return;
                if (defender.Body.Contains(attacker.Body.Left - attacker.AttackRange, attacker.Body.Y + attacker.Body.Height / 4))
                    defender.HealthPoints -= attacker.AttackDamage;
            }
        }

        private void HandleDamage(GameObject obj, Fighter target)
        {
            
        }
    }
}

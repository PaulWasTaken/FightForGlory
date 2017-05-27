using System.Collections.Generic;
using Game.GameInformation;
using Game.Fighters;
using Game.GameObjects;

namespace Game.Controllers
{
    public class CombatController
    {
        private readonly Fighter first;
        private readonly Fighter second;
        private bool wasCompletedFirst;
        private bool wasCompletedSecond;

        public CombatController(Fighter first, Fighter second)
        {
            this.first = first;
            this.second = second;
        }

        public void CheckForCombat(GameState gameState)
        {
            if (first.IsAttacking)
            {
                if (!wasCompletedFirst)
                {
                    HandleDamage(first, second);
                    wasCompletedFirst = true;
                }
            }
            else
                wasCompletedFirst = false;
            if (second.IsAttacking)
            {
                if (!wasCompletedSecond)
                {
                    HandleDamage(second, first);
                    wasCompletedSecond = true;
                }
            }
            else
                wasCompletedSecond = false;

            var toRemove = new List<GameObject>();
            foreach (var obj in gameState.GameObjects)
            {
                HandleDamage(obj, gameState.GetOpponent(obj.Source));
                if (obj.ShouldBeRemoved(gameState.GetOpponent(obj.Source)))
                    toRemove.Add(obj);
            }
            gameState.GameObjects.RemoveAll(item => toRemove.Contains(item));
        }

        private void HandleDamage(Fighter attacker, Fighter defender)
        {
            if (defender.Body.Bottom < attacker.Body.Bottom - attacker.Body.Height / 2)
                return;

            if (attacker.LookingRight)
            {
                if (defender.IsBlocking && !defender.LookingRight) return;
                if (defender.Body.Contains(attacker.Body.Right + attacker.AttackRange, attacker.Body.Y + attacker.Body.Height / 4))
                    defender.TakeDamage(attacker.AttackDamage);
            }
            else
            {
                if (defender.IsBlocking && defender.LookingRight) return;
                if (defender.Body.Contains(attacker.Body.Left - attacker.AttackRange, attacker.Body.Y + attacker.Body.Height / 4))
                    defender.TakeDamage(attacker.AttackDamage);
            }
        }

        private void HandleDamage(GameObject obj, Fighter target)
        {
            if (obj.ShouldDealDamage(target))
                target.TakeDamage(obj.Damage);
        }
    }
}

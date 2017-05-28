using System.Collections.Generic;
using Game.BaseStructures.Enums;
using Game.GameInformation;
using Game.Fighters;
using Game.GameObjects;

namespace Game.Controllers
{
    public class CombatController
    {
        private readonly Dictionary<PlayerNumber, bool> wasProcessed;

        public CombatController(Fighter first, Fighter second)
        {
            wasProcessed = new Dictionary<PlayerNumber, bool> {{first.Number, false}, {second.Number, false}};
        }

        public void CheckForCombat(GameState gameState)
        {
            ProcessCombat(gameState.FirstPlayer, gameState.SecondPlayer);
            ProcessCombat(gameState.SecondPlayer, gameState.FirstPlayer);

            var toRemove = new List<GameObject>();
            foreach (var obj in gameState.GameObjects)
            {
                HandleDamage(obj, gameState.GetOpponent(obj.Source));
                if (obj.ShouldBeRemoved(gameState.GetOpponent(obj.Source)))
                    toRemove.Add(obj);
            }
            gameState.GameObjects.RemoveAll(item => toRemove.Contains(item));
        }

        private void ProcessCombat(Fighter firstFighter, Fighter secondFighter)
        {
            if (!firstFighter.IsAttacking)
            {
                wasProcessed[firstFighter.Number] = false;
                return;
            }
            if (wasProcessed[firstFighter.Number]) return;
            HandleDamage(firstFighter, secondFighter);
            wasProcessed[firstFighter.Number] = true;
        }

        private void HandleDamage(Fighter attacker, Fighter defender)
        {
            if (defender.Body.Bottom < attacker.Body.Bottom - attacker.Body.Height / 2)
                return;

            if (attacker.LookingRight)
            {
                if (defender.IsBlocking && !defender.LookingRight) return;
                if (defender.Body.Contains(attacker.Body.Right + attacker.AttackRange,
                    attacker.Body.Y + attacker.Body.Height / 4))
                    defender.TakeDamage(attacker.AttackDamage);
            }
            else
            {
                if (defender.IsBlocking && defender.LookingRight) return;
                if (defender.Body.Contains(attacker.Body.Left - attacker.AttackRange,
                    attacker.Body.Y + attacker.Body.Height / 4))
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

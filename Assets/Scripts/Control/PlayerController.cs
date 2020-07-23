using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using UnityEngine;
using RPG.Movement;
using RPG.Resources;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover mover;
        private Fighter fighter;
        private Health health;

        void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(ScreenPointToRay());
            foreach (var hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if(target == null) continue;

                GameObject targetGameObject = target.gameObject;
                if (!fighter.CanAttack(targetGameObject))
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(targetGameObject);
                }
                return true;
            }

            return false;
        }
        
    
        public bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ScreenPointToRay(), out hit);
            if(hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(hit.point,1f);
                }
                return true;
            }

            return false;
        }

        private static Ray ScreenPointToRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
    



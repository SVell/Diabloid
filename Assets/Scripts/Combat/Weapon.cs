using System.Runtime.InteropServices;
using RPG.Core;
using RPG.Resources;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [FormerlySerializedAs("weaponPrefab")] [SerializeField] private GameObject equippedPrefab = null;
        [FormerlySerializedAs("weaponOverride")] [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private Projectile projectile = null;
        [SerializeField] private float weaponDamage = 5;
        [SerializeField] private float weaponRange = 2;
        [SerializeField] private bool isRightHanded = true;

        private const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand,Animator animator)
        {
            DestroyOldWeapon(rightHand,leftHand);
            if (equippedPrefab != null)
            {
                GameObject weapon = Instantiate(equippedPrefab, GetTransform(rightHand, leftHand));
                weapon.name = weaponName;
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else
            {
                var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                if (overrideController != null)
                {
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
                }
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if(oldWeapon == null) return;

            oldWeapon.name = "Destroying";
            Destroy(oldWeapon.gameObject);
        }


        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, rightHand).position, Quaternion.identity);
            projectileInstance.SetTarget(instigator,target,weaponDamage);
        }

        public float GetDamage()
        {
            return weaponDamage;
        }
        
        public float GetRange()
        {
            return weaponRange;
        }
    }
}

using System.Runtime.InteropServices;
using RPG.Core;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "WeaponConfig/Make new Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private Weapon equippedPrefab = null;
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private Projectile projectile = null;
        [SerializeField] private float weaponDamage = 5;
        [SerializeField] private float weaponPercentageBonus = 0;
        [SerializeField] private float weaponRange = 2;
        [SerializeField] private bool isRightHanded = true;

        private const string weaponName = "Weapon";

        public Weapon Spawn(Transform rightHand, Transform leftHand,Animator animator)
        {
            DestroyOldWeapon(rightHand,leftHand);

            Weapon weapon = null;
            if (equippedPrefab != null)
            {
                weapon = Instantiate(equippedPrefab, GetTransform(rightHand, leftHand));
                weapon.gameObject.name = weaponName;
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

            return weapon;
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

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, rightHand).position, Quaternion.identity);
            projectileInstance.SetTarget(instigator,target,calculatedDamage);
        }

        public float GetDamage()
        {
            return weaponDamage;
        }

        public float GetPercentageBonus()
        {
            return weaponPercentageBonus;
        }
        
        public float GetRange()
        {
            return weaponRange;
        }
    }
}

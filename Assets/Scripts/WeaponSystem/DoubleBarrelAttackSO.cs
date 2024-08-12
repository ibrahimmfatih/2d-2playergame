using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA.WeaponSystem
{
    [CreateAssetMenu(menuName = "Attacks/DoubleBarrelAttack")]
    public class DoubleBarrelAttackSO : AttackPatternSO
    {
      
        [SerializeField]
        private float offsetFromShootingPoint = 0.3f;


        public override void Perform(Transform shootingStartPoint)
        {
            Vector3 offsetVector = shootingStartPoint.rotation * new Vector3(offsetFromShootingPoint, 0, 0);
            Vector2 point1 = shootingStartPoint.position + offsetVector;
            Vector2 point2 = shootingStartPoint.position - offsetVector;

            Instantiate(projectile, point1, shootingStartPoint.rotation);
            Instantiate(projectile, point2, shootingStartPoint.rotation);
        }
    }
}
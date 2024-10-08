using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace PA.WeaponSystem
{
    [CreateAssetMenu(menuName = "Attacks/BrustAttack")]
    public class BrustAttackSO : AttackPatternSO
    {
       
       
       
        [SerializeField]
        private float offset = 0.1f;
        public override void Perform(Transform shootingStartPoint)
        {
            Vector3 offsetDirection = shootingStartPoint.rotation * new Vector3(0,offset,0);

            Instantiate(projectile, shootingStartPoint.position, shootingStartPoint.rotation);
            Instantiate(projectile, shootingStartPoint.position + offsetDirection, shootingStartPoint.rotation);
            Instantiate(projectile, shootingStartPoint.position - offsetDirection, shootingStartPoint.rotation);
        }
    }
}
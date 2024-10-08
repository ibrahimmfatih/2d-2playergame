using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA.WeaponSystem
{
    [CreateAssetMenu(menuName = "Attacks/DefaultAttack")]
    public class DefaultAttackSO : AttackPatternSO
    {
      

        public override void Perform(Transform shootingStartPoint)
        {
            Instantiate(projectile, shootingStartPoint.position, shootingStartPoint.rotation);
        }
    }
}
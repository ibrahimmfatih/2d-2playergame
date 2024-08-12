using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA.WeaponSystem
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        List<AttackPatternSO> weapons;
        private int index = 0;
        [SerializeField]
        private AudioClip weaponSwap;
        public bool shootingDelayed;
        [SerializeField]
        private AttackPatternSO attackPattern;
        [SerializeField]
        private Transform shootingStartPoint;

        public GameObject projectile;

        public AudioSource gunAudio;

        public void SwapWeapon()
        {
            index++;
            index = index >= weapons.Count ? 0: index;
            attackPattern = weapons[index];
            gunAudio.PlayOneShot(weaponSwap);
        
        }




        public void PerformAttack()
        {
            if (shootingDelayed == false)
            {
                shootingDelayed = true;
                gunAudio.PlayOneShot(attackPattern.AudioSFX);
                //GameObject p = Instantiate(projectile, transform.position, Quaternion.identity);
                attackPattern.Perform(shootingStartPoint); 
                StartCoroutine(DelayShooting());
            }
        
    }

        private IEnumerator DelayShooting()
        {
            yield return new WaitForSeconds(attackPattern.AttackDelay);
            shootingDelayed = false;
        }

    }
}
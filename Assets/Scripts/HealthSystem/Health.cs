using UnityEngine;
using UnityEngine.Events;

namespace PA.HealthSystem
{
    public class Health : MonoBehaviour, IHittable
    {
        [field: SerializeField]
        public int CurrentHealth { get; private set; }
        public UnityEvent OnDeath, OnHit;

        private void Start()
        {
            if (OnDeath == null)
                OnDeath = new UnityEvent();
            if (OnHit == null)
                OnHit = new UnityEvent();
        }

        public void GetHit(int damageValue, GameObject sender)
        {
            CurrentHealth -= damageValue;

            if (CurrentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
            else
            {
                OnHit?.Invoke();
            }
        }

        public void InitializeHealth(int startingHealth)
        {
            if (startingHealth < 0)
                startingHealth = 0;
            CurrentHealth = startingHealth;
        }
    }
}

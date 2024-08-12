using System.Collections;
using UnityEngine;
using PA.HealthSystem;
using UnityEngine.SceneManagement;

public class Boss : Enemy
{
    public float rotationSpeed = 50f;
    private Health bossHealth;
    private Vector3 targetPosition;
    private bool isAtTargetPosition = false;
    public float moveSpeed = 2f;

    private new void Start()
    {
        base.Start();
        targetPosition = new Vector3(0, 0, 0); // Sahnenin ortas�



        bossHealth = GetComponent<Health>();
        if (bossHealth != null)
        {
            bossHealth.OnDeath.AddListener(HandleBossDeath);
        }

        // Boss'un can�n� 100 yap
        bossHealth.InitializeHealth(100);

        // Ate� etme h�z�n� artt�r (daha k�sa bir delay)
        shootingDelay = 0.5f;
    }

    private new void Update()
    {
        if (!isAtTargetPosition)
        {
            // Boss hedef pozisyona ula�mad�ysa, pozisyona do�ru hareket etsin
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Hedef pozisyona ula�t���nda
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isAtTargetPosition = true;
                rb2d.velocity = Vector2.zero; // Hareket etmeyi durdur
            }
        }
        else
        {
            // Hedef pozisyona ula�t���nda s�rekli d�nmeye ba�la
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            // Ate� etmeye ba�la
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(ShootWithDelay(shootingDelay));
            }
        }
    }

    private new void FixedUpdate()
    {
        // Boss hareket etmeyecek, sadece d�nmeye ba�layacak
    }

    private void HandleBossDeath()
    {
        Debug.Log("Boss has died.");
        // Boss �ld���nde yap�lacak i�lemler (oyunu bitir ve di�er sahneye ge�)
        SceneManager.LoadScene("EndScreen"); // "EndScene" yerine ge�ilmesi gereken sahnenin ad�n� yaz�n
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerHealth = collision.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.GetHit(1, gameObject); // Oyuncunun can�n� azalt
            bossHealth.GetHit(1, collision.gameObject); // Boss'un can�n� azalt
        }
    }
}

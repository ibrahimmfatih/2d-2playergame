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
        targetPosition = new Vector3(0, 0, 0); // Sahnenin ortasý



        bossHealth = GetComponent<Health>();
        if (bossHealth != null)
        {
            bossHealth.OnDeath.AddListener(HandleBossDeath);
        }

        // Boss'un canýný 100 yap
        bossHealth.InitializeHealth(100);

        // Ateþ etme hýzýný arttýr (daha kýsa bir delay)
        shootingDelay = 0.5f;
    }

    private new void Update()
    {
        if (!isAtTargetPosition)
        {
            // Boss hedef pozisyona ulaþmadýysa, pozisyona doðru hareket etsin
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Hedef pozisyona ulaþtýðýnda
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isAtTargetPosition = true;
                rb2d.velocity = Vector2.zero; // Hareket etmeyi durdur
            }
        }
        else
        {
            // Hedef pozisyona ulaþtýðýnda sürekli dönmeye baþla
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            // Ateþ etmeye baþla
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(ShootWithDelay(shootingDelay));
            }
        }
    }

    private new void FixedUpdate()
    {
        // Boss hareket etmeyecek, sadece dönmeye baþlayacak
    }

    private void HandleBossDeath()
    {
        Debug.Log("Boss has died.");
        // Boss öldüðünde yapýlacak iþlemler (oyunu bitir ve diðer sahneye geç)
        SceneManager.LoadScene("EndScreen"); // "EndScene" yerine geçilmesi gereken sahnenin adýný yazýn
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerHealth = collision.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.GetHit(1, gameObject); // Oyuncunun canýný azalt
            bossHealth.GetHit(1, collision.gameObject); // Boss'un canýný azalt
        }
    }
}

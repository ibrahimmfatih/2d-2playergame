using System.Collections;
using UnityEngine;
using PA.HealthSystem;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int initialHealthValue = 3;

    public GameObject projectile;
    public float shootingDelay;

    public bool isShooting = false;

    public float speed = 2;
    public float speedVariation = 0.3f;
    public Rigidbody2D rb2d;
    bool firstShoot = true;

    public EnemySpawner enemySpawner;

    [SerializeField]
    private Health health;

    public void Awake()
    {
        health = GetComponent<Health>();
        rb2d = GetComponent<Rigidbody2D>();
        speed += UnityEngine.Random.Range(0, speedVariation);
    }

    public void Start()
    {
        health.InitializeHealth(initialHealthValue);
    }

    public void Update()
    {
        GameObject nearestPlayer = FindNearestPlayer();
        if (nearestPlayer != null)
        {
            var playerComponent = nearestPlayer.GetComponent<Player>();
            if (playerComponent != null && playerComponent.isAlive)
            {
                Vector3 desiredDirection = nearestPlayer.transform.position - transform.position;
                float desiredAngle = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.AngleAxis(desiredAngle, Vector3.forward);

                if (isShooting == false)
                {
                    isShooting = true;
                    StartCoroutine(ShootWithDelay(shootingDelay));
                }
            }
            else
            {
                var player2Component = nearestPlayer.GetComponent<Player2>();
                if (player2Component != null && player2Component.isAlive)
                {
                    Vector3 desiredDirection = nearestPlayer.transform.position - transform.position;
                    float desiredAngle = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg - 90;
                    transform.rotation = Quaternion.AngleAxis(desiredAngle, Vector3.forward);

                    if (isShooting == false)
                    {
                        isShooting = true;
                        StartCoroutine(ShootWithDelay(shootingDelay));
                    }
                }
            }
        }
    }

    public void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + Vector2.down * speed * Time.deltaTime);
    }

    public IEnumerator ShootWithDelay(float shootingDelay)
    {
        if (firstShoot)
        {
            firstShoot = false;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0, 0.5f));
        }
        yield return new WaitForSeconds(shootingDelay);
        GameObject p = Instantiate(projectile, transform.position, transform.rotation);
        isShooting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() || collision.GetComponent<Player2>())
        {
            IHittable hittable = collision.GetComponent<IHittable>();
            if (hittable != null)
            {
                hittable.GetHit(1, gameObject);
                Death();
            }
        }
    }

    public void EnemyKilledOutsideBounds()
    {
        enemySpawner.EnemyKilled(this, false);
        Destroy(gameObject);
    }

    public void Death()
    {
        enemySpawner.EnemyKilled(this, true);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private GameObject FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearestPlayer = null;
        float minDistance = float.MaxValue;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPlayer = player;
            }
        }

        return nearestPlayer;
    }
}

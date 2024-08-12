using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PA.WeaponSystem;
using PA.HealthSystem;

public class Player : MonoBehaviour, IHittable
{
    public float speed = 2;
    public Transform playerShip;
    public ScreenBounds screenBounds;
    public int initialHealthValue = 3;

    [SerializeField]
    private Transform liveImagesUIParent;
    List<Image> lives = new List<Image>();

    Rigidbody2D rb2d;
    Vector2 movementVector = Vector2.zero;

    public bool isAlive = true;

    public InGameMenu loseScreen;
    public Button menuButton;
    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private Health health;

    private Vector3 bossPosition = Vector3.zero; // Boss'un pozisyonu 0, 0, 0
    private bool isLevel2 = false;

    private void OnEnable()
    {
        if (health == null)
        {
            health = GetComponent<Health>();
            health.InitializeHealth(initialHealthValue);
        }
        health.OnDeath.AddListener(Death);
        health.OnDeath.AddListener(UpdateUI);
        health.OnHit.AddListener(UpdateUI);
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnDeath.RemoveListener(Death);
            health.OnDeath.RemoveListener(UpdateUI);
            health.OnHit.RemoveListener(UpdateUI);
        }
    }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        foreach (Transform item in liveImagesUIParent)
        {
            lives.Add(item.GetComponent<Image>());
        }

        // Sadece Level 2'deyken Boss'a kilitlenmeyi etkinleþtirin
        isLevel2 = SceneManager.GetActiveScene().name == "Level2"; // Level 2'nin sahne adýný buraya yazýn
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input.Normalize();
        movementVector = speed * input;

        if (!isAlive)
            return;

        if (isLevel2)
        {
            // Sadece Level 2'de Boss'a doðru dönme iþlemi (0, 0, 0 noktasý)
            Vector2 direction = (bossPosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb2d.rotation = angle - 90f;  // Sprite'ýn yönüne göre açýyý ayarlayýn (burada -90 derece düzeltme var)
        }

        if (Input.GetKey(KeyCode.Space))
        {
            weapon.PerformAttack();
        }
        if (Input.GetKey(KeyCode.Q))
        {
            weapon.SwapWeapon();
        }
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = rb2d.position + movementVector * Time.fixedDeltaTime;
        if (!screenBounds.AmIOutOfBounds(newPosition))
        {
            rb2d.MovePosition(newPosition);
        }
    }

    public void ReduceLives()
    {
        health.GetHit(1, gameObject);
    }

    private void Death()
    {
        isAlive = false;

        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        StartCoroutine(DestroyCoroutine());
    }

    private void UpdateUI()
    {
        for (int i = 0; i < lives.Count; i++)
        {
            if (i >= health.CurrentHealth)
            {
                lives[i].color = Color.black;
            }
            else
            {
                lives[i].color = Color.white;
            }
        }
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        loseScreen.Toggle();
        menuButton.interactable = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IHittable hittable = collision.gameObject.GetComponent<IHittable>();
            if (hittable != null)
            {
                hittable.GetHit(1, gameObject);
            }
        }
    }

    public void GetHit(int damageValue, GameObject sender)
    {
        health.GetHit(damageValue, sender);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    public int health = 3;
    public float speed = 2;
    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb2d.velocity = Vector3.down * speed;
    }

    private void Update()
    {
        Player nearestPlayer = FindNearestPlayer();
        if (nearestPlayer != null)
        {
            Vector3 desiredDirection = nearestPlayer.transform.position - transform.position;
            float desiredAngle = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(desiredAngle, Vector3.forward);
            rb2d.velocity = desiredDirection.normalized * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Projectile>())
        {
            Destroy(collision.gameObject);
        }

        if (collision.GetComponent<Player>())
        {
            Destroy(gameObject);
            return;
        }

        Debug.Log(collision.name);
        health--;
        if (health <= 0)
            Destroy(gameObject);
    }

    private Player FindNearestPlayer()
    {
        Player[] players = FindObjectsOfType<Player>();
        Player nearestPlayer = null;
        float minDistance = float.MaxValue;

        foreach (Player player in players)
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

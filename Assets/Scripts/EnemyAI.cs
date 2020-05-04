using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // variables
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float projSpeed = 3f;

    // cached references
    private GameObject player;
    [SerializeField] private GameObject rangeCollider;
    [SerializeField] private GameObject projectile;
    private CircleCollider2D circleCollider;

    //cached variables
    private float minRange;
    private float storedSpeed;
    private float distance;
    private bool isFiring = false;
    private float timer = 0f;

    private void Start()
    {
        circleCollider = rangeCollider.GetComponent<CircleCollider2D>();
        minRange = circleCollider.radius;
        storedSpeed = moveSpeed;

        player = GameObject.Find("Player");
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        bool tooClose = distance < minRange;

        SmoothMovement();
        MoveToPlayer(tooClose);
        ShootingHandler();
    }

    private void MoveToPlayer(bool tooClose)
    {
        if(tooClose == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -moveSpeed * Time.deltaTime);
        }
    }

    private void SmoothMovement()
    {
        if (distance > minRange && distance < minRange + 0.1f)
        {
            moveSpeed = 0.1f;
        }
        else
        {
            moveSpeed = storedSpeed;
        }
    }

    private void ShootingHandler()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= 3f)
        {
            isFiring = true;
        }

        if(isFiring && timer >= 1f)
        {
            ProjectileSpawn();
            timer = 0f;
        }

        if (Vector3.Distance(transform.position, player.transform.position) > 3f)
        {
            isFiring = false;
        }

        timer += Time.deltaTime;
    }

    private void ProjectileSpawn()
    {
        Vector2 direction = (Vector2)((player.transform.position - transform.position));
        direction.Normalize();
        // Creates the bullet locally
        GameObject proj = (GameObject)Instantiate(
                                projectile,
                                transform.position + (Vector3)(direction * 0.5f),
                                Quaternion.identity);

        // Adds velocity to the bullet
        proj.GetComponent<Rigidbody2D>().velocity = direction * projSpeed;
        proj.transform.parent = transform;
        Destroy(proj, 1f);
    }

    private void DamageTaken(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            GetComponentInParent<Spawners>().EnemyIsKilled();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile")
        {
            float damage = collision.gameObject.GetComponent<Projectile>().GetDamage();
            DamageTaken(damage);
            Destroy(collision.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // variables
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float projSpeed = 5f;
    [SerializeField] private float timeBetweenAttacks = 0.05f;

    // cached references
    [SerializeField] private GameObject damageCollider;
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject projectile;
    private Rigidbody2D rigidBody;
    private Animator animator;
    //private SpriteRenderer objRenderer;

    // cached variables
    private float moveH, moveV;
    private float colorTimer = 0f;
    private float attTimer = 0f;
    private float powerUpTimer = 0f;
    private float powerUpDuration = 0f;
    private bool isPoweredUp = false;
    // bool powerUp

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //objRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveH = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveV = Input.GetAxisRaw("Vertical") * moveSpeed;

        SpriteHandler();
        if (attTimer >= timeBetweenAttacks)
        {
            HandleShooting();
            attTimer = 0f;
        }
        else
        {
            attTimer += Time.deltaTime;
        }

        //objRenderer.material.color =  Color.Lerp(Color.blue, Color.white, colorTimer);

        if(colorTimer <= 5f)
        {
            colorTimer += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(moveH, moveV);
    }

    private void SpriteHandler()
    {
        if(moveH == +1 * moveSpeed && moveV == -1 * moveSpeed)
        {
            gameObject.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            animator.SetBool("isFacingUp", false);
        }
        else
        if(moveH == -1 * moveSpeed && moveV == -1 * moveSpeed)
        {
            gameObject.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            animator.SetBool("isFacingUp", false);
        }
        else
        if(moveH == +1 * moveSpeed && moveV == +1 * moveSpeed)
        {
            gameObject.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            animator.SetBool("isFacingUp", true);
        }
        else
        if (moveH == -1 * moveSpeed && moveV == +1 * moveSpeed)
        {
            gameObject.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            animator.SetBool("isFacingUp", true);
        }
        else
        if (moveH == 0 && moveV == +1 * moveSpeed)
        {
            gameObject.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            animator.SetBool("isFacingUp", true);
        }
        else
        if (moveH == 0 && moveV == -1 * moveSpeed)
        {
            gameObject.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            animator.SetBool("isFacingUp", false);
        }
        else
        if (moveH == +1 * moveSpeed && moveV == 0)
        {
            gameObject.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            animator.SetBool("isFacingUp", false);
        }
        else
        if (moveH == -1 * moveSpeed && moveV == 0)
        {
            gameObject.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            animator.SetBool("isFacingUp", false);
        }
        else
        if (moveH == 0 && moveV == 0)
        {
            gameObject.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            animator.SetBool("isFacingUp", false);
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 direction = (Vector2)((worldMousePos - transform.position));
            direction.Normalize();

            // Creates the bullet locally
            GameObject proj = (GameObject)Instantiate(
                                    projectile,
                                    transform.position + (Vector3)(direction * 0.5f),
                                    Quaternion.identity);

            // Adds velocity to the bullet
            proj.GetComponent<Rigidbody2D>().velocity = direction * projSpeed;

            Destroy(proj, 1f);
        }

        PowerUpTimer();
    }

    private void DamageTaken(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            var levelController = GameObject.Find("LevelController");
            levelController.GetComponent<LevelController>().LoseScene();
        }
    }

    public float GetPlayerHealth()
    {
        return health;
    }

    private void ReplenishHealth()
    {
        int extra = Random.Range(30, 50);

        if(health + extra >= 100)
        {
            health = 100;
        }
        else
        {
            health += extra;
        }
    }

    private void PowerUp()
    {
        if(isPoweredUp)
            timeBetweenAttacks = timeBetweenAttacks / 20;
    }

    private void PowerUpTimer()
    {
        if(isPoweredUp)
        {
            if(powerUpTimer >= powerUpDuration)
            {
                powerUpTimer = 0;
                isPoweredUp = false;
                timeBetweenAttacks *= 20;
            }
            else
            {
                powerUpTimer += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyProjectile")
        {
            float damage = collision.gameObject.GetComponent<Projectile>().GetDamage();
            DamageTaken(damage);
            Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "Spawner")
        {
            collision.gameObject.GetComponent<Spawners>().PlayerIsInRange(true);
        }

        if(collision.gameObject.tag == "YellowNexus")
        {
            collision.gameObject.GetComponent<Nexus>().Deactivate();
            ReplenishHealth();
        }

        if (collision.gameObject.tag == "RedNexus")
        {
            collision.gameObject.GetComponent<Nexus>().Deactivate();
            powerUpDuration = collision.gameObject.GetComponent<Nexus>().GetTimer();
            PowerUp();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spawner")
        {
            collision.gameObject.GetComponent<Spawners>().PlayerIsInRange(false);
        }
    }
}

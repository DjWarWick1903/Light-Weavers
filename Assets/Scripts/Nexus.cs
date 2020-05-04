using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : MonoBehaviour
{
    private float timer = 0f;
    [SerializeField] private float deactivateTimer = 60f;
    private bool isOn = true;
    private CircleCollider2D spriteCollider;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteCollider = GetComponent<CircleCollider2D>();
        animator.SetBool("isActive", true);
    }

    void Update()
    {
        if(!isOn)
        {
            spriteCollider.enabled = false;
            animator.SetBool("isActive",false);
        }

        if(!isOn)
        {
            if (timer >= deactivateTimer)
            {
                timer = 0;
                animator.SetBool("isActive", true);
                spriteCollider.enabled = true;
                isOn = true;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    public void Deactivate()
    {
        isOn = false;
    }

    public float GetTimer()
    {
        return deactivateTimer;
    }
}

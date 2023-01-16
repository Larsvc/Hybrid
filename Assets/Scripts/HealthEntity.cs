using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthEntity : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 1000;
    protected float health;
    //[SerializeField] protected Animator hitmarker;
    protected Animator animator;

    public bool IsDead;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (health <= 0 && !IsDead)
        {
            Die();
            IsDead = true;
        }
    }

    public virtual void TakeHit(float damage, Animator hitmarker)
    {
        hitmarker.SetTrigger("hit");
        health -= damage;
        animator.SetTrigger("hit");
        hitmarker.GetComponent<AudioSource>().Play();
        hitmarker.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.3f);
    }

    protected abstract void Die();
}

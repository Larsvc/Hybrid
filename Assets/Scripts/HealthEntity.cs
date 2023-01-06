using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthEntity : MonoBehaviour
{
    [SerializeField] protected float health = 1000;
    [SerializeField] protected Animator hitmarker;
    protected Animator animator;

    public bool IsDead
    {
        get { return health > 0; }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void TakeHit(float damage)
    {
        hitmarker.SetTrigger("hit");
        health -= damage;
        animator.SetTrigger("hit");
        hitmarker.GetComponent<AudioSource>().Play();
        hitmarker.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.3f);
    }

    protected abstract void Die();
}

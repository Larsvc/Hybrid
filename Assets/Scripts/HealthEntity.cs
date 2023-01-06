using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthEntity : MonoBehaviour
{
    [SerializeField]private float health = 1000;
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
    }

    protected abstract void Die();
}

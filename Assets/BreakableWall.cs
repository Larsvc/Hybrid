using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : HealthEntity
{

    private Rigidbody[] stones;
    private bool destroyed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        stones = GetComponentsInChildren<Rigidbody>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (destroyed)
            foreach (Rigidbody stone in stones)
                stone.transform.localScale *= 0.998f;
    }

    public override void TakeHit(float damage, Animator hitmarker)
    {
        if (!destroyed)
            base.TakeHit(damage, hitmarker);
    }

    protected override void Die()
    {
        foreach (Rigidbody stone in stones)
            stone.isKinematic = false;

        destroyed = true;
        Destroy(gameObject, 4f);
    }
}

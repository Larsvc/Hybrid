using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityModule : Module
{
    protected bool canActivate = true;
    [SerializeField] abstract protected float Cooldown
    {
        get;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void Activate()
    {
        if (!canActivate)
            return;

        transform.GetComponentInChildren<ParticleSystem>().Play();

        doAbility();

        canActivate = false;

        StartCoroutine(WaitForCooldown());
    }

    public abstract void doAbility();

    IEnumerator WaitForCooldown()
    {
        yield return new WaitForSeconds(Cooldown);
        canActivate = true;
    }
}

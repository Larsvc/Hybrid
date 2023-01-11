using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldModule : Module
{

    protected override void Die()
    {
        Destroy(transform.GetChild(0).gameObject);
        base.Die();
    }
}

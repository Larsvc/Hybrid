using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldModule : AbilityModule
{

    bool active = false;
    public float shieldTime = 3.5f;

    protected override float Cooldown
    {
        get { return 10f; }
    }

    protected override void doAbility()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        active = true;
        StartCoroutine(WaitForShieldEnd());
    }

    IEnumerator WaitForShieldEnd()
    {
        yield return new WaitForSeconds(shieldTime);
        transform.GetChild(0).gameObject.SetActive(false);
        active = false;
    }

    protected override void Die()
    {
        base.Die();
    }

    public override void TakeHit(float damage)
    {
        hitmarker.SetTrigger("hit");
        if (!active)
        {
            health -= damage;
            animator.SetTrigger("hit");
            hitmarker.GetComponent<AudioSource>().Play();
            hitmarker.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.3f);
        }
        else
        {
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("hit");
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.3f);
        }
    }
}

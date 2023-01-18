using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamModule : Module
{
    [SerializeField] private float damage = 100f;

    private float maxDamageVelocity = 40f;

    private bool canRam = true;

    protected override void Start()
    {
        base.Start();
    }

    private float actualDamage
    {
        get { return damage * multiplier; }
    }

    private float force
    {
        get { return 3000f * multiplier; }
    }

    private float multiplier
    {
        get { return player.GetComponent<Rigidbody>().velocity.magnitude / maxDamageVelocity; }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform otherBase = other.transform.root;
        if (otherBase.tag == "Player" && otherBase != transform.root && player.GetComponent<Rigidbody>().velocity.magnitude > 4  && canRam)
        {
            StartCoroutine(HitEnemyEffect());
            otherBase.GetComponent<HealthEntity>().TakeHit(actualDamage, player.hitmarker);
            otherBase.GetComponent<Rigidbody>().AddForce(player.transform.forward * force * otherBase.GetComponent<Rigidbody>().mass / 1.7f + player.transform.up * otherBase.GetComponent<Rigidbody>().mass / 1.7f, ForceMode.Force);
            //player.GetComponent<Rigidbody>().velocity *= 0.8f;
            player.cam.GetComponent<CameraShake>().startShaking(0.2f, 1f * multiplier, 80f);
        }
    }

    IEnumerator HitEnemyEffect()
    {
        canRam = false;
        GetComponent<AudioSource>().Play();
        Time.timeScale = 0.01f;
        yield return new WaitForSeconds(.002f);
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1f);
        canRam = true;
    }

}

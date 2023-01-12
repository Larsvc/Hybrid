using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpAbility : AbilityModule
{
    GameObject player;
    float basePlayerSpeed;
    //public float speedUpModifier = 2;
    public float forceModifier = .6f;
    public float spedUpTime = 4;

    bool activated;

    protected override float Cooldown
    {
        get { return 10f; }
    }

    protected override void doAbility()
    {
        //player.GetComponent<PlayerCar>().moveSpeed = basePlayerSpeed * speedUpModifier;
        activated = true;
        StartCoroutine(WaitForEffectEnd());
    }

    IEnumerator WaitForEffectEnd()
    {
        yield return new WaitForSeconds(spedUpTime);
        //player.GetComponent<PlayerCar>().moveSpeed = basePlayerSpeed;
        activated = false;
        GetComponentInChildren<AudioSource>().Stop();
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = transform.root.gameObject;
        basePlayerSpeed = player.GetComponent<PlayerCar>().moveSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if (activated)
            player.GetComponent<Rigidbody>().AddForce(player.transform.forward * forceModifier, ForceMode.Impulse);
    }
}

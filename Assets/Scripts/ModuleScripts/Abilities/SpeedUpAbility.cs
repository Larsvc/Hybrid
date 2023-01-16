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

    private Rigidbody playerRB;

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
        GetComponentInChildren<ParticleSystem>().Stop();
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = transform.root.gameObject;
        basePlayerSpeed = player.GetComponent<PlayerCar>().moveSpeed;

        playerRB = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        if (activated)
            playerRB.AddForce(player.transform.forward * forceModifier * playerRB.mass, ForceMode.Impulse);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpAbility : AbilityModule
{
    GameObject player;
    float basePlayerSpeed;
    public float speedUpModifier = 2;
    public float spedUpTime = 4;

    protected override float Cooldown
    {
        get { return 10f; }
    }

    public override void doAbility()
    {
        player.GetComponent<PlayerCar>().moveSpeed = basePlayerSpeed * speedUpModifier;
        StartCoroutine(WaitForEffectEnd());
    }

    IEnumerator WaitForEffectEnd()
    {
        yield return new WaitForSeconds(spedUpTime);
        player.GetComponent<PlayerCar>().moveSpeed = basePlayerSpeed;
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        player = transform.root.gameObject;
        basePlayerSpeed = player.GetComponent<PlayerCar>().moveSpeed;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : HealthEntity
{
    private bool pickedUp;
    [HideInInspector] public PlayerCar carriedBy;

    private float slowPercentage = 0.2f;

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

    private void Pickup(Transform player, bool pickup)
    {
        pickedUp = pickup;
        transform.SetParent(player);
        transform.localPosition = new Vector3(0, 0, -1.5f);
        GetComponent<Collider>().isTrigger = false;
        /*GetComponentInChildren<ParticleSystem>().Clear();
        GetComponentInChildren<ParticleSystem>().Stop();*/
        /*GetComponentInChildren<Light>().intensity = 0;*/
        transform.GetChild(0).gameObject.SetActive(!pickup);

        if (pickup)
        {
            carriedBy.SetSpeed(carriedBy.baseSpeed * (1f - slowPercentage));
            GetComponentInChildren<AudioSource>().Play();
            GetComponentInChildren<ParticleSystem>().Play();
        }
        else
        {
            if (carriedBy)
                carriedBy.SetSpeed(carriedBy.baseSpeed);

            carriedBy = null;
        }
        
        GetComponentInChildren<AudioSource>().Play();
        //GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !pickedUp)
        {
            carriedBy = other.GetComponent<PlayerCar>();
            Pickup(other.transform, true);
        }
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }
}

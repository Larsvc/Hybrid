using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : HealthEntity
{
    private bool pickedUp;
    [HideInInspector] public PlayerCar carriedBy;

    private float slowPercentage = 0.7f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (carriedBy && !GetComponentInChildren<ParticleSystem>().isEmitting && carriedBy.GetComponent<Rigidbody>().velocity.magnitude > 1)
        {
            GetComponent<AudioSource>().Play();
            GetComponentInChildren<ParticleSystem>().Play();
        }
        else if (carriedBy && carriedBy.GetComponent<Rigidbody>().velocity.magnitude < 1)
        {
            GetComponent<AudioSource>().Stop();
            GetComponentInChildren<ParticleSystem>().Stop();
        }

        if (carriedBy && carriedBy.IsDead)
            Pickup(null, false);

        if (transform.position.y < -1f)
            Die();
    }

    public void Pickup(Transform player, bool pickup)
    {
        pickedUp = pickup;
        transform.SetParent(player);
        CarControllerPlaceholder carController = carriedBy.GetComponent<CarControllerPlaceholder>();

        if (pickup)
            transform.localPosition = new Vector3(0, 1, -5f);
        else
        {
            transform.rotation = Quaternion.identity;
        }

        //GetComponent<Collider>().isTrigger = !pickup;
        /*GetComponentInChildren<ParticleSystem>().Clear();
        GetComponentInChildren<ParticleSystem>().Stop();*/
        /*GetComponentInChildren<Light>().intensity = 0;*/
        transform.GetChild(0).gameObject.SetActive(!pickup);

        if (pickup)
        {
            //carriedBy.SetSpeed(carriedBy.baseSpeed * (1f - slowPercentage));
            GetComponentInChildren<AudioSource>().Play();
            carController.motorTorque = carController.baseMotorTorque * (1f - slowPercentage);
        }
        else
        {
            if (carriedBy)
                carController.motorTorque = carController.baseMotorTorque;

            carriedBy = null;
            GetComponentInChildren<ParticleSystem>().Stop();
        }
        
        GetComponentInChildren<AudioSource>().Play();
        //GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Player" && !pickedUp)
        {
            carriedBy = other.transform.root.GetComponent<PlayerCar>();
            Pickup(other.transform.root, true);
        }
    }

    protected override void Die()
    {
        Destroy(gameObject);
        if (carriedBy)
            carriedBy.GetComponent<CarControllerPlaceholder>().motorTorque = carriedBy.GetComponent<CarControllerPlaceholder>().baseMotorTorque;
    }
}

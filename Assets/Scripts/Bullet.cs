using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 hitPoint;

    private float prevDistance = 100000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hitPoint != Vector3.zero && prevDistance < Vector3.Distance(transform.position, hitPoint))
            Destroy(gameObject);

        prevDistance = Vector3.Distance(transform.position, hitPoint);
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<PlayerCar>().TakeHit();
            Destroy(gameObject);
        }
    }*/
}

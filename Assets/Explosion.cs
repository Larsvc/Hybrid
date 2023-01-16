using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    float damage;
    private Animator hitmarkerFromPlayer;
    List<Collider> collidedObjects;

    // Start is called before the first frame update
    void Start()
    {
        collidedObjects = new List<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<HealthEntity>() && !collidedObjects.Contains(col))
        {
            col.GetComponent<HealthEntity>().TakeHit(damage, hitmarkerFromPlayer);
            collidedObjects.Add(col);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<HealthEntity>() && !collidedObjects.Contains(collision.collider))
        {
            collision.collider.GetComponent<HealthEntity>().TakeHit(damage, hitmarkerFromPlayer);
            collidedObjects.Add(collision.collider);
        }
    }
}

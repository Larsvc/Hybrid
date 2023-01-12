using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private LayerMask mask;

    [SerializeField] private float damage = 60f;
    bool dead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!((mask.value & (1 << collision.gameObject.layer)) > 0) && !dead)
        {
            dead = true;

            bool canDamage = FirstParent(collision.transform);
            if (canDamage)
            {
                FirstParent(collision.transform).TakeHit(damage);
            }

            GameObject explosion = Instantiate(PrefabManager.instance.cannonballExplosion, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!((mask.value & (1 << col.gameObject.layer)) > 0) && !dead)
        {
            dead = true;

            bool canDamage = FirstParent(col.transform);
            if (canDamage)
            {
                FirstParent(col.transform).TakeHit(damage);
            }

            GameObject explosion = Instantiate(PrefabManager.instance.cannonballExplosion, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
            Destroy(gameObject);
        }
    }

    private HealthEntity FirstParent(Transform t)
    {
        if (t.GetComponent<HealthEntity>() && t != transform.root && t != transform)
            return t.GetComponent<HealthEntity>();
        else if (t.parent != null)
            return FirstParent(t.parent);
        else
            return null;
    }
}

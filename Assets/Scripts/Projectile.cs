using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Projectile : MonoBehaviour
{
    public LayerMask mask;

    [SerializeField] private float damage = 60f;
    [SerializeField] private float radius = 5f;
    private Animator hitmarkerFromPlayer;
    bool dead;

    public void SetHitMarker(Animator h)
    {
        hitmarkerFromPlayer = h;
    }

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
        if (((mask.value & (1 << collision.gameObject.layer)) > 0) && !dead && !collision.collider.gameObject.GetComponent<Projectile>())
        {
            dead = true;

            AreaDamage();

            GameObject explosion = Instantiate(PrefabManager.instance.cannonballExplosion, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (((mask.value & (1 << col.gameObject.layer)) > 0) && !dead && !col.GetComponent<Projectile>())
        {
            dead = true;

            AreaDamage();

            GameObject explosion = Instantiate(PrefabManager.instance.cannonballExplosion, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
            Destroy(gameObject);
        }
    }

    private void AreaDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        List<HealthEntity> shouldBeDamaged = new List<HealthEntity>();
        foreach (Collider c in colliders)
            {
                HealthEntity fp = FirstParent(c.transform);
                if (fp && !shouldBeDamaged.Contains(fp))
                {
                    float distance = Vector3.Distance(transform.position, fp.transform.position);
                    fp.TakeHit(damage / Mathf.Sqrt(distance), hitmarkerFromPlayer);
                    shouldBeDamaged.Add(fp);
                }

                /*bool canDamage = FirstParent(collision.collider.transform);
                if (canDamage)
                {
                    FirstParent(collision.collider.transform).TakeHit(damage, hitmarkerFromPlayer);
                }*/
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

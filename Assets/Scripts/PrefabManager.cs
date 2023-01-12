using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager instance;
    [Header("Particles")]
    public GameObject hitEffectParticles;
    public GameObject moduleExplosion;
    public GameObject cannonballExplosion;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

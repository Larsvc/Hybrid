using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoint : MonoBehaviour
{
    public int playerNumber = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Cargo>() != null)
        {
            //Puntje optellen met 1
            //ScoreManager.AddScore(playerNumber);
            Destroy(other.gameObject);
        }
    }
}
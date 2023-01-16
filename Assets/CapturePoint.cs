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
        if (other.GetComponent<Cargo>() != null && other.GetComponent<Cargo>().carriedBy.playerNumber == playerNumber)
        {
            //Puntje optellen met 1
            //ScoreManager.AddScore(playerNumber);
            //Destroy(other.gameObject);
            GameManager.instance.AddPoint(playerNumber);
            Debug.Log("yoo ik score voor " + playerNumber);
            other.GetComponent<Cargo>().Pickup(null, false);
            DoVisuals(other);
        }
    }

    private void DoVisuals(Collider other)
    {
        other.GetComponentInChildren<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();
        Destroy(other.GetComponent<Collider>());
        Destroy(other.GetComponentInChildren<ParticleSystem>());
        other.transform.position = transform.GetChild(0).position;
        other.transform.SetParent(transform.GetChild(0));
        GetComponent<Animator>().SetTrigger("capture");
        Destroy(other.gameObject, 3f);
    }
}

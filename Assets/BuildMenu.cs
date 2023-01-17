using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    int currentPlayerIndex;

    [SerializeField] private GameObject[] screens;

    [SerializeField] private AudioClip clickSound;

    [SerializeField] private PlayerCar[] players;

    private bool wasUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Submit") != 0 && !wasUp && currentPlayerIndex < screens.Length - 1)
        {
            players[currentPlayerIndex].pickingModules = false;
            wasUp = true;
            GetComponent<AudioSource>().PlayOneShot(clickSound);
            screens[currentPlayerIndex].SetActive(false);
            screens[currentPlayerIndex + 1].SetActive(true);
            GameManager.instance.SelectModules(currentPlayerIndex);
            currentPlayerIndex++;

            if (currentPlayerIndex < players.Length)
                players[currentPlayerIndex].pickingModules = true;

            if (currentPlayerIndex == screens.Length - 1)
                Application.LoadLevel(1);
        }
        else if (Input.GetAxisRaw("Submit") == 0)
            wasUp = false;
    }
}

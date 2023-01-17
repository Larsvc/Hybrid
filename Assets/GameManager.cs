using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int[] scores = new int[2];
    private TextMeshProUGUI[] scoreTexts = new TextMeshProUGUI[2];
    // Start is called before the first frame update

    private GameObject currentCargo;
    [SerializeField] private Transform cargoSpawnPoint;
    [SerializeField] private GameObject cargoPrefab;
    [SerializeField] private TextMeshProUGUI captureText;
    [SerializeField] private int maxScore = 3;
    [SerializeField] private GameObject p1WinScreen;
    [SerializeField] private GameObject p2WinScreen;

    public static ModuleInfo[] playerSelectedModules = new ModuleInfo[2];

    public class ModuleInfo
    {
        public string[] modules = new string[4];

        public ModuleInfo(string[] mods)
        {
            modules = mods;
        }
    }

    public void SelectModules(int player)
    {
        playerSelectedModules[player - 1] = new ModuleInfo(ModuleController.instance.modules);
    }

    void Start()
    {
        instance = this;

        if (GameObject.Find("Cargo"))
        currentCargo = GameObject.Find("Cargo");

        for (int i = 0; i < scoreTexts.Length; i++)
            scoreTexts[i] = GameObject.Find("ScoreText" + (i + 1)).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCargo();
        UpdateText();
    }

    private void UpdateText()
    {
        scoreTexts[0].text = scores[0].ToString();
        scoreTexts[1].text = scores[1].ToString();
    }

    private void CheckCargo()
    {
        if (currentCargo == null)
            currentCargo = Instantiate(cargoPrefab, cargoSpawnPoint.position, Quaternion.identity);
    }

    public void AddPoint(int player)
    {
        scores[player-1]++;
        captureText.GetComponent<Animator>().SetTrigger("capture");
        captureText.GetComponent<AudioSource>().Play();
        captureText.text = "Player " + player + " captured the cargo!";

        // Checken of iemand gewonnen heeft
        if (scores[0] >= maxScore)
            p1WinScreen.SetActive(true);
        if (scores[1] >= maxScore)
            p2WinScreen.SetActive(true);
    }
}

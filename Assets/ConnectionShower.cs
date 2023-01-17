using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionShower : MonoBehaviour
{
    [SerializeField] private Sprite[] statusSprites;
    [SerializeField] private Image statusShower;

    [SerializeField] private TextMeshProUGUI moduleText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moduleText.text = "Modules connected: " + ModuleController.instance.modules.Where(m => m != "").ToArray().Length;
    }

    public void ShowConnection(int connected)
    {
        statusShower.sprite = statusSprites[connected];
    }
}

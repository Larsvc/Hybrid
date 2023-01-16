using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using UnityEngine;

public class ModuleController : MonoBehaviour
{
    public SerialController serialController;

    public float wait = 0.1f;

    public string[] modules = new string[4];
    public static ModuleController instance;

    private void Start()
    {
        instance = this;
    }

    // Executed each frame
    void Update()
    {

        wait -= Time.deltaTime;
        if (wait < 0)
        {
            serialController.SendSerialMessage("A");
            wait = 0.1f;
        }

        //---------------------------------------------------------------------
        // Receive data
        //---------------------------------------------------------------------

        string message = serialController.ReadSerialMessage();

        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
        {
            Debug.Log(message);
            modules = message.Split(' ').Select(x => IdToName(int.Parse(x))).ToArray();
        }
    }

    private string IdToName(int id)
    {
        switch (id)
        {
            case 32:
                return "Minigun";
            case 33:
                return "Shield";
            case 34:
                return "Booster";
            case 35:
                return "Launcher";
            case 36:
                return "Ram";
            default:
                return "";
        }
    }
}

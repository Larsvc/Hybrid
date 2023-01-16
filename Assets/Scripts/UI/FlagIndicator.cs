using UnityEngine;
using UnityEngine.UI;

public class FlagIndicator : MonoBehaviour
{
    public Transform flag;
    public Image indicator;
    public float cornerThreshold = 0.1f;
    public bool rightScreenIndicator = true;
    public Camera cam;

    void Update()
    {
        Vector3 flagScreenPos = cam.WorldToScreenPoint(flag.position);
        int middle = Screen.width / 2;

        if (rightScreenIndicator)
        {
            if (flagScreenPos.x < middle + middle * cornerThreshold && flagScreenPos.x > middle)
            {
                if (flagScreenPos.y < Screen.height * cornerThreshold)
                {
                    indicator.transform.position = new Vector3(middle, 0, 0);
                }
                else if (flagScreenPos.y > Screen.height * (1 - cornerThreshold))
                {
                    indicator.transform.position = new Vector3(middle, Screen.height, 0);
                }
                else
                {
                    indicator.transform.position = flagScreenPos;
                }
            }
            else
            {
                indicator.transform.position = new Vector3(-1000, -1000, 0);
            }
        }
        else
        {
            if (flagScreenPos.x > middle - middle * cornerThreshold && flagScreenPos.x < middle)
            {
                if (flagScreenPos.y < Screen.height * cornerThreshold)
                {
                    indicator.transform.position = new Vector3(middle, 0, 0);
                }
                else if (flagScreenPos.y > Screen.height * (1 - cornerThreshold))
                {
                    indicator.transform.position = new Vector3(middle, Screen.height, 0);
                }
                else
                {
                    indicator.transform.position = flagScreenPos;
                }
            }
            else
            {
                indicator.transform.position = new Vector3(-1000, -1000, 0);
            }
        }
    }
}
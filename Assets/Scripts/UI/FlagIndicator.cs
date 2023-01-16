using UnityEngine;
using UnityEngine.UI;

public class FlagIndicator : MonoBehaviour
{
    public Transform flag;
    public Image indicator;
    public float cornerThreshold = 0.1f;

    void Update()
    {
        Vector3 flagScreenPos = Camera.main.WorldToScreenPoint(flag.position);
        indicator.transform.position = flagScreenPos;

        if (flagScreenPos.x < Screen.width * cornerThreshold || flagScreenPos.x > Screen.width * (1 - cornerThreshold) ||
            flagScreenPos.y < Screen.height * cornerThreshold || flagScreenPos.y > Screen.height * (1 - cornerThreshold))
        {
            indicator.color = Color.red;
        }
        else
        {
            indicator.color = Color.green;
        }
    }
}
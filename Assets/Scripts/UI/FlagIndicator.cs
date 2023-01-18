using UnityEngine;
using UnityEngine.UI;

public class FlagIndicator : MonoBehaviour
{
    public Transform flag; // Assign the flag object in the inspector
    public Image leftIndicator; // Assign the left indicator image in the inspector
    public Image rightIndicator; // Assign the right indicator image in the inspector

    public Camera cam1;
    public Camera cam2;

    private float borderPadding = 32;

    public Color currentColour = Color.white;

    void Update()
    {
        // Get the flag's position on the screen
        Vector3 flagScreenPos1 = Vector3.zero, flagScreenPos2 = Vector3.zero;

        leftIndicator.color = currentColour;
        rightIndicator.color = currentColour;

        if (flag)
        {
            flagScreenPos1 = cam1.WorldToScreenPoint(flag.position);
            flagScreenPos2 = cam2.WorldToScreenPoint(flag.position);
            var horizontalRelation1 = new Vector3(flag.position.x, cam1.transform.position.y, flag.position.z);

            var targetDir = horizontalRelation1 - cam1.transform.position;
            var forward = cam1.transform.forward;
            var angle1 = Vector3.Angle(targetDir, forward);

            var horizontalRelation2 = new Vector3(flag.position.x, cam2.transform.position.y, flag.position.z);

            var targetDir2 = horizontalRelation2 - cam2.transform.position;
            var forward2 = cam2.transform.forward;
            var angle2 = Vector3.Angle(targetDir2, forward2);

            bool visible1 = angle1 < 145f;
            bool visible2 = angle2 < 145f;

            leftIndicator.gameObject.SetActive(visible1);
            rightIndicator.gameObject.SetActive(visible2);
        }else
        {
            leftIndicator.gameObject.SetActive(false);
            rightIndicator.gameObject.SetActive(false);
        }

        // Check if the flag is on the left side of the screen
        if (flagScreenPos1.x < Screen.width / 2 - borderPadding && flag)
        {
            // Position the left indicator at the flag's screen position
            leftIndicator.rectTransform.position = new Vector3(
                Mathf.Clamp(flagScreenPos1.x, borderPadding, Screen.width / 2f - borderPadding),
                Mathf.Clamp(flagScreenPos1.y, borderPadding, Screen.height - borderPadding), 0f);
            leftIndicator.enabled = true; // Enable the left indicator
        }/*else
        {
            leftIndicator.enabled = false;
        }*/

        if (flagScreenPos2.x > Screen.width / 2 + borderPadding && flag)
        {
            // Position the right indicator at the flag's screen position
            rightIndicator.rectTransform.position = new Vector3(
                Mathf.Clamp(flagScreenPos2.x, Screen.width / 2f + borderPadding, Screen.width - borderPadding),
                Mathf.Clamp(flagScreenPos2.y, borderPadding, Screen.height - borderPadding), 0f);
            rightIndicator.enabled = true; // Enable the right indicator
        }
        /*else
            rightIndicator.enabled = false;*/
    }
}
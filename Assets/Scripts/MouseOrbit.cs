using UnityEngine;
using System.Collections;
using System.Linq;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbit : MonoBehaviour
{
    public PlayerCar target;
    public Transform playerHip;

    public float distance = 5.0f;
    public static float xSpeed = 150.0f;
    public static float ySpeed = 100.0f;

    public string lookHorizontal = "Mouse X";
    public string lookVertical = "Mouse Y";

    public static float lookModifier = 1f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    private Rigidbody rigidbody;

    private ShootModule[] modules;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }

        //modules = target.Find("Modules").GetComponentsInChildren<ShootModule>();
    }

    void RotateGunModules()
    {
        //if (modules.Length == 0)
           // modules = target.Find("Modules").GetComponentsInChildren<ShootModule>();

        
        foreach (ShootModule g in target.gunModules)
        {
            Transform m = g.transform;
            m.eulerAngles = new Vector3(m.eulerAngles.x, transform.eulerAngles.y, m.eulerAngles.z);
            m.up = m.root.transform.TransformDirection(Vector3.up);
            m.GetChild(1).eulerAngles = transform.eulerAngles;
        }
    }

    void LateUpdate()
    {
        bool isMoving = Input.GetAxis("Vertical") != 0;

        if (target)
        {
            x += Input.GetAxis(lookHorizontal) * xSpeed * lookModifier * Time.deltaTime;
            y -= Input.GetAxis(lookVertical) * ySpeed * lookModifier * Time.deltaTime;
            /*else if (isMoving || Input.GetMouseButton(1))
              {
                  x = target.eulerAngles.y;
              }*/

            RotateGunModules();

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = transform.rotation;
            rotation = Quaternion.Euler(y, x, 0);

            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

            /*RaycastHit hit;
            int mask = 1 << 6; //TODO: idk if work
            mask = mask << 7;
            mask = ~mask;
            if (Physics.Linecast(target.position, transform.position, out hit, mask))
            {
                distance -= hit.distance;
            }*/
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + playerHip.position + Vector3.up;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
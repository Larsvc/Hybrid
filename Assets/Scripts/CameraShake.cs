using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    Vector3 originalPos;

    public float shakeLength = 0.2f;
    public float shakeTimer;
    public float shakeAmount = 3;
    public float shakeSpeed = 100;
    public bool isShaking = false;
    public bool shakeOnce = false;
    Vector3 newPos;

    void Awake()
    {
        instance = this;
        shakeTimer = shakeLength;
    }

    void OnEnable()
    {
        originalPos = transform.position;
    }

    /* public IEnumerator Shake(float duration, float magnitude)
	{
		originalPos = transform.localPosition;
		float elapsed = 0.0f;
		while (elapsed < duration)
		{
			float x = Random.Range(-1f, 1f) * magnitude;
			float y = Random.Range(-1f, 1f) * magnitude;
			transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
			elapsed += Time.deltaTime;
			yield return null;
		}
		transform.localPosition = originalPos;
	} */

    void Update()
    {
        if (shakeOnce)
        {
            Shake();
        }else
        {
            originalPos = transform.localPosition;
        }
    }

    public void startShaking(float length = 0.2f, float amount = 3f, float speed = 100f)
    {
        shakeTimer = length;
        shakeAmount = amount;
        shakeSpeed = speed;
        newPos = transform.localPosition;
        shakeOnce = true;
    }

    private void Shake()
    {
        if (shakeTimer > 0)
        {
            isShaking = true;

            if (Vector2.Distance(newPos, transform.localPosition) <= shakeAmount / 30) { newPos = originalPos + (Vector3)Random.insideUnitCircle * shakeAmount; }

            transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, Time.deltaTime * shakeSpeed);

            shakeTimer -= Time.deltaTime;
        }
        else
        {
            shakeTimer = 0f;
            transform.localPosition = originalPos;
            isShaking = false;
            shakeOnce = false;
        }
    }
}

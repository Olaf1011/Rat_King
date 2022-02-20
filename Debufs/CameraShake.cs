using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform tm;

    // Amplitude of the shake. A larger value shakes the camera harder.
    [SerializeField] private float shakeAmount = 0.7f;
    [SerializeField] private bool shake = false;
    Vector3 originalPos;
    // Start is called before the first frame update
    void Start()
    {
        tm = this.gameObject.GetComponent<Transform>();
        originalPos = tm.position;
    }

    void Update()
    {
        if (shake)
            Shake();
    }
        
    private void Shake()
    {
        Vector3 temp = (originalPos + Random.insideUnitSphere * shakeAmount);
        temp.z = -10.0f;
        tm.localPosition = temp;
    }

    public void StartShake()
    {
        shake = true;
    }

    public void ShakeDone()
    {
        shake = false;
        tm.position = originalPos;
    }
}

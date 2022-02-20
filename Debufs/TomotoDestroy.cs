using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomotoDestroy : MonoBehaviour
{
    private float timer;

    [SerializeField] private float Threshold;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > Threshold)
            Destroy(this.gameObject);
    }
}

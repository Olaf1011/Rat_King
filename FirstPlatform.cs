using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlatform : MonoBehaviour
{

    public float delay = 1.0f;
    public bool timerStarted = true;
    private PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void Update()
    {
        if (timerStarted)
        {
            delay -= Time.deltaTime;
            if (delay <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}

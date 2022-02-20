using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformControl : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < spawnPoints.Length; ++i)
        {
            GameObject instance = Instantiate(platformPrefab, spawnPoints[i].transform.position, Quaternion.identity);
            Rigidbody2D _rb = instance.GetComponent<Rigidbody2D>();

        }
        
    }
}

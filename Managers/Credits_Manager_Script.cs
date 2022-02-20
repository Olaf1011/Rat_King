using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Credits_Manager_Script : MonoBehaviour
{
    public List<GameObject> platforms;
    public List<GameObject> leftsideSpawns;
    public List<GameObject> rightsideSpawns;
    public float spawnRate;
    private float spawnTime;
    public float platformSpeed;
    private int currentPlatformIndex;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = 0.0f;
        currentPlatformIndex = 0;
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime >= spawnRate)
        {
            spawnTime = 0.0f;

            if (currentPlatformIndex == platforms.Count)
                currentPlatformIndex = 0;

            GameObject obj=Instantiate(platforms[currentPlatformIndex++], leftsideSpawns[Random.Range(0, 5)].transform.position,
                Quaternion.identity);
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -platformSpeed);

            if (currentPlatformIndex == platforms.Count)
                currentPlatformIndex = 0;

            obj=Instantiate(platforms[currentPlatformIndex++], rightsideSpawns[Random.Range(0, 5)].transform.position,
                Quaternion.identity);
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, platformSpeed);

        }

    }
}

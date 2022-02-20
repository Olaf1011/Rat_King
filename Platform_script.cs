using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_script : MonoBehaviour
{
    private Camera cam;

    private Transform tm;
    [SerializeField] private float destroyDelay = 1.0f;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        tm = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(!(cam.orthographicSize + 0.1f > tm.position.y && -(cam.orthographicSize - 0.1f) < tm.position.y) && timer > destroyDelay)
            Destroy(this.gameObject);
 
    }

    //private void OnBecameInvisible()
    //{
    //    Destroy(gameObject);
    //}
}

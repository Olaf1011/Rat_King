using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoThrowers : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private Vector2 range;
    [SerializeField] private float interval = .3f;
    [SerializeField] private GameObject tomato;

    private float timer;

    [SerializeField]  private bool StartThrowing;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        StartThrowing = false;
    }

    void Update()
    {
        if (GameManager.Instance.sGameState == GameManager.GameState.PLAYING)
        {
            if (StartThrowing)
            {
                timer += Time.deltaTime;
                if (timer >= interval)
                {
                    SpawnTOMATO();
                    timer = 0.0f;
                }
            }    
        }
    }

    private void SpawnTOMATO()
    {
        Instantiate(tomato, new Vector3(Random.Range(range.x, range.y), cam.orthographicSize + 0.5f, 0.0f),
            Quaternion.identity);

    }

    public void StartTomoto()
    {
        StartThrowing = true;
    }

    public void StopTomoto()
    {
        StartThrowing = false;
        timer = 0.0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    [SerializeField] private float thershold = 0.5f;
    public PlayerManager.Player player;
    private List<string> axisKey = new List<string>();
    private bool isStunned = false;
    private float timer;
    
    void Start()
    {
       
    }

    public void SetPlayer(PlayerManager.Player p)
    {
        player = p;
        axisKey.Add(player == PlayerManager.Player.ONE ? "Horizontal" : "HorizontalP2");
        axisKey.Add(player == PlayerManager.Player.ONE ? "Vertical" : "VerticalP2");
    }
    
    
    public bool CanProcessInput()
    {

        if (isStunned)
            timer += Time.deltaTime;
        if (timer >= thershold)
        {
            isStunned = false;
            timer = 0.0f;
        }
        return GameManager.Instance.sGameState == GameManager.GameState.PLAYING && !isStunned;
    }

    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Vector2 move = new Vector2(Input.GetAxisRaw(axisKey[0]), Input.GetAxisRaw(axisKey[1]));

            // always returns a magnitude of 1
            move = Vector3.ClampMagnitude(move, 1);

            return move;
        }

        return Vector3.zero;
    }
    
    public float GetHorizontalInput()
    {
        if (CanProcessInput())
        {
            float move = Input.GetAxisRaw(axisKey[0]);

            return move;
        }
        return 0;
    }
   
    public float GetVerticalInput()
    {
        if (CanProcessInput())
        {
            float move = Input.GetAxisRaw(axisKey[1]);

            return move;
        }
        return 0;
    }

    public void SetStunned()
    {
        isStunned = true;
    }
}
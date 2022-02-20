using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float delay = 3.0f;
    private bool killing = false;
    private PlayerManager player;
    private void Update()
    {
        if (killing)
        {
            delay -= Time.deltaTime;
            if (delay <= 0.0f)
            {
                FlagEndGame();
            }
        }
    }
    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            
            killing = true;
            player = other.GetComponentInParent<PlayerManager>();
            KillPlayer();
        }
    }
    private void KillPlayer()
    {
        player.PlayerDeath();
    }

    private void FlagEndGame()
    { 
        killing = false;
    }
}
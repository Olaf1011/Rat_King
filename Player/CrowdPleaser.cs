using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdPleaser : MonoBehaviour
{
    public enum Player
    {
        [InspectorName("One")]
        PLAYER_ONE,
        [InspectorName("Two")]
        PLAYER_TWO
    }
    public float CrowdMood { get { return crowdMood; } }
    public bool IsWaiting { set { isWaiting = value; } }

    
    [SerializeField] private float crowdMood = 0.0f;
    [SerializeField] private float crowdMoodMod = 20.0f;
    public float crowdMoodModMod = 1.0f;
    public float crowdMoodModModMod = 1.0f;
    [SerializeField] private float crowdMoodThreshold = 1.0f;
    [SerializeField] private float crowdMoodCooldown = 10.0f;
    [SerializeField] private float crowdMoodDecreaseRate = 1.0f;

    [SerializeField] private bool isWaiting = false;
    private DebuffManager debuffManager;


    [SerializeField] public Player player = Player.PLAYER_ONE;
    [SerializeField] private GameObject debuffManagerGameObject;
    private string key;

    // Start is called before the first frame update
    void Start()
    {
        if (debuffManagerGameObject == null)
        {
            debuffManagerGameObject = GameObject.FindGameObjectWithTag("DebuffManager");
        }
        debuffManager = debuffManagerGameObject.GetComponent<DebuffManager>();
        key = player == Player.PLAYER_ONE ? "ChargePlayerOne" : "ChargePlayerTwo";
    }


    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.sGameState == GameManager.GameState.PLAYING && GameManager.Instance.sSubState == GameManager.SubState.MAIN_GAME)
        {
            if(crowdMood > 0.0f)
            {
                crowdMood -= crowdMoodDecreaseRate * Time.deltaTime;
                if(crowdMood < 0.0f)
                {
                    crowdMood = 0.0f;
                }
            }
            IncreaseMood();
        }
    }

    private void IncreaseMood()
    {
        if (Input.GetButtonDown(key) && !isWaiting)
        {
            crowdMood += (Time.deltaTime * (crowdMoodMod * crowdMoodModMod)) * crowdMoodModModMod;
        }
        
        if (isWaiting)
        {
            crowdMood += Time.deltaTime;
            if (crowdMood >= crowdMoodCooldown)
            {
                isWaiting = false;
                crowdMood = 0.0f;
            }
        }
        else if (crowdMood >= crowdMoodThreshold)
        {
            isWaiting = true;
            crowdMood = 0.0f;
            debuffManager.SetPlayerDebuff(player);
        }
    }

}

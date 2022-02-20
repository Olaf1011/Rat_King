using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Platform_Manager_Script : MonoBehaviour
{
    public GameObject platform;
    public List<GameObject> leftsideSpawns;
    public List<GameObject> rightsideSpawns;
    public float spawnRate;
    private float spawnTime;
    [SerializeField] private float platformSpeed;
    public float platformSpeedMax;

    public float maxHorizontalSpawnGap = 2;

    private int prevRightSpawn = 2;
    private int prevLeftSpawn = 2;

    [Space(10)]
    [SerializeField] private List<GameObject> spawnedPlatformsRight = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnedPlatformsLeft = new List<GameObject>();
    [Space(10)]
    [SerializeField] private GameObject warningGlow;
    [Space(10)]
    
    [SerializeField] private Camera cam;
    [SerializeField] private float switchTimer = 0.0f;
    [SerializeField] private float maxTimer = 20.0f;
    [SerializeField] private float timer = 0.0f;
    [SerializeField] private float switchTimerTheshold = 30.0f;

    [SerializeField] private bool isFlipped = false;
    [SerializeField] private AnimationCurve test;
    

    private bool isStarting = true;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = 0.0f;
        if (cam == null)
            cam = FindObjectOfType<Camera>();
        FlipPlatforms();
        SoundManager.Instance.PlaySound(SoundManager.SoundNames.inGameMusicStart);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.sGameState != GameManager.GameState.PLAYING)
            return;
        
        if (isStarting)
        {
            timer += Time.deltaTime;

            platformSpeed = test.Evaluate(timer);
            if (Mathf.Abs(platformSpeed) >= platformSpeedMax)
            {
                isStarting = false;
                GameManager.Instance.sSubState = GameManager.SubState.MAIN_GAME;
            }
            
        }
        else
        {
            if(platformSpeed < 0.0f)
                platformSpeed = -platformSpeedMax;
            else
            {
                platformSpeed = platformSpeedMax;
            }
        }

        if(Mathf.Abs(platformSpeed) > 0.1f)
            spawnTime += Time.deltaTime * (Mathf.Abs(platformSpeed) / platformSpeedMax);

        if (spawnTime >= spawnRate)
        {
            spawnTime = 0.0f;

            int spawngap = Mathf.RoundToInt(Random.Range(-maxHorizontalSpawnGap, maxHorizontalSpawnGap));

            int newSpawn = prevLeftSpawn + spawngap;
            
            if(newSpawn < 0)
            {
                newSpawn = 0;
            }
            else if(newSpawn >= leftsideSpawns.Count)
            {
                newSpawn = 4;
            }

            if(newSpawn == prevLeftSpawn)
            {
                if(newSpawn == 4)
                {
                    --newSpawn;
                }
                else
                {
                    ++newSpawn;
                }
            }

            prevLeftSpawn = newSpawn;

            spawnedPlatformsLeft.Add(Instantiate(platform, leftsideSpawns[prevLeftSpawn].transform.position,
                Quaternion.identity));
            spawnedPlatformsLeft.Last().GetComponent<Rigidbody2D>().velocity = new Vector2(0, platformSpeed);

            spawngap = Mathf.RoundToInt(Random.Range(-maxHorizontalSpawnGap, maxHorizontalSpawnGap));

            newSpawn = prevRightSpawn + spawngap;

            if (newSpawn < 0)
            {
                newSpawn = 0;
            }
            else if (newSpawn >= leftsideSpawns.Count)
            {
                newSpawn = 4;
            }

            if (newSpawn == prevRightSpawn)
            {
                if (newSpawn == 4)
                {
                    --newSpawn;
                }
                else
                {
                    ++newSpawn;
                }
            }

            prevRightSpawn = newSpawn;

            spawnedPlatformsRight.Add(Instantiate(platform, rightsideSpawns[prevRightSpawn].transform.position,
                Quaternion.identity));
            spawnedPlatformsRight.Last().GetComponent<Rigidbody2D>().velocity = new Vector2(0, -platformSpeed);
        }

        spawnedPlatformsRight.RemoveAll(item => item == null);
        spawnedPlatformsLeft.RemoveAll(item => item == null);

        if (spawnedPlatformsLeft.Count > 0)
        {
            var debuff = spawnedPlatformsLeft.First().GetComponent<PlatformDebuffs>().currentDebuff;
            spawnedPlatformsLeft.Last().GetComponent<PlatformDebuffs>().SetAllDebuffs(debuff);
        }

        if (spawnedPlatformsRight.Count > 0)
        {
            var debuff = spawnedPlatformsRight.First().GetComponent<PlatformDebuffs>().currentDebuff;
            spawnedPlatformsRight.Last().GetComponent<PlatformDebuffs>().SetAllDebuffs(debuff);
    
        }
        
        if (timer < maxTimer)
        {
            foreach (var GAME_OBJECT in spawnedPlatformsLeft)
            {
                GAME_OBJECT.GetComponent<Rigidbody2D>().velocity = new Vector2(0, platformSpeed);
            }
            foreach (var GAME_OBJECT in spawnedPlatformsRight)
            {
                GAME_OBJECT.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -platformSpeed);
            }
        }
        if(GameManager.Instance.sSubState == GameManager.SubState.MAIN_GAME)
            switchTimer += Time.deltaTime;
        
        if (switchTimer > switchTimerTheshold)
        {
            FlipPlatforms(true);
        }


        if (!SoundManager.Instance.IsPlaying(SoundManager.SoundNames.inGameMusicStart))
        {
            if (!SoundManager.Instance.IsPlaying(SoundManager.SoundNames.inGameMusicMain) && timer + 10.0f > maxTimer)
                SoundManager.Instance.PlaySound(SoundManager.SoundNames.inGameMusicMain);
        }
    }

    public void SetDebuffAll(DebuffManager.Debuffs debuff)
    {
        foreach (var platRight in spawnedPlatformsRight)
        {
            platRight.GetComponent<PlatformDebuffs>().SetDebuff(debuff);
        }

        foreach (var platLeft in spawnedPlatformsLeft)
        {
            platLeft.GetComponent<PlatformDebuffs>().SetDebuff(debuff);
        }
    }

    public void ClearDebuffAll(DebuffManager.Debuffs debuf)
    {
        foreach (var platRight in spawnedPlatformsRight)
        {
            platRight.GetComponent<PlatformDebuffs>().ClearDebuff(debuf);
        }

        foreach (var platLeft in spawnedPlatformsLeft)
        {
            platLeft.GetComponent<PlatformDebuffs>().ClearDebuff(debuf);
        }
    }

    public void SetDebuffPlayer(CrowdPleaser.Player player, DebuffManager.Debuffs debuff)
    {
        if (player == CrowdPleaser.Player.PLAYER_TWO)
        {
            foreach (var platLeft in spawnedPlatformsLeft)
            {
                platLeft.GetComponent<PlatformDebuffs>().SetDebuff(debuff);
            }
        }
        else
        {
            foreach (var platRight in spawnedPlatformsRight)
            {
                platRight.GetComponent<PlatformDebuffs>().SetDebuff(debuff);
            }
        }
    }

    public void ClearDebuffPlayer(CrowdPleaser.Player player, DebuffManager.Debuffs debuff)
    {
        if (player == CrowdPleaser.Player.PLAYER_TWO)
        {
            foreach (var platLeft in spawnedPlatformsLeft)
            {
                platLeft.GetComponent<PlatformDebuffs>().ClearDebuff(debuff);
            }
        }
        else
        {
            foreach (var platRight in spawnedPlatformsRight)
            {
                platRight.GetComponent<PlatformDebuffs>().ClearDebuff(debuff);
            }
        }
    }


    private void FlipPlatforms(bool notStart = false)
    {
        if (notStart)
        {
            StartCoroutine(DoAFlip(5));
            int value = Random.Range(0,1);
            SoundManager.Instance.PlaySound(
                value == 1 ? SoundManager.SoundNames.kingDialogue : SoundManager.SoundNames.kingDialogueFast);
        }
        else
            StartCoroutine(DoAFlip(0));
            
    }

    private IEnumerator DoAFlip(float time)
    {
        switchTimer = 0.0f;
        warningGlow.SetActive(true);
        yield return new WaitForSeconds(time); 
        
        warningGlow.SetActive(false);
        SetPlatformPos(!isFlipped);
        platformSpeed *= -1.0f;

        foreach (var platLeft in spawnedPlatformsLeft)
        {
            if(platLeft != null)
                platLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(0, platformSpeed);
        }
        foreach (var platRight in spawnedPlatformsRight)
        {
            if(platRight != null)
                platRight.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -platformSpeed);
        }      }
    
    
    private void SetPlatformPos(bool flip)
    {
        const float OFFSET = 0.1f;
        if (flip)
        {

            foreach (var rSpawn in rightsideSpawns)
            {
                Vector3 pos = rSpawn.gameObject.GetComponent<Transform>().position;
                pos.Set(pos.x, (cam.orthographicSize + OFFSET), 0.0f);
                rSpawn.gameObject.GetComponent<Transform>().position = pos;
            }
            foreach (var lSpawn in leftsideSpawns)
            {
                Vector3 pos = lSpawn.gameObject.GetComponent<Transform>().position;
                pos.Set(pos.x, -(cam.orthographicSize + OFFSET), 0.0f);
                lSpawn.gameObject.GetComponent<Transform>().position = pos;
            }
        }
        else
        {
            foreach (var rSpawn in rightsideSpawns)
            {
                Vector3 pos = rSpawn.gameObject.GetComponent<Transform>().position;
                pos.Set(pos.x, -(cam.orthographicSize + OFFSET), 0.0f);
                rSpawn.gameObject.GetComponent<Transform>().position = pos;
            }
            foreach (var lSpawn in leftsideSpawns)
            {
                Vector3 pos = lSpawn.gameObject.GetComponent<Transform>().position;
                pos.Set(pos.x, (cam.orthographicSize + OFFSET), 0.0f);
                lSpawn.gameObject.GetComponent<Transform>().position = pos;
            }
        }

        isFlipped = flip;
    }
}

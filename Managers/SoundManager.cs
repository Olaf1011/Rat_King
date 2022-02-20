using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance => instance;
    private static SoundManager instance;

    public enum SoundNames
    {
        deathSound,
        kingDialogue,
        kingDialogueFast,
        victorySound,
        menuUp,
        menuDown,
        crowdBackground,
        crowdBuild,
        crowdCheers,
        gamePaused,
        gameUnpaused,
        inputConfirm,
        buffActive1,
        buffActive2,
        buffActive3,
        buffActive4,
        buffActive5,
        walkingSlow,
        walkingNormal,
        walkingFast,
        jump,
        jump2,
        impactToPlatform,
        impactToPlatform2,
        throwProjectile,
        spikeIncrease,
        projctileFalling,
        mainMenuMusic,
        inGameMusicStart,
        inGameMusicMain,
        pauseMenuMusic,
        postGameSummary,
        brittlePlatformBroken,
        honeyTrapActive,
        platformShrinking,
        inputBroken,
        screenShake,
        invertedControls,
        ratkingTheme,
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {

    }

    private void Update()
    {
        
    }
    [Serializable]
    public struct KeySF
    {
        public SoundNames sn;
        public AudioSource ass;
    }

    [SerializeField]
    private List<KeySF> soundEffects;

    public bool IsPlaying(SoundNames sn)
    {
        foreach (KeySF sf in soundEffects)
        {
            if (sf.sn == sn)
            {
                return sf.ass.isPlaying;
            }
        }

        return false;
    }

    public void PlaySound(SoundNames sn)
    {
        foreach(KeySF sf in soundEffects)
        {
            if(sf.sn == sn)
            {
                sf.ass.Play();
            }
        }
        
    }

    public void StopSound(SoundNames sn)
    {
        foreach (KeySF sf in soundEffects)
        {
            if (sf.sn == sn)
            {
                sf.ass.Stop();
            }
        }
    }
    public void PauseSound(SoundNames sn)
    {
        foreach (KeySF sf in soundEffects)
        {
            if (sf.sn == sn)
            {
                sf.ass.Pause();
            }
        }
    }

    public void PauseAll()
    {
        foreach (KeySF sf in soundEffects)
        {

            sf.ass.Pause();
        }
    }

    public void UnPauseAll()
    {
        foreach (KeySF sf in soundEffects)
        {

            sf.ass.UnPause();
        }
    }

    
    public void UnPauseSound(SoundNames sn)
    {
        foreach (KeySF sf in soundEffects)
        {
            if (sf.sn == sn)
            {
                sf.ass.UnPause();
            }
        }
    }

    public void StopAll()
    {
        foreach (KeySF sf in soundEffects)
        {
            sf.ass.Stop();
        }
    }
    
    
    public AudioSource GetAss(SoundNames sn)
    {
        foreach (KeySF sf in soundEffects)
        {
            if (sf.sn == sn)
            {
                return (sf.ass);
            }
        }
        return (soundEffects[0].ass);
    }

}

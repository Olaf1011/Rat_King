using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{

    public SoundManager.SoundNames LevelMusic;
    public SoundManager.SoundNames[] BackgroundNoise;
    // Start is called before the first frame update
    void Start()
    {
        PlayBackgroundNoise();
        SoundManager sm = SoundManager.Instance;
        sm.PlaySound(LevelMusic);
    }
    public void PlayBackgroundNoise()
    {
        SoundManager sm = SoundManager.Instance;

        foreach (SoundManager.SoundNames sn in BackgroundNoise)
        {
            sm.PlaySound(sn);
        }
    }public void StopBackgroundNoise()
    {
        SoundManager sm = SoundManager.Instance;

        foreach (SoundManager.SoundNames sn in BackgroundNoise)
        {
            sm.StopSound(sn);
        }
    }
    public void SetBackgroundVolume(float volume)
    {
        SoundManager sm = SoundManager.Instance;

        foreach (SoundManager.SoundNames sn in BackgroundNoise)
        {
            sm.GetAss(sn).volume = volume;
        }
    }
}

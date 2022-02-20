using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundNames.mainMenuMusic);
    }


    public void PlayButton ()
    {
        SceneManager.LoadScene("Game");
        SoundManager.Instance.StopSound(SoundManager.SoundNames.mainMenuMusic);
    }

    public void QuitGame ()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
        SoundManager.Instance.StopSound(SoundManager.SoundNames.mainMenuMusic);
    }

    public void OnHoverEnter()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundNames.menuUp);
    }
    public void OnHoverExit()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundNames.menuDown);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float defaultVolume = 0.5f;
    [SerializeField] AudioSource music;

    void Start()
    {
        slider.value = PlayerPrefsController.GetMasterVolume();
    }

    private void Update()
    {
        PlayerPrefsController.SetMasterVolume(slider.value);
        music.volume = PlayerPrefsController.GetMasterVolume();
    }

    public void SaveAndExit()
    {
        PlayerPrefsController.SetMasterVolume(slider.value);
        SceneManager.LoadScene("Main Menu");
    }

    public void DefaultVolume()
    {
        slider.value = defaultVolume;
    }
}

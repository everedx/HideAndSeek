using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Canvas canvas;

    public Slider masterSlider;

    public Slider sfxSlider;

    public Slider musicSlider;
    public void showOptions()
    {
        canvas.enabled = true;
    }
    public void hideOptions()
    {
        canvas.enabled = false;
    }

    public void UpdateVolumes()
    {
        float masterVolume, sfxVolume, musicVolume;
        GetSliderVolumes(out masterVolume, out sfxVolume, out musicVolume);

        if (GameManager.instanceExists)
        {
            GameManager.instance.SetVolumes(masterVolume, sfxVolume, musicVolume, false);
        }
    }

    void GetSliderVolumes(out float masterVolume, out float sfxVolume, out float musicVolume)
    {
        masterVolume = masterSlider != null ? masterSlider.value : 1;
        sfxVolume = sfxSlider != null ? sfxSlider.value : 1;
        musicVolume = musicSlider != null ? musicSlider.value : 1;
    }
}

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

    private float mastPrevValue;
    private float sfxPrevValue;
    private float musicPrevValue;
    float masterVolume, sfxVolume, musicVolume;

    private void Start()
    {
        mastPrevValue = 0;
        sfxPrevValue = 0;
        musicPrevValue = 0;
    }
    public void showOptions()
    {
        canvas.enabled = true;
        //set SlidersToVolume
        if (GameManager.instanceExists)
        {
            float master, sfx, music;
            GameManager.instance.GetVolumes(out master, out sfx, out music);

            if (masterSlider != null)
            {
                masterSlider.value = master;
            }
            if (sfxSlider != null)
            {
                sfxSlider.value = sfx;
            }
            if (musicSlider != null)
            {
                musicSlider.value = music;
            }
        }
    }
    public void hideOptions()
    {
        canvas.enabled = false;
    }

    public void UpdateVolumes()
    {
        
        GetSliderVolumes(out masterVolume, out sfxVolume, out musicVolume);

        if (GameManager.instanceExists)
        {
            GameManager.instance.SetVolumes(masterVolume, sfxVolume, musicVolume, false);
        }
    }
    public void UpdateVolumesSave()
    {

        GetSliderVolumes(out masterVolume, out sfxVolume, out musicVolume);

        if (GameManager.instanceExists)
        {
            GameManager.instance.SetVolumes(masterVolume, sfxVolume, musicVolume, true);
        }
    }


    void GetSliderVolumes(out float masterVolume, out float sfxVolume, out float musicVolume)
    {
        masterVolume = masterSlider != null ? masterSlider.value : 1;
        sfxVolume = sfxSlider != null ? sfxSlider.value : 1;
        musicVolume = musicSlider != null ? musicSlider.value : 1;
    }

    public void masterChangedValue()
    {
        GetSliderVolumes(out masterVolume, out sfxVolume, out musicVolume);
        //Debug.Log(masterVolume / 20 + " " + masterVolume + " " + mastPrevValue);
        if (mastPrevValue > masterVolume + 0.05f || mastPrevValue < masterVolume - 0.05f)
        {
            masterSlider.GetComponent<AudioSource>().Play();
            mastPrevValue = masterVolume;
        }
           
    }

    public void sfxChangedValue()
    {
        GetSliderVolumes(out masterVolume, out sfxVolume, out musicVolume);
        if (sfxPrevValue > sfxVolume + 0.05f || sfxPrevValue < sfxVolume - 0.05f)
        {
            sfxSlider.GetComponent<AudioSource>().Play();
            sfxPrevValue = sfxVolume;
        }
    }

    public void musicChangedValue()
    {
        GetSliderVolumes(out masterVolume, out sfxVolume, out musicVolume);
        if (musicPrevValue > musicVolume + 0.05f || musicPrevValue < musicVolume - 0.05f)
        {
            musicSlider.GetComponent<AudioSource>().Play();
            musicPrevValue = musicVolume;
        }
    }
}

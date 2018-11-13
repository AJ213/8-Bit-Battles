using UnityEngine;

public class VolumeValues : MonoBehaviour
{
    public float soundEffectVolume = 0.5f;
    public float musicVolume = 0.5f;

    public void SaveVolumeOptions()
    {
        PlayerPrefs.SetFloat("Music Volume", musicVolume);
        PlayerPrefs.SetFloat("Sound Effect Volume", soundEffectVolume);
    }
    public void LoadVolumeOptions()
    {
        soundEffectVolume = PlayerPrefs.GetFloat("Sound Effect Volume", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("Music Volume", 0.5f);
    }
}

using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("----------AudioSource----------")]
    public AudioSource BackgroundSource;
    public AudioSource SFXSource;

    [Header("----------AudioClip----------")]
    public AudioClip BackgroundMusic;
    public AudioClip swordSwing;
    public AudioClip hurt;
    void Start()
    {
        BackgroundSource.clip = BackgroundMusic;
        BackgroundSource.Play();
    }

    public void playSFX(AudioClip audioClip)
    {
        SFXSource.PlayOneShot(audioClip);
    }

}

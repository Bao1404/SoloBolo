using UnityEngine;

public class AudioMenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource buttonAudioSource;

    [SerializeField] private AudioClip backgroundAudioClip;
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip clickClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayBackgroundMusic();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void PlayBackgroundMusic()
    {
        backgroundAudioSource.clip = backgroundAudioClip;
        backgroundAudioSource.Play();
    }

    public void PlayHoverSound()
    {
        buttonAudioSource.PlayOneShot(hoverClip);
    }

    public void PlayClickSound()
    {
        buttonAudioSource.PlayOneShot(clickClip);
    }
}

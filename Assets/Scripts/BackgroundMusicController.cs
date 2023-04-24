using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{

    public static BackgroundMusicController Instance { get; private set; }

    private AudioSource audioSource;

    public AudioClip winMusic, loseMusic;
    private AudioClip basicMusic;

    private Dictionary<AudioClip, float> musicVolumes = new Dictionary<AudioClip, float>();


    private void Awake() 
    {
        if (Instance != this) 
            Instance = this;

        audioSource = GetComponent<AudioSource>();
        basicMusic = audioSource.clip;

        if (basicMusic != null) musicVolumes.Add(basicMusic, .25f);
        if (winMusic != null) musicVolumes.Add(winMusic, .35f);
        if (loseMusic != null) musicVolumes.Add(loseMusic, .75f);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null || audioSource.clip == clip) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.volume = musicVolumes[clip];
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayWinMusic() => PlayMusic(winMusic);
    public void PlayLoseMusic() => PlayMusic(loseMusic);
    public void PlayBasicMusic() => PlayMusic(basicMusic);



}

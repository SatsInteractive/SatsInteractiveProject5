using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private AudioSource musicAudioSource;
    
    [SerializeField] private AudioClip[] menuMusics;
    [SerializeField] private AudioClip[] mainMusics;
    
    private float audioLevel = 0.5f;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.loop = true;
        if (musicAudioSource == null)
        {
            musicAudioSource = gameObject.AddComponent<AudioSource>();
            musicAudioSource.loop = true;
        }
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    

    private IEnumerator PlayMenuMusicLoop()
    {
        int musicIndex = 0;

        while (true)
        {
            musicAudioSource.clip = menuMusics[musicIndex];
            musicAudioSource.Play();

            yield return new WaitForSeconds(musicAudioSource.clip.length);

            musicIndex = (musicIndex + 1) % menuMusics.Length;
        }
    }
    
    private IEnumerator PlayMainMusicLoop()
    {
        int musicIndex = 0;

        while (true)
        {
            musicAudioSource.clip = mainMusics[musicIndex];
            musicAudioSource.Play();

            yield return new WaitForSeconds(musicAudioSource.clip.length);

            musicIndex = (musicIndex + 1) % mainMusics.Length;
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            StopCoroutine(PlayMainMusicLoop());
            musicAudioSource.Stop();
            if (menuMusics.Length > 0)
            {
                StartCoroutine(PlayMenuMusicLoop());
            }
        }
        else if (scene.name == "Main")
        {
            StopCoroutine(PlayMenuMusicLoop());
            musicAudioSource.Stop();
            if (mainMusics.Length > 0)
            {
                StartCoroutine(PlayMainMusicLoop());
            }
        }
    }
    
    public void AdjustAudioLevel(float audioLevel)
    {
        this.audioLevel = audioLevel;
        musicAudioSource.volume = audioLevel;
    }
}

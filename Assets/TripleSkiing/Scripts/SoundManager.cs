using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;
    [SerializeField]
    public AudioClip bonus;
    [SerializeField]
    public AudioClip die;
    [SerializeField]
    public AudioClip gameOver;
    [SerializeField]
    public AudioClip MainMenu;
    [SerializeField]
    public AudioClip GamePlay;
    public AudioSource audioSource;

    // Use this for initialization
    void Awake () {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }

	}

    void Start()
    {
        soundBackgroundMenu();
    }

    public void playSoundBonus()
    {
        playAudioClip(bonus,1);
    }

    public void playSoundDie()
    {
        playAudioClip(die,1);
    }

    public void playSoundGameOver()
    {
        audioSource.clip = gameOver;
        audioSource.loop = false;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void soundBackgroundMenu()
    {
        audioSource.clip = MainMenu;
        audioSource.loop = true;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void soundBackgroundPlay()
    {
        audioSource.clip = GamePlay;
        audioSource.loop = true;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    void playAudioClip(AudioClip audio,float vol)
    {
        audioSource.PlayOneShot(audio, vol);
    }
}

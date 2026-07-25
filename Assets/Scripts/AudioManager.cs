using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class LevelAudio
    {
        public string sceneName;
        public AudioClip music;
    }

    [Header("Music")]
    public AudioSource musicSource;

    [Header("Level Music")]
    public LevelAudio[] levelAudio;

    private AudioClip currentLevelMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        PlayMusicForScene("Main Menu");
    }

    public void PlayMusicForScene(string sceneName)
    {
        foreach (LevelAudio level in levelAudio)
        {
            if (level.sceneName == sceneName)
            {
                if (level.music != null)
                {
                    currentLevelMusic = level.music;

                    musicSource.Stop();
                    musicSource.clip = currentLevelMusic;
                    musicSource.time = 0f;
                    musicSource.Play();
                }

                return;
            }
        }

        Debug.LogWarning("No music found for scene: " + sceneName);
    }

    public void RestartCurrentLevelMusic()
    {
        if (currentLevelMusic == null)
            return;

        musicSource.Stop();
        musicSource.clip = currentLevelMusic;
        musicSource.time = 0f;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
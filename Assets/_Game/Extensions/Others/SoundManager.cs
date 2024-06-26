using UnityEngine;
using System;

public enum SoundType
{
    Button = 0,
    Win = 1,
    Loose = 2,
    Die = 3,
    Attack = 4,
    WeaponHit = 5,
    SizeUp = 6
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private SoundList[] soundList;
    private AudioSource audioSource;
    public float currentVolume;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(Constant.SOUND_SETTING))
        {
            PlayerPrefs.SetInt(Constant.SOUND_SETTING, 1);
        }
        else
        {
            currentVolume = PlayerPrefs.GetInt(Constant.SOUND_SETTING);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        currentVolume = Instance.audioSource.volume;
    }

    public static void SoundSetting()
    {
        if (Instance.audioSource.volume > 0)
        {
            Instance.audioSource.volume = 0;
            PlayerPrefs.SetInt(Constant.SOUND_SETTING, 0);
        }
        else
        {
            Instance.audioSource.volume = 1;
            PlayerPrefs.SetInt(Constant.SOUND_SETTING, 1);
        }
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = Instance.soundList[(int)sound].getSounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        Instance.audioSource.PlayOneShot(randomClip, volume);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
}

[System.Serializable]
public struct SoundList
{
    public AudioClip[] getSounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}

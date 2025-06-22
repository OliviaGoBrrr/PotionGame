using UnityEngine;
using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine.Audio;

public enum SoundType
{
    MENU,
    GOOD,
    BAD,
    POTION,
    BERRIES,
    FEATHERS,
    HONEY,
    HERBS,
    MOSS,
    NUTS,
    ROOTS,
    WATER
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;
    private AudioSource AudioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, string soundName, float volume)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;

        for (int i = 0; i < clips.Length; i++)
        {
            if(clips[i].name.CompareTo(soundName) == 0)
            {
                AudioClip soundClip = clips[i];
                instance.AudioSource.PlayOneShot(soundClip, volume);
                return;
            }
        }

        Debug.LogError("No sound of that name in the selected sound clips");
    }

    public static void PlayRandomSound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.AudioSource.pitch = 1 * Mathf.Pow(1.059463f, UnityEngine.Random.Range(-3,2));
        instance.AudioSource.PlayOneShot(randomClip, volume);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for(int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif

}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}

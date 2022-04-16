using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundName
{
   NONE,
   MAIN_MENU,
   MISSION_CLEAR,
   TASK_CLEAR,
   ZAPPING,
   BOOKCASE,
   CHAIR,
   HURT,
   SWITCH,
   START,
   SKIP,
   THEME,
   DRINK,
   LOCKED,
}

public class AudioManager : Singleton<AudioManager>
{
    [Header("Settings")]
    [SerializeField] private float _randomPitchRange = 0.02f;

    [Header("Sounds")]
    public List<Sound> Sounds = new List<Sound>();

    private int _playingSoundsCount;

    public override void Awake()
    {
        base.Awake();

        foreach (Sound s in Sounds)
        {
            if (s.RandomPitch)
            {
                for (int i = 0; i < 3; i++)
                {
                    AudioSource source = gameObject.AddComponent<AudioSource>();
                    s.Sources.Add(source);
                    s.Sources[i].clip = s.Clip;
                    s.Sources[i].outputAudioMixerGroup = s.Output;
                    s.Sources[i].volume = s.Volume;
                    s.Sources[i].loop = s.Loop;
                    s.Sources[i].playOnAwake = s.PlayOnAwake;
                    s.Sources[i].pitch = (Random.Range(s.Pitch - _randomPitchRange, s.Pitch + _randomPitchRange));
                }
            }
            else
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                s.Sources.Add(source);
                foreach (AudioSource src in s.Sources)
                {
                    src.clip = s.Clip;
                    src.outputAudioMixerGroup = s.Output;
                    src.volume = s.Volume;
                    src.loop = s.Loop;
                    src.playOnAwake = s.PlayOnAwake;
                    src.pitch = s.Pitch;
                }
            }
        }
    }

    public void PlaySound(SoundName name)
    {
        Sound selectedSound = Sounds.Find(s => s.Name == name);

        if (selectedSound == null)
            return;

        if (selectedSound.PlayOneShot)
        {
            if (_playingSoundsCount > 20) return;
            StartCoroutine(PlayOneShot(selectedSound));
        }
        else
        {
            selectedSound.Sources[Random.Range(0, selectedSound.Sources.Count)].Play();
        }
    }

    IEnumerator PlayOneShot(Sound sound)
    {
        _playingSoundsCount++;
        sound.Sources[Random.Range(0, sound.Sources.Count)].PlayOneShot(sound.Clip, 1f);
        yield return new WaitForSeconds(sound.Clip.length);
        _playingSoundsCount--;
    }

    public void StopSound(SoundName name)
    {
        Sound selectedSound = Sounds.Find(s => s.Name == name);

        if (selectedSound == null)
            return;

        for (int i = 0; i < selectedSound.Sources.Count; i++)
        {
            selectedSound.Sources[i].Stop();
        }
    }

    public void PlayScarySound()
    {
        Sound selectedSound = Sounds.Find(s => s.Name == SoundName.THEME);

        selectedSound.Sources[0].pitch = 0.9f;
    }

    public void PlayNormalSound()
    {
        Sound selectedSound = Sounds.Find(s => s.Name == SoundName.THEME);

        selectedSound.Sources[0].pitch = 1.5f;
    }
}

[System.Serializable]
public class Sound
{
    public SoundName Name;
    public AudioClip Clip;
    public AudioMixerGroup Output;
    public bool PlayOneShot;
    public bool Loop;
    public bool PlayOnAwake;
    public bool RandomPitch;
    [Range(0f, 1f)] public float Volume;
    [Range(-3f, 3f)] public float Pitch;
    public List<AudioSource> Sources;
}
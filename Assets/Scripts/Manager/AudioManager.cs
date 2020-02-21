﻿using UnityEngine;
using System;

public class AudioManager : MonoBehaviour 
{
    [SerializeField] private Sound[] sounds;

    public static AudioManager instance;

   private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        //DontDestroyOnLoad(this.gameObject);

        foreach (Sound s in sounds)
        {
            s.source = this.gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = s.spatialBlend;
            s.source.loop = s.loop;
            s.source.rolloffMode = s.rollOffMode;
            s.source.minDistance = s.minDistance;
            s.source.maxDistance = s.maxDistance;
        }
    }

    private void Start()
    {
        Play(SoundNames.PILOT_BREATHING);
        Play(SoundNames.RADIO);
    }

    public void Play(string name)
    {
        Sound s = GetSound(name);

		if (s != null)
		{
			s.source.Play();
		}
    }

	public void Pause(string name)
	{
		Sound s = GetSound(name);

		if (s != null)
		{
			s.source.Pause();
		}
	}
	
    public AudioSource AttachSoundTo(string soundName, GameObject g)
    {
        Sound s = GetSound(soundName);

        s.source = g.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.spatialBlend = s.spatialBlend;
        s.source.loop = s.loop;
        s.source.rolloffMode = s.rollOffMode;
        s.source.minDistance = s.minDistance;
        s.source.maxDistance = s.maxDistance;

		return s.source;
    }

    public Sound GetSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            print("Warning! Sound: '" + name + "'was not found.");
            return null;
        }
        return s;
    }

    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        [Range(0f,1f)]
        public float volume;

        [Range(0.5f, 3f)]
        public float pitch;

        public bool loop;

        [Header("3D Sound Settings")]
        [Range(0f, 1f)]
        public float spatialBlend;

        public AudioRolloffMode rollOffMode;

        public float minDistance;

        public float maxDistance;

        [HideInInspector]
        public AudioSource source;
    }
}

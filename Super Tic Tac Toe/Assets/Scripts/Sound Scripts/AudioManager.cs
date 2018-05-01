using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour 
{
	[HideInInspector]
	public static AudioManager Instance;
	public Sound[] Sounds;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		else
		{
			Destroy(this.gameObject);
			return;
		}

		DontDestroyOnLoad(this.gameObject);

		foreach (Sound s in Sounds)
		{
			s.Source = gameObject.AddComponent<AudioSource>();
			s.Source.clip = s.Clip;
			s.Source.volume = s.Volume;
			s.Source.pitch = s.Pitch;
			s.Source.loop = s.Loop;
		}
	}

	void Start()
	{
		Play("ThemeMusic");
	}

	public void Play (string _name)
	{
		Sound _s = Array.Find(Sounds, Sound => Sound.Name == _name);

		if (_s == null)
			return;

		_s.Source.Play();
	}
}

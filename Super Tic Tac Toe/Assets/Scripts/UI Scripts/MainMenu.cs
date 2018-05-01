using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour 
{
	public GameObject AudioManagerObject;
	public Button PlayButton;
	public Button CreditsButton;
	public Button ExitButton;

	private AudioManager _audioManager;

	void Start () 
	{
		_audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;

		PlayButton.onClick.AddListener(delegate { PlaySound(); });
		CreditsButton.onClick.AddListener(delegate { PlaySound(); });
		ExitButton.onClick.AddListener(delegate { PlaySound(); });
	}

	void PlaySound()
	{
		_audioManager.Play("InitialMenuButtons");
	}
}

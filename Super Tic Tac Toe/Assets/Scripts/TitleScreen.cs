using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour 
{
	public GameObject GameMainMenu;
	public GameObject AudioManagerObject;

	private AudioManager _audioManager;

	void Start()
	{
		_audioManager = AudioManagerObject.GetComponent<AudioManager>();
	}

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			_audioManager.Play("StartGame");
			this.gameObject.SetActive(false);
			GameMainMenu.SetActive(true);
		}
	}
}

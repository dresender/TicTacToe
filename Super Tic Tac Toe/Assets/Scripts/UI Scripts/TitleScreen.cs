using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour 
{
	public GameObject GameMainMenu;

	private AudioManager _audioManager;

	void Start()
	{
		_audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
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

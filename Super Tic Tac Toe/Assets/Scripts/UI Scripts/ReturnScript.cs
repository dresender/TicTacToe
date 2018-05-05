using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnScript : MonoBehaviour 
{
	private AudioManager _audioManager;
	private GameManager _gameManager;

	void Awake()
	{
		_audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
		_gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
	}

	void OnMouseDown() 
	{
		_gameManager.ResetGame();
		_audioManager.Play("ReturnButton");
		SceneManager.LoadScene("Menu");
	}

}

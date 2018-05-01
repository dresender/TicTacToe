using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnScript : MonoBehaviour 
{
	public AudioManager _audioManager;

	void Awake()
	{
		_audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;
	}

	void OnMouseDown() 
	{
		_audioManager.Play("ReturnButton");
		SceneManager.LoadScene("Menu");
	}

}

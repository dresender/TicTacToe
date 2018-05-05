using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour 
{
	private AudioManager _audioManager;

	void Start () 
	{
		_audioManager = FindObjectOfType(typeof(AudioManager)) as AudioManager;		
	}

	public void PlayReturnButtonSound()
	{
		_audioManager.Play("ReturnButton");
	}
}

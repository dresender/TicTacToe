using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScript : MonoBehaviour 
{
	private GameManager _gameManagerscript;

	void Start () 
	{
		_gameManagerscript = FindObjectOfType(typeof(GameManager)) as GameManager;		
	}

	void OnMouseDown()
	{
		_gameManagerscript.ResetGame();
	}

}

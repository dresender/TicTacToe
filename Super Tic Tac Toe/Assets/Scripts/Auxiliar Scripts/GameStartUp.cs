using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartUp : MonoBehaviour 
{
	public GameObject GameManagerObject;
	private BoardManager _boardScript;

	void Start () 
	{
		_boardScript = FindObjectOfType(typeof(BoardManager)) as BoardManager;
		_boardScript.BoardInitialSetup();
	}
}

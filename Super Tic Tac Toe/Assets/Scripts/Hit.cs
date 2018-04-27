using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour {

	private GameManagement _script;

	void Awake()
	{
		_script = FindObjectOfType(typeof(GameManagement)) as GameManagement;
	}

	void OnMouseDown()
	{
		//if (_script.PlayerTurn == GameManagement.Player.Player)
		_script.PlaceNewPiece(this.gameObject);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour {

	private MatchManager _script;

	void Awake()
	{
		_script = FindObjectOfType(typeof(MatchManager)) as MatchManager;
	}

	void OnMouseDown()
	{
		if (_script._playerTurn == MatchManager.Player.Player)
		_script.PlaceNewPiece(this.gameObject);
	}

}

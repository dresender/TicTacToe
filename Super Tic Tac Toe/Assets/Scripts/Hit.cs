using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour {

	private GameManager _script;

	void Awake()
	{
		_script = FindObjectOfType(typeof(GameManager)) as GameManager;
	}

	void OnMouseDown()
	{
		if (_script.GameOverConfirmed != true)
		_script.PlaceNewPiece(this.gameObject);
	}

}

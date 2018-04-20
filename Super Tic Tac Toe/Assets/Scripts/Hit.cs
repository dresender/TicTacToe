using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour {

	//public GameObject Object;
	
	private MatchManager _script; 

	void Awake()
	{
		//_script = Object.GetComponent<MatchManager>();
		_script = FindObjectOfType(typeof(MatchManager)) as MatchManager;
	}

	void OnMouseDown()
	{
		_script.PlaceNewPiece(this.gameObject);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour {

	public GameObject Cross;
	public GameObject Circle;
	public enum Piece { Empty, Cross, Circle};
	public Piece Turn;

	public void SpawnNewPiece(GameObject obj)
	{
		if (Turn == Piece.Cross)
		{
			Instantiate(Cross, obj.transform.position, Quaternion.identity);
			Turn = Piece.Circle;
		}
		else
		{
			Instantiate(Circle, obj.transform.position, Quaternion.identity);
			Turn = Piece.Cross;
		}

		Destroy(obj.gameObject);
	}
}

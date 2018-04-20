using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public GameObject GameManager;
	public GameObject EmptyCell;
	public float BoardSize = 3f;


	private MatchManager _matchManager;
	private float BoardLines;
	private float BoardColumns;
	private Vector2 _position;

	void Start()
	{
		BoardLines = BoardSize;
		BoardColumns = BoardSize;
		_position = new Vector2(-4f, 4f);

		for (int i = 0; i < BoardColumns; i++)
		{
			_position -= new Vector2(0f, 2f);

			for(int j = 0; j < BoardLines; j++)
			{
				_position += new Vector2(2f, 0f);
				Instantiate(EmptyCell, _position, Quaternion.identity);
			}
		}
	}

}

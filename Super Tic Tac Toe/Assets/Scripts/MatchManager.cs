using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour {
	
	public int BoardSize = 3;
	public GameObject Cross;
	public GameObject Circle;
	public GameObject EmptyCell;
	public enum Piece { Empty, Cross, Circle};
	public Piece Turn;

	private int BoardLines;
	private int BoardColumns;
	private Vector2 _cellPosition;
	private Vector2[,] _cellPositionStorage;
	private Piece[,] _cellPiece;


	void Start()
	{
		BoardLines = BoardSize;
		BoardColumns = BoardSize;

		BoardInitialSetup();
	}
	public void PlaceNewPiece(GameObject obj)
	{
		if (Turn == Piece.Cross)
		{
			Instantiate(Cross, obj.transform.position, Quaternion.identity);
			UpdatingCellPiece(obj.transform.position, Piece.Cross);
			Turn = Piece.Circle;
		}
		else
		{
			Instantiate(Circle, obj.transform.position, Quaternion.identity);
			UpdatingCellPiece(obj.transform.position, Piece.Circle);
			Turn = Piece.Cross;
		}

		Destroy(obj.gameObject);
		CheckIfGameIsOver();
	}

	private void BoardInitialSetup()
	{
		//Setting up the array that will be used to identify what kind of Piece lies on each cell
		_cellPiece = new Piece[BoardLines, BoardColumns];

		//Starting setup position for the empty cells placement
		_cellPosition = new Vector2(-2 * BoardSize, BoardSize);

		_cellPositionStorage = new Vector2[BoardLines, BoardColumns];

		//Loop over every cell on the board and place a empty cell
		for (int i = 0; i < BoardColumns; i++)
		{
			for(int j = 0; j < BoardLines; j++)
			{
				_cellPosition += new Vector2(BoardSize, 0f);
				Instantiate(EmptyCell, _cellPosition, Quaternion.identity);
				_cellPiece[i,j] = Piece.Empty;
				_cellPositionStorage[i,j] = _cellPosition;
			}
			_cellPosition -= new Vector2(0f, BoardSize);
			_cellPosition -= new Vector2(BoardSize * BoardSize, 0f);
		}
	}

	private void UpdatingCellPiece(Vector2 _hit, Piece _type)
	{
		var _hitPosition = _hit;

		for (int i = 0; i < BoardColumns; i++)
		{
			for(int j = 0; j < BoardLines; j++)
			{
				if(_cellPositionStorage[i,j] == _hitPosition)
				{
					_cellPiece[i,j] = _type;
					Debug.Log(string.Format("{0} {1} {2}", i, j, _type));
				}
			}
		}
	}

	private bool CheckIfGameIsOver()
	{
		var _gameOver = false;

		//Check all lines

		return _gameOver;
	}
}

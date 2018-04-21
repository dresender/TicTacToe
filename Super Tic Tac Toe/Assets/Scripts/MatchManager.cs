using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour {
	
	public int BoardSize = 3;
	public GameObject Cross;
	public GameObject Circle;
	public GameObject EmptyCell;
	public enum Piece { Empty, Cross, Circle };
	public Piece Turn;

	private Vector2 _cellPosition;
	private Vector2[,] _cellPositionStorage;
	private Piece[,] _cellPiecesArray;
	private Piece _victoriousPlayer;
	private bool _gameOverConfirmed;


	void Start()
	{
		_victoriousPlayer = Piece.Empty;
		_gameOverConfirmed = false;

		BoardInitialSetup();
	}

	public void PlaceNewPiece(GameObject obj)
	{
		if(!_gameOverConfirmed)
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

			//Checking for a victorious player
			_victoriousPlayer = CheckIfGameIsOver();
			if (_victoriousPlayer != Piece.Empty)
				_gameOverConfirmed = true;
			
			Debug.Log(string.Format("{0}",_victoriousPlayer));		
		}
	}

	private void BoardInitialSetup()
	{
		//Setting up the array that will be used to identify what kind of Piece lies on each cell
		_cellPiecesArray = new Piece[BoardSize, BoardSize];

		//Starting setup position for the empty cells placement
		_cellPosition = new Vector2(-2 * BoardSize, BoardSize);

		//Setting up the array that will store the position of every cell in game world
		_cellPositionStorage = new Vector2[BoardSize, BoardSize];

		//Loop over every cell on the board and place a empty cell
		for (int i = 0; i < BoardSize; i++)
		{
			for(int j = 0; j < BoardSize; j++)
			{
				_cellPosition += new Vector2(BoardSize, 0f);
				Instantiate(EmptyCell, _cellPosition, Quaternion.identity);
				_cellPiecesArray[i,j] = Piece.Empty;
				_cellPositionStorage[i,j] = _cellPosition;
			}
			_cellPosition -= new Vector2(0f, BoardSize);
			_cellPosition -= new Vector2(BoardSize * BoardSize, 0f);
		}
	}

	private void UpdatingCellPiece(Vector2 _hit, Piece _type)
	{
		var _hitPosition = _hit;

		for (int i = 0; i < BoardSize; i++)
		{
			for(int j = 0; j < BoardSize; j++)
			{
				if(_cellPositionStorage[i,j] == _hitPosition)
					_cellPiecesArray[i,j] = _type;
			}
		}
	}

	private Piece CheckIfGameIsOver()
	{
		var _gameOver = Piece.Empty;

		_gameOver = CheckGameOverLines();
		if (_gameOver != Piece.Empty)
			return _gameOver;

		_gameOver = CheckGameOverColumns();
		if (_gameOver != Piece.Empty)
			return _gameOver;

		_gameOver = CheckGameOverDiagLeft();
		if (_gameOver != Piece.Empty)
			return _gameOver;

		_gameOver = CheckGameOverDiagRight();
		if (_gameOver != Piece.Empty)
			return _gameOver;

		return _gameOver;
	}

	private Piece CheckGameOverLines()
	{
		var _cross = 0;
		var _circle = 0;

		//Checking every column for a sequence of the same Piece
		for (int i = 0; i < BoardSize; i++)
		{
			for (int j = 0; j < BoardSize; j++)
			{
				_cross += _cellPiecesArray[i,j] == Piece.Cross ? 1 : 0;
				_circle += _cellPiecesArray[i,j] == Piece.Circle ? 1 : 0;				
			}

			if (_cross == BoardSize || _circle == BoardSize)
				Debug.Log(string.Format("CheckGameOverLines"));

			if (_cross == BoardSize)
				return Piece.Cross;

			if (_circle == BoardSize)
				return Piece.Circle;

			_cross = 0;
			_circle = 0;
		}

		return Piece.Empty;
	}

	private Piece CheckGameOverColumns()
	{
		var _cross = 0;
		var _circle = 0;

		//Checking every line for a sequence of the same Piece
		for (int j = 0; j < BoardSize; j++)
		{
			for (int i = 0; i < BoardSize; i++)
			{
				_cross += _cellPiecesArray[i,j] == Piece.Cross ? 1 : 0;
				_circle += _cellPiecesArray[i,j] == Piece.Circle ? 1 : 0;				
			}

			if (_cross == BoardSize || _circle == BoardSize)
			Debug.Log(string.Format("CheckGameOverColumns"));

			if (_cross == BoardSize)
				return Piece.Cross;

			if (_circle == BoardSize)
				return Piece.Circle;

			_cross = 0;
			_circle = 0;
		}	

		return Piece.Empty;
	}	

	private Piece CheckGameOverDiagLeft()
	{
		var _cross = 0;
		var _circle = 0;
		//Checking left to right diagonal for a sequence of the same Piece
		for (int i = 0; i < BoardSize; i++)
		{
				_cross += _cellPiecesArray[i,i] == Piece.Cross ? 1 : 0;
				_circle += _cellPiecesArray[i,i] == Piece.Circle ? 1 : 0;				
		}

		if (_cross == BoardSize || _circle == BoardSize)
		Debug.Log(string.Format("CheckGameOverDiagLeft"));
		
		if (_cross == BoardSize)
			return Piece.Cross;

		if (_circle == BoardSize)
			return Piece.Circle;

		return Piece.Empty;
	}

	private Piece CheckGameOverDiagRight()
	{
		var _cross = 0;
		var _circle = 0;
		var j = BoardSize - 1;

		//Checking right to left diagonal
		for (int i = 0; i < BoardSize; i++)
		{
				_cross += _cellPiecesArray[i,j] == Piece.Cross ? 1 : 0;
				_circle += _cellPiecesArray[i,j] == Piece.Circle ? 1 : 0;	
				j--;		
		}

		if (_cross == BoardSize || _circle == BoardSize)
		Debug.Log(string.Format("CheckGameOverDiagRight"));

		if (_cross == BoardSize)
			return Piece.Cross;

		if (_circle == BoardSize)
			return Piece.Circle;

		return Piece.Empty;
	}
}

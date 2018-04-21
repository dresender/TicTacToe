using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MatchManager : MonoBehaviour {
	
	public GameObject Cross;
	public GameObject Circle;
	public GameObject EmptyCell;
	public enum Piece { Empty, Cross, Circle };
	public enum Player { Player, AI };
	public int BoardSize = 3;
	public Player _playerTurn;

	private Vector2[,] _cellPositionStorage;
	private Vector2 _cellPosition;
	private Piece[,] _cellPiecesArray;
	private Piece[,] _cellPiecesArrayClone;
	private Piece _victoriousPlayer;
	private Piece _pieceTurn;
	private Piece _playerOnePieceType;
	private Piece _playerTwoPieceType;
	private Player _playerOne;
	private Player _playerTwo;
	private Player _aiTurnCount;
	private int[,] _scoreBoard;
	private int _score;
	private bool _gameOverConfirmed;

	void Start()
	{
		_victoriousPlayer = Piece.Empty;
		_gameOverConfirmed = false;

		//Temporary
		_playerTurn = _playerOne;
		_pieceTurn = Piece.Cross;
		_playerOne = Player.Player;
		_playerOnePieceType = Piece.Cross;
		_playerTwo = Player.AI;
		_playerTwoPieceType = Piece.Circle;

		BoardInitialSetup();
	}

	public void PlaceNewPiece(GameObject obj)
	{
		if(!_gameOverConfirmed)
		{
			if (_playerTurn == Player.Player)
			{
				if (_pieceTurn == Piece.Cross)
				{
					Instantiate(Cross, obj.transform.position, Quaternion.identity);
					UpdatingCellPiecesArray(obj.transform.position, Piece.Cross);
					_pieceTurn = Piece.Circle;
					_playerTurn = ChangePlayerTurn(_playerTurn);
					Destroy(obj.gameObject);

					if (_playerTurn == Player.AI)
						MakeMove();
				}
				else
				{
					Instantiate(Circle, obj.transform.position, Quaternion.identity);
					UpdatingCellPiecesArray(obj.transform.position, Piece.Circle);
					_pieceTurn = Piece.Cross;
					_playerTurn = ChangePlayerTurn(_playerTurn);
					Destroy(obj.gameObject);

					if (_playerTurn == Player.AI)
						MakeMove();
				}
			}			

			//Checking for a victorious player
			_victoriousPlayer = CheckIfGameIsOver(_cellPiecesArray);
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

		//Setting up the scoreBoard for the Minimax algorithm AI
		_scoreBoard = new int[BoardSize, BoardSize];

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

	private void UpdatingCellPiecesArray(Vector2 _hit, Piece _type)
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

	private Piece CheckIfGameIsOver(Piece[,] _board)
	{
		var _gameOver = Piece.Empty;

		_gameOver = CheckGameOverLines(_board);
		if (_gameOver != Piece.Empty)
			return _gameOver;

		_gameOver = CheckGameOverColumns(_board);
		if (_gameOver != Piece.Empty)
			return _gameOver;

		_gameOver = CheckGameOverDiagLeft(_board);
		if (_gameOver != Piece.Empty)
			return _gameOver;

		_gameOver = CheckGameOverDiagRight(_board);
		if (_gameOver != Piece.Empty)
			return _gameOver;

		return _gameOver;
	}

	private Piece CheckGameOverLines(Piece[,] _board)
	{
		var _cross = 0;
		var _circle = 0;

		//Checking every column for a sequence of the same Piece
		for (int i = 0; i < BoardSize; i++)
		{
			for (int j = 0; j < BoardSize; j++)
			{
				_cross += _board[i,j] == Piece.Cross ? 1 : 0;
				_circle += _board[i,j] == Piece.Circle ? 1 : 0;				
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

	private Piece CheckGameOverColumns(Piece[,] _board)
	{
		var _cross = 0;
		var _circle = 0;

		//Checking every line for a sequence of the same Piece
		for (int j = 0; j < BoardSize; j++)
		{
			for (int i = 0; i < BoardSize; i++)
			{
				_cross += _board[i,j] == Piece.Cross ? 1 : 0;
				_circle += _board[i,j] == Piece.Circle ? 1 : 0;				
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

	private Piece CheckGameOverDiagLeft(Piece[,] _board)
	{
		var _cross = 0;
		var _circle = 0;
		//Checking left to right diagonal for a sequence of the same Piece
		for (int i = 0; i < BoardSize; i++)
		{
				_cross += _board[i,i] == Piece.Cross ? 1 : 0;
				_circle += _board[i,i] == Piece.Circle ? 1 : 0;				
		}

		if (_cross == BoardSize || _circle == BoardSize)
		Debug.Log(string.Format("CheckGameOverDiagLeft"));
		
		if (_cross == BoardSize)
			return Piece.Cross;

		if (_circle == BoardSize)
			return Piece.Circle;

		return Piece.Empty;
	}

	private Piece CheckGameOverDiagRight(Piece[,] _board)
	{
		var _cross = 0;
		var _circle = 0;
		var j = BoardSize - 1;

		//Checking right to left diagonal
		for (int i = 0; i < BoardSize; i++)
		{
				_cross += _board[i,j] == Piece.Cross ? 1 : 0;
				_circle += _board[i,j] == Piece.Circle ? 1 : 0;	
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

	private Player ChangePlayerTurn(Player p)
	{
		var _currentPlayer = p;

		if (_currentPlayer == _playerOne)
			return _playerTwo;
		else
			return _playerOne;
	}

	private void MakeMove()
	{	
		Vector2 _move = new Vector2();
		CloneCellPiecesArray();
		var _emptyCells = CheckHowManyEmptyCells(_cellPiecesArrayClone);
		_score = 0;

		//Find out the best move

		//Plays the best move found
			//With the celltransform to play, play the correct Piece on the correct spot
		//Pass turn to the Player						
	}

	private int Minimax(Piece[,] _board, int _depth)
	{	
		var _gameWonResult = CheckIfGameIsOver(_board);

		if (_gameWonResult != Piece.Empty || _depth == 0)
			return CheckWhichPlayerWon(_gameWonResult);

		if (_aiTurnCount == Player.Player)
		{
			for (var i = 0; i < BoardSize; i++)
			{
				for (var j = 0; j < BoardSize; j++)
				{
					if (_board[i,j] == Piece.Empty)
						_board[i,j] = _playerOnePieceType;
						_aiTurnCount = Player.AI;
						_scoreBoard[i,j] = Minimax(_board, _depth - 1);
				}
			}
		}
		else
		{
			for (var i = 0; i < BoardSize; i++)
			{
				for (var j = 0; j < BoardSize; j++)
				{
					if (_board[i,j] == Piece.Empty)
						_board[i,j] = _playerTwoPieceType;
						_aiTurnCount = Player.Player;
						_scoreBoard[i,j] = Minimax(_board, _depth - 1);
				}
			}
		}

		return;
	}

	private int CheckWhichPlayerWon(Piece p)
	{
		if (p == _playerOnePieceType)
			return -1;
		if (p == _playerTwoPieceType)
			return 1;

		return 0;
	}

	private int CheckHowManyEmptyCells(Piece[,] _board)
	{
		var _emptyCells = 0;

		for (int i = 0; i < BoardSize; i++)
		{
			for(int j = 0; j < BoardSize; j++)
			{
				if(_board[i,j] == Piece.Empty)
					_emptyCells++;
			}
		}
		return _emptyCells;
	}

	private Piece[,] CloneCellPiecesArray()
	{
		return _cellPiecesArrayClone = (Piece[,])_cellPiecesArray.Clone();
	}

	private void ClearCellPiecesArrayClone()
	{
		Array.Clear(_cellPiecesArrayClone, 0, _cellPiecesArrayClone.Length);
	}
}

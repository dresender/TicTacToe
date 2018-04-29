using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public GameObject GameBoard;
	public enum Player { PlayerOne, PlayerTwo };
	public enum PlayerControl { Human, AI }
	public enum PlayerPawn { Cross, Circle };
	public enum GameDifficulty { Easy, Hard };
	public Player Turn;
	public PlayerControl PlayerOne = PlayerControl.Human;
	public PlayerControl PlayerTwo = PlayerControl.AI;
	public PlayerPawn PlayerOnePawn = PlayerPawn.Cross;
	public PlayerPawn PlayerTwoPawn = PlayerPawn.Circle;
	public GameDifficulty GameDifficultyChoice;
	public bool GameOverConfirmed = false;
	public bool GameStarted = false;


	private BoardManager _board;
	private GameObject _pawn;
	private GameObject _playerOneDesignatedPawn;
	private GameObject _playerTwoDesignatedPawn;
	private GameObject _victoriousPlayer;
	private PlayerControl _aiTurnCount;
	private PlayerControl _currentPlayerController;
	private int _bestLine;
	private int _bestColumn;
	private int[,] _scoreArray;

	void Start()
	{
		_board = GameBoard.GetComponent<BoardManager>();
		_scoreArray = new int[_board.BoardSize, _board.BoardSize];
		SetupPlayerPawns();
		DontDestroyOnLoad(this.gameObject);

		//Temporary
		_currentPlayerController = PlayerControl.Human;
		GameDifficultyChoice = GameDifficulty.Hard;
	}

	public void PlaceNewPiece(GameObject obj)
	{
		if (!GameOverConfirmed)
		{
			if (Turn == Player.PlayerOne)
				_pawn = _playerOneDesignatedPawn;
			else
				_pawn = _playerTwoDesignatedPawn;

			//Human Player
			if (_currentPlayerController == PlayerControl.Human && !GameOverConfirmed)
			{
				var _instantiatedGameObject = Instantiate(_pawn, obj.transform.position, Quaternion.identity);
				_board.UpdateBoard(_instantiatedGameObject);
				Turn = ChangePlayerTurn(Turn);
				Destroy(obj.gameObject);
				CheckForGameOverCondition();
			}
			//AI Player
			if (_currentPlayerController == PlayerControl.AI && !GameOverConfirmed)
			{
				MakeAIMove();
				Turn = ChangePlayerTurn(Turn);
				CheckForGameOverCondition();
			}
		}
	}

	private void SetupPlayerPawns()
	{
		if (PlayerOnePawn == PlayerPawn.Cross)
			_playerOneDesignatedPawn = _board.Cross;
		else
			_playerOneDesignatedPawn = _board.Circle;

		if (PlayerTwoPawn == PlayerPawn.Cross)
			_playerTwoDesignatedPawn = _board.Cross;
		else
			_playerTwoDesignatedPawn = _board.Circle;
	}

	private Player ChangePlayerTurn(Player _p)
	{
		if (_p == Player.PlayerOne)
			_p = Player.PlayerTwo;
		else
			_p = Player.PlayerOne;
		
		if (_currentPlayerController == PlayerControl.Human)
			_currentPlayerController = PlayerControl.AI;
		else
			_currentPlayerController = PlayerControl.Human;

		return _p;
	}

	private void CheckForGameOverCondition()
	{
		//Checking for a victorious player
		_victoriousPlayer = _board.CheckIfGameIsOver(_board.Board, false);
		if (_victoriousPlayer != _board.EmptyCell)
			GameOverConfirmed = true;

		var count = 0;

		for (int i = 0; i < _board.BoardSize; i++)
		{
			for (int j = 0; j < _board.BoardSize; j++)
			{
				if (_board.Board[i, j].gameObject.tag == _board.EmptyCell.tag)
					count++;
			}
		}

		if (count == 0)
			GameOverConfirmed = true;
	}

	private void MakeAIMove()
    {
		//Clone the game Board array to use in the AI searching movement script
		var _boardCopy = CloneArray(_board.Board);

		//Find out all the empty cells in the cellPiecesArray cloned
		var _emptyCells = CheckHowManyEmptyCells(_boardCopy);

		//Minimax Call
		_aiTurnCount = PlayerControl.AI;
		Minimax(_boardCopy, _emptyCells, true);

		//Move Position from Minimax
		var _newMovePosition = _board.Board[_bestLine, _bestColumn];

		//Destroy the empty cell
		Destroy(_board.Board[_bestLine, _bestColumn].gameObject);

		GameObject _newPositionObject = new GameObject();
		_newPositionObject.transform.position = _newMovePosition.transform.position;

		if (_playerTwoDesignatedPawn.gameObject.tag == _board.Circle.gameObject.tag)
		{
			_board.Circle.transform.position = _newPositionObject.transform.position;
			_newPositionObject = _board.Circle;
			_board.UpdateBoard(_newPositionObject);
		}

		if (_playerTwoDesignatedPawn.gameObject.tag == _board.Cross.gameObject.tag)
		{ 
			_board.Cross.transform.position = _newPositionObject.transform.position;
			_newPositionObject = _board.Cross;
			_board.UpdateBoard(_newPositionObject);
		}

		Debug.Log(string.Format("{0}, {1}, {2}", _scoreArray[0,0], _scoreArray[0,1], _scoreArray[0,2]));
		Debug.Log(string.Format("{0}, {1}, {2}", _scoreArray[1,0], _scoreArray[1,1], _scoreArray[1,2]));
		Debug.Log(string.Format("{0}, {1}, {2}", _scoreArray[2,0], _scoreArray[2,1], _scoreArray[2,2]));

        //Plays the best move found
        Instantiate(_newPositionObject, _newPositionObject.transform.position, Quaternion.identity);
    }

    private int Minimax(GameObject[,] _b, int _depth, bool firstTime = false)
	{
		var _gameWonResult = _board.CheckIfGameIsOver(_b);

		if (_gameWonResult != _board.EmptyCell || _depth == 0)
			return CheckWhichPlayerWon(_gameWonResult);

		if (_aiTurnCount == PlayerControl.AI)
		{
			int max = -99;
			for (var i = 0; i < _board.BoardSize; i++)
			{
				for (var j = 0; j < _board.BoardSize; j++)
				{
					//Check for the first empty Spot
					if (_b[i, j].gameObject.tag == _board.EmptyCell.gameObject.tag)
					{
						//Assign Human player pawn to the empty spot
						_b[i, j] = _playerTwoDesignatedPawn;
						_aiTurnCount = PlayerControl.Human;

						//Calls Minimax recursively, sending the updated temp board
						//and decreasing the depth by one!
						int next = Minimax(_b, _depth - 1);

						//For debugging purposes, the _scoreArray is updated
						//with the next value
						_scoreArray[i,j] = next;

						//If the terminal node returns a next smaller number,
						//thats the new min (favorite move for the AI Player)
						if (next > max)
						{
							max = next;

							//If this is the initial board (or the board to choose
							//a movement), then update the game board array index variables
							if (firstTime)
							{
								_bestLine = i;
								_bestColumn = j;
							}
						}

						//Resets the temporary board for the next moves
						_b[i, j] = _board.EmptyCell;
					}
				}
			}
			return max;
		}
		else
		{
			int min = 99;
			for (var i = 0; i < _board.BoardSize; i++)
			{
				for (var j = 0; j < _board.BoardSize; j++)
				{
					if (_b[i, j].gameObject.tag == _board.EmptyCell.gameObject.tag)
					{
						//Assign Human player pawn to the empty spot
						_b[i, j] = _playerOneDesignatedPawn;

						//Passes the turn for the AI
						_aiTurnCount = PlayerControl.AI;

						//Calls Minimax recursively, sending the updated temp board
						//and decreasing the depth by one!
						int next = Minimax(_b, _depth - 1);

						//If the terminal node returns a next smaller number,
						//thats the new min (favorite move for the Human Player)
						if (next < min)
						{
							min = next;
						}

						//Resets the temporary board for the next moves
						_b[i, j] = _board.EmptyCell;
					}
				}
			}
			return min;
		}        
	}

	private int CheckHowManyEmptyCells(GameObject[,] _b)
	{
		var _emptyCells = 0;

		for (int i = 0; i < _board.BoardSize; i++)
		{
			for(int j = 0; j < _board.BoardSize; j++)
			{
				if(_b[i,j].gameObject.tag == _board.EmptyCell.gameObject.tag)
					_emptyCells++;
			}
		}
		return _emptyCells;
	}

	private int CheckWhichPlayerWon(GameObject _obj)
	{
		if (_obj == _playerOneDesignatedPawn)
			return -1;
		if (_obj == _playerTwoDesignatedPawn)
			return 1;

		return 0;
	}

	private GameObject[,] CloneArray(GameObject[,] _array)
	{
		var _arrayClone = new GameObject[_board.BoardSize, _board.BoardSize];
		_arrayClone = (GameObject[,])_array.Clone();
		return _arrayClone;
	}

	private void ClearArray(GameObject[,] _array)
	{
		Array.Clear(_array, 0, _array.Length);
	}

	private void ClearArray(int[,] _array)
	{
		Array.Clear(_array, 0, _array.Length);
	}
}

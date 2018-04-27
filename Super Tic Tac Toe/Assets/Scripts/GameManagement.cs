using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour 
{
	public GameObject GameBoard;
	public enum Player { PlayerOne, PlayerTwo };
	public enum PlayerControl { Human, AI }
	public enum PlayerPawn { Cross, Circle };
	public Player Turn;
	public PlayerControl PlayerOne = PlayerControl.Human;
	public PlayerControl PlayerTwo = PlayerControl.Human;
	public PlayerPawn PlayerOnePawn = PlayerPawn.Cross;
	public PlayerPawn PlayerTwoPawn = PlayerPawn.Circle;


	private BoardManager _board;
	private GameObject _victoriousPlayer;
	private GameObject _pawn;
	private GameObject[,] _boardArrayClone;
	private PlayerControl _currentPlayerController;
	private GameObject _playerOneDesignatedPawn;
	private GameObject _playerTwoDesignatedPawn;
	private PlayerControl _aiTurnCount;
	private bool _gameOverConfirmed;
	private int _bestLine;
	private int _bestColumn;

	void Start()
	{
		_board = GameBoard.GetComponent<BoardManager>();
		_boardArrayClone = new GameObject[_board.BoardSize, _board.BoardSize];

		SetupPlayerPawns();

		//Temporary
		_currentPlayerController = PlayerControl.Human;
	}

	public void PlaceNewPiece(GameObject obj)
	{
		if (!_gameOverConfirmed)
		{
			if (Turn == Player.PlayerOne)
				_pawn = _playerOneDesignatedPawn;
			else
				_pawn = _playerTwoDesignatedPawn;

			//Human Player
			if (_currentPlayerController == PlayerControl.Human)
			{
				var _instantiatedGameObject = Instantiate(_pawn, obj.transform.position, Quaternion.identity);
				_board.UpdateBoard(_instantiatedGameObject);
				Turn = ChangePlayerTurn(Turn);
				Destroy(obj.gameObject);

				MakeAIMove();
			}
			// else
			// {
			// 	MakeAIMove();
			// }

			//Checking for a victorious player
			_victoriousPlayer = _board.CheckIfGameIsOver(_board.Board);
			Debug.Log(_victoriousPlayer);
			if (_victoriousPlayer != _board.EmptyCell)
				_gameOverConfirmed = true;
		}
	}

	void SetupPlayerPawns()
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

	private void MakeAIMove()
    {
        //Clone the game Board array to use in the AI searching movement script
        CloneBoardArray(_board.Board);
		
        //Find out all the empty cells in the cellPiecesArray cloned
        var _emptyCells = CheckHowManyEmptyCells(_boardArrayClone);
		Debug.Log(_emptyCells);

        //Find out the best move
        _aiTurnCount = PlayerControl.AI;
        Minimax(_boardArrayClone, _emptyCells, true);

        var _newMovePosition = _board.Board[_bestLine, _bestColumn];

        //Destroy the empty cell
        Destroy(_board.Board[_bestLine, _bestColumn]);

        GameObject _newPositionObject = new GameObject();
        _newPositionObject.transform.position = _newMovePosition.transform.position;

        if (_playerTwoDesignatedPawn.gameObject.tag == _board.Circle.gameObject.tag)
        {
            _newPositionObject = _board.Circle;
			_board.UpdateBoard(_newPositionObject);
			Debug.Log(_newPositionObject);
        }

        if (_playerTwoDesignatedPawn.gameObject.tag == _board.Cross.gameObject.tag)
        { 
            _newPositionObject = _board.Cross;
            _board.UpdateBoard(_newPositionObject);
        }

        //Plays the best move found
        Instantiate(_newPositionObject, _newPositionObject.transform.position, Quaternion.identity);

        //With the celltransform to play, play the correct Piece on the correct spot
        //Pass turn to the Player
        Turn = ChangePlayerTurn(Turn);

		// for (int i = 0; i < _board.BoardSize; i++)
		// {
		// 	for (int j = 0; j < _board.BoardSize; j++)
		// 	{
		// 		Debug.Log(string.Format("{0}, {1}, {2}", i, j, _boardArrayClone[i,j]));
		// 	}
		// }

        //Clear cellPiecesArray cloned
        ClearBoardArrayClone();
    }

    private int Minimax(GameObject[,] _b, int _depth, bool firstTime = false)
    {
        var _gameWonResult = _board.CheckIfGameIsOver(_b);

        if (_gameWonResult != _board.EmptyCell || _depth == 0)
            return CheckWhichPlayerWon(_gameWonResult);

        if (_aiTurnCount == PlayerControl.AI)
        {
            int max = -999;
            for (var i = 0; i < _board.BoardSize; i++)
            {
                for (var j = 0; j < _board.BoardSize; j++)
                {
                    if (_b[i, j].gameObject.tag == _board.EmptyCell.gameObject.tag)
                    {
                        _b[i, j] = _playerTwoDesignatedPawn;
                        _aiTurnCount = PlayerControl.Human;
                        int next = Minimax(_b, _depth - 1);

                        if (next > max)
                        {
                            max = next;
                            if (firstTime)
                            {
                                _bestLine = i;
                                _bestColumn = j;
								Debug.Log(string.Format("{0}, {1}", i, j));
                            }
                        }
                    }
                }
            }
            return max;
        }
        else
        {
            int min = 999;
            for (var i = 0; i < _board.BoardSize; i++)
            {
                for (var j = 0; j < _board.BoardSize; j++)
                {
                    if (_b[i, j].gameObject.tag == _board.EmptyCell.gameObject.tag)
                    {
                        _b[i, j] = _playerOneDesignatedPawn;
                        _aiTurnCount = PlayerControl.AI;
                        int next = Minimax(_b, _depth - 1);

                        if (next < min)
                        {
                            min = next;
                        }
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

	private GameObject[,] CloneBoardArray(GameObject[,] _b)
	{
		_boardArrayClone = new GameObject[_board.BoardSize, _board.BoardSize];
		return _boardArrayClone = (GameObject[,])_b.Clone();
	}

	private void ClearBoardArrayClone()
	{
		Array.Clear(_boardArrayClone, 0, _boardArrayClone.Length);
	}
}

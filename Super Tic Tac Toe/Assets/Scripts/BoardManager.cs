﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour 
{
	public int BoardSize = 3;
	public GameObject Cross;
	public GameObject Circle;
	public GameObject EmptyCell;
	public GameObject DiagonalLeftLine;
	public GameObject DiagonalRightLine;
	public GameObject HorizontalLine;
	public GameObject VerticalLine;
	public GameObject[,] Board;

	void Start()
	{
		Board = new GameObject[BoardSize, BoardSize];

		DontDestroyOnLoad(this.gameObject);
		BoardInitialSetup();
	}

	public void UpdateBoard(GameObject _gameObject)
	{
		for (int i = 0; i < BoardSize; i++)
		{
			for (int j = 0; j < BoardSize; j++)
			{
				if (Board[i,j].transform.position == _gameObject.transform.position)
				{
					Board[i,j] = _gameObject;
				}
			}
		}
	}

	public void BoardInitialSetup()
	{
		var _cellPosition = new Vector2(-2 * BoardSize, BoardSize);
		
		//Loop over every cell on the board and place a empty cell
		for (int i = 0; i < BoardSize; i++)
		{
			for(int j = 0; j < BoardSize; j++)
			{
				_cellPosition += new Vector2(BoardSize, 0f);
                Board[i,j] = Instantiate(EmptyCell, _cellPosition, Quaternion.identity);
			}
			_cellPosition -= new Vector2(BoardSize * BoardSize, BoardSize);
		}
	}	

	public GameObject CheckIfGameIsOver(GameObject[,] _b, bool _minimaxCheck = true)
	{
		var _gameOver = EmptyCell;

		_gameOver = CheckGameOverLines(_b, _minimaxCheck);
		if (_gameOver != EmptyCell)
			return _gameOver;

		_gameOver = CheckGameOverColumns(_b, _minimaxCheck);
		if (_gameOver != EmptyCell)
			return _gameOver;

		_gameOver = CheckGameOverDiagLeft(_b, _minimaxCheck);
		if (_gameOver != EmptyCell)
			return _gameOver;

		_gameOver = CheckGameOverDiagRight(_b, _minimaxCheck);
		if (_gameOver != EmptyCell)
			return _gameOver;

		return _gameOver;
	}

	private GameObject CheckGameOverLines(GameObject[,] Board, bool _minimaxCheck)
	{
		var _cross = 0;
		var _circle = 0;

		//Checking every line for a sequence of the same Piece
		for (int i = 0; i < BoardSize; i++)
		{
			for (int j = 0; j < BoardSize; j++)
			{
				_cross += Board[i,j].gameObject.tag == Cross.gameObject.tag ? 1 : 0;
				_circle += Board[i,j].gameObject.tag == Circle.gameObject.tag ? 1 : 0;	
			}

			if (_cross == BoardSize || _circle == BoardSize)
			{
				if (!_minimaxCheck)
				{
					if (i == 0)
						Instantiate(HorizontalLine, new Vector2(0f, 3f), Quaternion.identity);
					
					if (i == 1)
						Instantiate(HorizontalLine, new Vector2(0f, 0f), Quaternion.identity);
					
					if (i == 2)
						Instantiate(HorizontalLine, new Vector2(0f, -3f), Quaternion.identity);
				}						
			}

			if (_cross == BoardSize)
				return Cross;

			if (_circle == BoardSize)
				return Circle;

			_cross = 0;
			_circle = 0;
		}

		return EmptyCell;
	}

	private GameObject CheckGameOverColumns(GameObject[,] Board, bool _minimaxCheck)
	{
		var _cross = 0;
		var _circle = 0;

		//Checking every column for a sequence of the same Piece
		for (int j = 0; j < BoardSize; j++)
		{
			for (int i = 0; i < BoardSize; i++)
			{
				_cross += Board[i,j].gameObject.tag == Cross.gameObject.tag ? 1 : 0;
				_circle += Board[i,j].gameObject.tag == Circle.gameObject.tag ? 1 : 0;				
			}

			if (_cross == BoardSize || _circle == BoardSize)
			{
				if (!_minimaxCheck)
				{
					if (j == 0)
						Instantiate(VerticalLine, new Vector2(-3f, 0f), Quaternion.identity);
					
					if (j == 1)
						Instantiate(VerticalLine, new Vector2(0f, 0f), Quaternion.identity);
					
					if (j == 2)
						Instantiate(VerticalLine, new Vector2(3f, 0f), Quaternion.identity);
				}						
			}

			if (_cross == BoardSize)
				return Cross;

			if (_circle == BoardSize)
				return Circle;			
			

			_cross = 0;
			_circle = 0;
		}	

		return EmptyCell;
	}	

	private GameObject CheckGameOverDiagLeft(GameObject[,] Board, bool _minimaxCheck)
	{
		var _cross = 0;
		var _circle = 0;
		
		//Checking left to right diagonal for a sequence of the same Piece
		for (int i = 0; i < BoardSize; i++)
		{
				_cross += Board[i,i].gameObject.tag == Cross.gameObject.tag ? 1 : 0;
				_circle += Board[i,i].gameObject.tag == Circle.gameObject.tag ? 1 : 0;				
		}

		if (_cross == BoardSize || _circle == BoardSize)
		{
			if (!_minimaxCheck)
				Instantiate(DiagonalLeftLine, new Vector2(0f, 0f), Quaternion.identity);
		}		

		if (_cross == BoardSize)
			return Cross;

		if (_circle == BoardSize)
			return Circle;

		return EmptyCell;
	}

	private GameObject CheckGameOverDiagRight(GameObject[,] Board, bool _minimaxCheck)
	{
		var _cross = 0;
		var _circle = 0;
		var j = BoardSize - 1;

		//Checking right to left diagonal
		for (int i = 0; i < BoardSize; i++)
		{
				_cross += Board[i,j].gameObject.tag == Cross.gameObject.tag ? 1 : 0;
				_circle += Board[i,j].gameObject.tag == Circle.gameObject.tag ? 1 : 0;
				j--;		
		}

		if (_cross == BoardSize || _circle == BoardSize)
		{
			if (!_minimaxCheck)
				Instantiate(DiagonalRightLine, new Vector2(0f, 0f), Quaternion.identity);
		}

		if (_cross == BoardSize)
			return Cross;		

		if (_circle == BoardSize)
			return Circle;			

		return EmptyCell;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour, IMoveable
{
    int _oppositeId;
    int _id;


    private void Start()
    {
        if (gameObject.CompareTag("Black"))
        {
            _id = 2;
            _oppositeId = 1;
            BoardManager.instance.remainingBlackPiece.Add(gameObject);
        }
        else if (gameObject.CompareTag("White"))
        {
            _id = 1;
            _oppositeId = 2;
            BoardManager.instance.remainingWhitePiece.Add(gameObject);
        }
        BoardManager.instance.allPiece.Add(new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)), gameObject);
        BoardManager.instance.grid[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)] = _id;
    }

    private void OnDisable()
    {
        if (gameObject.CompareTag("Black"))
        {
            BoardManager.instance.remainingBlackPiece.Remove(gameObject);
        }
        else if (gameObject.CompareTag("White"))
        {
            BoardManager.instance.remainingWhitePiece.Remove(gameObject);
        }
    }

    public List<Vector2> CheckValidBoxToMove()
    {
        List<Vector2> _validPositionList = new();
        int _xPosition = Mathf.RoundToInt(transform.position.x);
        int _yPosition = Mathf.RoundToInt(transform.position.y);

        if (gameObject.CompareTag("Black"))
        {
            //move
            if (_yPosition == 6)
            {
                for (int i = -1; i >= -2; i--)
                {
                    if (BoardManager.instance.grid[_xPosition, _yPosition + i] == 0)
                    {
                        _validPositionList.Add(new Vector2(_xPosition, _yPosition + i));
                    }
                    else break;
                }
            }
            else
            {
                if (BoardManager.instance.grid[_xPosition, _yPosition - 1] == 0)
                {
                    _validPositionList.Add(new Vector2(_xPosition, _yPosition - 1));
                }
            }

            //knockout opposite
            if (_xPosition - 1 >= 0)
            {
                if (BoardManager.instance.grid[_xPosition - 1, _yPosition - 1] == _oppositeId)
                {
                    _validPositionList.Add(new Vector2(_xPosition - 1, _yPosition - 1));
                }
            }
            if (_xPosition + 1 < 8)
            {
                if (BoardManager.instance.grid[_xPosition + 1, _yPosition - 1] == _oppositeId)
                {
                    _validPositionList.Add(new Vector2(_xPosition + 1, _yPosition - 1));
                }
            }

            return _validPositionList;
        }

        if (gameObject.CompareTag("White"))
        {
            //move
            if (_yPosition == 1)
            {
                for (int i = 1; i <= 2; i++)
                {
                    if (BoardManager.instance.grid[_xPosition, _yPosition + i] == 0)
                    {
                        _validPositionList.Add(new Vector2(_xPosition, _yPosition + i));
                    }
                    else break;
                }
            }
            else
            {
                if (BoardManager.instance.grid[_xPosition, _yPosition + 1] == 0)
                {
                    _validPositionList.Add(new Vector2(_xPosition, _yPosition + 1));
                }
            }

            //knockout opposite
            if (_xPosition - 1 >= 0)
            {
                if (BoardManager.instance.grid[_xPosition - 1, _yPosition + 1] == _oppositeId)
                {
                    _validPositionList.Add(new Vector2(_xPosition - 1, _yPosition + 1));
                }
            }
            if (_xPosition + 1 < 8)
            {
                if (BoardManager.instance.grid[_xPosition + 1, _yPosition + 1] == _oppositeId)
                {
                    _validPositionList.Add(new Vector2(_xPosition + 1, _yPosition + 1));
                }
            }

            return _validPositionList;
        }




        return _validPositionList;
    }

    public int DeletePositionToMove()
    {
        BoardManager.instance.grid[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)] = 0;
        return _id;
    }

    public int GetOppositeId()
    {
        return _oppositeId;
    }
}
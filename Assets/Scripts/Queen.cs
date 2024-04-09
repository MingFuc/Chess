using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour, IMoveable
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

    public List<Vector2> CheckValidBoxToMove()
    {
        List<Vector2> _validPositionList = new();
        int _xPosition = Mathf.RoundToInt(transform.position.x);
        int _yPosition = Mathf.RoundToInt(transform.position.y);

        #region ------CheckVertically------
        for (int i = 1; i < 8; i++)
        {
            if (_yPosition + i > 7 || _yPosition + i < 0) break;

            if (BoardManager.instance.grid[_xPosition, _yPosition + i] == 0)
            {
                _validPositionList.Add(new Vector2(_xPosition, _yPosition + i));
            }
            else if (BoardManager.instance.grid[_xPosition, _yPosition + i] == _oppositeId)
            {
                _validPositionList.Add(new Vector2(_xPosition, _yPosition + i));
                break;
            }
            else break;
        }

        for (int i = -1; i > -8; i--)
        {
            if (_yPosition + i > 7 || _yPosition + i < 0) break;

            if (BoardManager.instance.grid[_xPosition, _yPosition + i] == 0)
            {
                _validPositionList.Add(new Vector2(_xPosition, _yPosition + i));
            }
            else if (BoardManager.instance.grid[_xPosition, _yPosition + i] == _oppositeId)
            {
                _validPositionList.Add(new Vector2(_xPosition, _yPosition + i));
                break;
            }
            else break;
        }
        #endregion

        #region ------CheckHorizontally------
        for (int i = 1; i < 8; i++)
        {
            if (_xPosition + i > 7 || _xPosition + i < 0) break;

            if (BoardManager.instance.grid[_xPosition + i, _yPosition] == 0)
            {
                _validPositionList.Add(new Vector2(_xPosition + i, _yPosition));
            }
            else if (BoardManager.instance.grid[_xPosition + i, _yPosition] == _oppositeId)
            {
                _validPositionList.Add(new Vector2(_xPosition + i, _yPosition));
                break;
            }
            else break;
        }

        for (int i = -1; i > -8; i--)
        {
            if (_xPosition + i > 7 || _xPosition + i < 0) break;

            if (BoardManager.instance.grid[_xPosition + i, _yPosition] == 0)
            {
                _validPositionList.Add(new Vector2(_xPosition + i, _yPosition));
            }
            else if (BoardManager.instance.grid[_xPosition + i, _yPosition] == _oppositeId)
            {
                _validPositionList.Add(new Vector2(_xPosition + i, _yPosition));
                break;
            }
            else break;
        }
        #endregion

        #region ------Upper right check------
        for (int i = 1; i < 8; i++)
        {
            bool _outerBreak = false;
            for (int j = 1; j < 8; j++)
            {
                if (_xPosition + i > 7 || _xPosition + i < 0 || _yPosition + j > 7 || _yPosition + j < 0) break;
                if (Mathf.Abs(i) != Mathf.Abs(j)) continue;

                if (BoardManager.instance.grid[_xPosition + i, _yPosition + j] == 0)
                {
                    _validPositionList.Add(new Vector2(_xPosition + i, _yPosition + j));
                }
                else if (BoardManager.instance.grid[_xPosition + i, _yPosition + j] == _oppositeId)
                {
                    _validPositionList.Add(new Vector2(_xPosition + i, _yPosition + j));
                    _outerBreak = true;
                    break;
                }
                else
                {
                    _outerBreak = true;
                    break;
                }
            }
            if (_outerBreak) break;
        }
        #endregion

        #region ------Bottom right check------
        for (int i = 1; i < 8; i++)
        {
            bool _outerBreak = false;
            for (int j = -1; j > -8; j--)
            {
                if (_xPosition + i > 7 || _xPosition + i < 0 || _yPosition + j > 7 || _yPosition + j < 0) break;
                if (Mathf.Abs(i) != Mathf.Abs(j)) continue;

                if (BoardManager.instance.grid[_xPosition + i, _yPosition + j] == 0)
                {
                    _validPositionList.Add(new Vector2(_xPosition + i, _yPosition + j));
                }
                else if (BoardManager.instance.grid[_xPosition + i, _yPosition + j] == _oppositeId)
                {
                    _validPositionList.Add(new Vector2(_xPosition + i, _yPosition + j));
                    _outerBreak = true;
                    break;
                }
                else
                {
                    _outerBreak = true;
                    break;
                }
            }
            if (_outerBreak) break;
        }
        #endregion

        #region ------Upper left check------
        for (int i = -1; i > -8; i--)
        {
            bool _outerBreak = false;
            for (int j = 1; j < 8; j++)
            {
                if (_xPosition + i > 7 || _xPosition + i < 0 || _yPosition + j > 7 || _yPosition + j < 0) break;
                if (Mathf.Abs(i) != Mathf.Abs(j)) continue;

                if (BoardManager.instance.grid[_xPosition + i, _yPosition + j] == 0)
                {
                    _validPositionList.Add(new Vector2(_xPosition + i, _yPosition + j));
                }
                else if (BoardManager.instance.grid[_xPosition + i, _yPosition + j] == _oppositeId)
                {
                    _validPositionList.Add(new Vector2(_xPosition + i, _yPosition + j));
                    _outerBreak = true;
                    break;
                }
                else
                {
                    _outerBreak = true;
                    break;
                }
            }
            if (_outerBreak) break;
        }
        #endregion

        #region ------Bottom left check------
        for (int i = -1; i > -8; i--)
        {
            bool _outerBreak = false;
            for (int j = -1; j > -8; j--)
            {
                if (_xPosition + i > 7 || _xPosition + i < 0 || _yPosition + j > 7 || _yPosition + j < 0) break;
                if (Mathf.Abs(i) != Mathf.Abs(j)) continue;

                if (BoardManager.instance.grid[_xPosition + i, _yPosition + j] == 0)
                {
                    _validPositionList.Add(new Vector2(_xPosition + i, _yPosition + j));
                }
                else if (BoardManager.instance.grid[_xPosition + i, _yPosition + j] == _oppositeId)
                {
                    _validPositionList.Add(new Vector2(_xPosition + i, _yPosition + j));
                    _outerBreak = true;
                    break;
                }
                else
                {
                    _outerBreak = true;
                    break;
                }
            }
            if (_outerBreak) break;
        }
        #endregion


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
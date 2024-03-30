using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : MonoBehaviour, IMoveable
{
    int _oppositeId;
    int _id;
    private void Awake()
    {
        if (gameObject.CompareTag("Black"))
        {
            _id = 2;
            _oppositeId = 1;
        }
        else if (gameObject.CompareTag("White"))
        {
            _id = 1;
            _oppositeId = 2;
        }
    }

    private void Start()
    {
        BoardManager.instance.allPiece.Add(new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)), gameObject);
    }

    public List<Vector2> CheckValidBoxToMove()
    {
        List<Vector2> _validPositionList = new();
        int _xPosition = Mathf.RoundToInt(transform.position.x);
        int _yPosition = Mathf.RoundToInt(transform.position.y);

        #region ------Upper right check------
        for (int i = 1; i < 8; i++)
        {
            bool _outerBreak = false;
            for (int j = 1; j < 8; j++)
            {
                if (_xPosition + i > 7|| _xPosition + i < 0 || _yPosition + j > 7 || _yPosition + j < 0) break;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour, IMoveable
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

        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                if (i == 0 || j == 0) continue;
                if (Mathf.Abs(i) == Mathf.Abs(j)) continue;
                if (_xPosition + i > 7 || _xPosition + i < 0 || _yPosition + j > 7 || _yPosition + j < 0) continue;

                if (BoardManager.instance.grid[_xPosition + i, _yPosition + j] == 0 ||
                    BoardManager.instance.grid[_xPosition + i, _yPosition + j] == _oppositeId)
                {
                    _validPositionList.Add(new Vector2(_xPosition + i, _yPosition + j));
                }

            }
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

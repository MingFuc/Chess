using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour, IMoveable
{
    int _oppositeId;

    private void Awake()
    {
        if (gameObject.CompareTag("Black"))
            _oppositeId = 1;
        else if (gameObject.CompareTag("White"))
            _oppositeId = 2;
    }

    public List<Vector2> CheckValidBoxToMove()
    {
        List<Vector2> _validPositionList = new();
        int _xPosition = Mathf.RoundToInt(transform.position.x);
        int _yPosition = Mathf.RoundToInt(transform.position.y);

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
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
}




using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    List<Vector2> CheckValidBoxToMove();

    int DeletePositionToMove();

    int GetOppositeId();
}

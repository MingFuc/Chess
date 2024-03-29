using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    public int[,] grid = new int[8, 8];

    public List<Vector2> validPosition = new();


    [SerializeField] GameObject _validSign;

    public List<GameObject> validSign = new();



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            grid[i, 0] = 1;
            grid[i, 1] = 1;

            grid[i, 7] = 2;
            grid[i, 6] = 2;

            for (int j = 2; j < 6; j++)
            {
                grid[i, j] = 0;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D _hitInfo = Physics2D.Raycast(_mousePosition, Vector2.zero);

            if (_hitInfo.collider != null)
            {
                //add valid box
                validPosition.Clear();
                IMoveable _moveable = _hitInfo.collider.gameObject.GetComponent<IMoveable>();
                validPosition.AddRange(_moveable.CheckValidBoxToMove());

                //spawn sign
                foreach(var _validSign in validSign)
                {
                    Destroy(_validSign);
                }

                validSign.Clear();

                foreach(var _validPosition in validPosition)
                {
                    validSign.Add(Instantiate(_validSign, _validPosition, Quaternion.identity));
                }
            }
        }
    }

}

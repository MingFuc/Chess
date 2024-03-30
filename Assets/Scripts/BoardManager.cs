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

    [SerializeField] GameObject temporaryPiece;

    public Dictionary<Vector2, GameObject> allPiece = new();

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
        //grid[6,2] = 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D _hitInfo = Physics2D.Raycast(_mousePosition, Vector2.zero);

            if (_hitInfo.collider != null)
            {

                if (_hitInfo.collider.gameObject.CompareTag("White") || _hitInfo.collider.gameObject.CompareTag("Black"))
                {

                    temporaryPiece = _hitInfo.collider.gameObject;

                    //add valid box
                    validPosition.Clear();
                    IMoveable _checkValidBoxToMove = _hitInfo.collider.gameObject.GetComponent<IMoveable>();
                    validPosition.AddRange(_checkValidBoxToMove.CheckValidBoxToMove());

                    //spawn sign
                    foreach (var _validSign in validSign)
                    {
                        Destroy(_validSign);
                    }

                    validSign.Clear();

                    foreach (var _validPosition in validPosition)
                    {
                        validSign.Add(Instantiate(_validSign, _validPosition, Quaternion.identity));
                    }
                }

                if (validSign.Contains(_hitInfo.collider.gameObject))
                {
                    int _oldXPosition = Mathf.RoundToInt(temporaryPiece.transform.position.x);
                    int _oldYPosition = Mathf.RoundToInt(temporaryPiece.transform.position.y);

                    int _newXPosition = Mathf.RoundToInt(_hitInfo.collider.gameObject.transform.position.x);
                    int _newYPosition = Mathf.RoundToInt(_hitInfo.collider.gameObject.transform.position.y);


                    IMoveable _deletePositionToMove = temporaryPiece.GetComponent<IMoveable>();
                    if(grid[_newXPosition, _newYPosition] == _deletePositionToMove.GetOppositeId())
                    {
                        Destroy(allPiece[new Vector2(_newXPosition, _newYPosition)]); //destroy old piece
                        allPiece.Remove(new Vector2(_newXPosition, _newYPosition));
                    }
                    grid[_newXPosition, _newYPosition] = _deletePositionToMove.DeletePositionToMove();

                    allPiece.Remove(new Vector2(_oldXPosition, _oldYPosition));
                    temporaryPiece.transform.position = _hitInfo.collider.gameObject.transform.position;
                    allPiece.Add(new Vector2(_newXPosition, _newYPosition), temporaryPiece);

                    //clear data after move
                    foreach (var _validSign in validSign)
                    {

                        Destroy(_validSign);
                    }

                    validSign.Clear();

                    validPosition.Clear();

                    temporaryPiece = null;
                }
            }
        }
    }

}

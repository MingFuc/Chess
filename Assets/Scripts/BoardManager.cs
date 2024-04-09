using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
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

    private bool _whiteTurn = true;
    private bool _blackTurn = false;
    private bool _botTurn = false;

    private bool _isGameOver = false;

    public List<GameObject> remainingWhitePiece = new();
    public List<GameObject> remainingBlackPiece = new();

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 2; j < 6; j++)
            {
                grid[i, j] = 0;
            }
        }
    }

    private void Start()
    {
        //for (int i = 0; i < 8; i++)
        //{
        //    grid[i, 0] = 1;
        //    grid[i, 1] = 1;

        //    grid[i, 7] = 2;
        //    grid[i, 6] = 2;

        //    for (int j = 2; j < 6; j++)
        //    {
        //        grid[i, j] = 0;
        //    }
        //}
    }

    private void Update()
    {
        if (_isGameOver == false)
        {
            WhiteTurn();
            //BlackTurn();
            BotTurn_Black();
        }
    }


    void WhiteTurn()
    {
        if (_whiteTurn == false) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D _hitInfo = Physics2D.Raycast(_mousePosition, Vector2.zero);

            if (_hitInfo.collider != null)
            {

                if (_hitInfo.collider.gameObject.CompareTag("White"))
                {

                    CheckPlayerInputAndPrepareBeforeMoving(_hitInfo.collider.gameObject);

                }

                if (validSign.Contains(_hitInfo.collider.gameObject))
                {

                    MoveToValidBoxAndDoSomeLogic(_hitInfo.collider.gameObject);

                    _whiteTurn = !_whiteTurn;
                    //_blackTurn = !_blackTurn;
                    _botTurn = !_botTurn;
                }
            }
        }
    }


    void BlackTurn()
    {
        if (_blackTurn == false) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D _hitInfo = Physics2D.Raycast(_mousePosition, Vector2.zero);

            if (_hitInfo.collider != null)
            {

                if (_hitInfo.collider.gameObject.CompareTag("Black"))
                {

                    CheckPlayerInputAndPrepareBeforeMoving(_hitInfo.collider.gameObject);

                }

                if (validSign.Contains(_hitInfo.collider.gameObject))
                {

                    MoveToValidBoxAndDoSomeLogic(_hitInfo.collider.gameObject);

                    _blackTurn = !_blackTurn;
                    _whiteTurn = !_whiteTurn;
                }
            }
        }
    }

    void CheckPlayerInputAndPrepareBeforeMoving(GameObject gob)
    {
        temporaryPiece = gob;

        //add valid box
        validPosition.Clear();
        IMoveable _checkValidBoxToMove = gob.GetComponent<IMoveable>();
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

    void MoveToValidBoxAndDoSomeLogic(GameObject gob)
    {
        int _oldXPosition = Mathf.RoundToInt(temporaryPiece.transform.position.x);
        int _oldYPosition = Mathf.RoundToInt(temporaryPiece.transform.position.y);

        int _newXPosition = Mathf.RoundToInt(gob.transform.position.x);
        int _newYPosition = Mathf.RoundToInt(gob.transform.position.y);


        IMoveable _deletePositionToMove = temporaryPiece.GetComponent<IMoveable>();
        if (grid[_newXPosition, _newYPosition] == _deletePositionToMove.GetOppositeId())
        {
            //Destroy(allPiece[new Vector2(_newXPosition, _newYPosition)]); //destroy old piece
            allPiece[new Vector2(_newXPosition, _newYPosition)].SetActive(false);
            allPiece.Remove(new Vector2(_newXPosition, _newYPosition));
        }
        grid[_newXPosition, _newYPosition] = _deletePositionToMove.DeletePositionToMove();

        allPiece.Remove(new Vector2(_oldXPosition, _oldYPosition));
        temporaryPiece.transform.position = gob.transform.position;
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

    void BotTurn_Black()
    {
        if (_botTurn == false) return;
        do
        {
            temporaryPiece = remainingBlackPiece[Random.Range(0, remainingBlackPiece.Count)];
            IMoveable _checkValidBoxToMove = temporaryPiece.GetComponent<IMoveable>();

            validPosition.Clear();
            validPosition.AddRange(_checkValidBoxToMove.CheckValidBoxToMove());

            if (validPosition.Count == 0)
            {
                Debug.Log($"Chose {temporaryPiece.name} but this piece cant move, wait bot select again");
            }
        } while (validPosition.Count == 0);

        GameObject _temporaryGameobjectMoveTo = new GameObject("_temporaryGameobject");
        _temporaryGameobjectMoveTo.transform.position = validPosition[Random.Range(0, validPosition.Count)];

        MoveToValidBoxAndDoSomeLogic(_temporaryGameobjectMoveTo);

        _botTurn = !_botTurn;
        _whiteTurn = !_whiteTurn;

    }

    public void GameOverMessage(string _blackOrWhite)
    {
        _isGameOver = true;
        print($"{_blackOrWhite} win");
    }

}

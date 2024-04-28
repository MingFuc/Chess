using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour, ISerializationCallbackReceiver
{
    public static BoardManager instance;

    public int[,] grid = new int[8, 8];

    public List<Vector2> validPosition = new();

    [SerializeField]
    private GameObject temporaryPiece;

    public Dictionary<Vector2, GameObject> allPiece = new();

    private bool _whiteTurn = true;
    private bool _blackTurn = false;
    private bool _botTurn = false;

    private bool _isGameOver = false;

    public List<GameObject> remainingWhitePiece = new();
    public List<GameObject> remainingBlackPiece = new();


    [Header("------MOVE SIGNAL------")]
    [SerializeField]
    private GameObject _validSign;

    public List<GameObject> validSign = new();

    [SerializeField]
    private GameObject ImageOfStartPosition;

    [SerializeField]
    private GameObject ImageOfEndPosition;

    private GameObject _imageOfStartPosition;
    private GameObject _imageOfEndPosition;


    [Header("------SOUND------")]
    [SerializeField]
    private AudioClip moveSound;
    private AudioSource moveSource;


    [Header("------BUTTON------")]
    [SerializeField]
    private GameObject whiteOption;
    [SerializeField]
    private GameObject blackOption;

    [Header("------BOTCHOICE------")]
    [SerializeField]
    private GameObject[] fourChoice = new GameObject[7];


    [Header("------WINNING MESSAGE------")]
    public GameObject messageArea;

    private bool isUpgradeing = false;
    private bool _playWithBot;


    #region serialize grid

    [SerializeField] private List<Package<int>> serializedGrid;

    [System.Serializable]
    struct Package<TElement>
    {

        public int Index1;
        public int Index2;
        public TElement Element;
        public Package(int index1, int index2, TElement element)
        {
            Index1 = index1;
            Index2 = index2;
            Element = element;
        }
    }
    public void OnBeforeSerialize()
    {
        serializedGrid = new List<Package<int>>();
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                serializedGrid.Add(new Package<int>(i, j, grid[i, j]));
            }
        }
    }

    public void OnAfterDeserialize()
    {
        foreach (var package in serializedGrid)
        {
            grid[package.Index1, package.Index2] = package.Element;
        }
    }





    #endregion


    private void Awake()
    {
        instance = this;

        _playWithBot = SceneController.instance.playWithBot;

        moveSource = GetComponent<AudioSource>();
        moveSource.clip = moveSound;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 2; j < 6; j++)
            {
                grid[i, j] = 0;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        WhiteTurn();
        BlackTurn(_playWithBot);
        BotTurn_Black(_playWithBot);
    }


    async void WhiteTurn()
    {
        if (_isGameOver == true) return;
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

                    if (IsKingDead(remainingBlackPiece))
                    {
                        GameOverMessage("White");
                    }

                    if (temporaryPiece.name.Contains("pawn") && temporaryPiece.transform.position.y > 6)
                    {
                        isUpgradeing = true;
                        await UpgradingPawn(whiteOption);
                    }

                    temporaryPiece = null;
                    _blackTurn = !_blackTurn;
                    _botTurn = !_botTurn;
                }
            }
        }
    }


    async void BlackTurn(bool _playWithBot)
    {
        if (_playWithBot) return;
        if (_isGameOver == true) return;
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

                    if (IsKingDead(remainingWhitePiece))
                    {
                        GameOverMessage("Black");
                    }

                    if (temporaryPiece.name.Contains("pawn") && temporaryPiece.transform.position.y < 1)
                    {
                        isUpgradeing = true;
                        await UpgradingPawn(blackOption);
                    }

                    temporaryPiece = null;
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
        Destroy(_imageOfStartPosition);
        Destroy(_imageOfEndPosition);


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

        //move signal
        _imageOfStartPosition = Instantiate(ImageOfStartPosition, new Vector3(_oldXPosition, _oldYPosition), Quaternion.identity);
        _imageOfEndPosition = Instantiate(ImageOfEndPosition, new Vector3(_newXPosition, _newYPosition), Quaternion.identity);

        //Debug.Log($"{temporaryPiece.name} move from {_oldXPosition},{_oldYPosition} to {_newXPosition},{_newYPosition}");
        //Debug.Log($"{_oldXPosition},{_oldYPosition} --> {grid[_oldXPosition, _oldYPosition]}, {_newXPosition},{_newYPosition} --> {grid[_newXPosition, _newYPosition]}");

        //sound 
        moveSource.Play();

        //clear data after move
        foreach (var _validSign in validSign)
        {

            Destroy(_validSign);
        }

        validSign.Clear();

        validPosition.Clear();



    }

    void BotTurn_Black(bool _playWithBot)
    {
        if (!_playWithBot) return;
        if (_isGameOver == true) return;
        if (_botTurn == false) return;

        do
        {
            temporaryPiece = remainingBlackPiece[UnityEngine.Random.Range(0, remainingBlackPiece.Count)];
            IMoveable _checkValidBoxToMove = temporaryPiece.GetComponent<IMoveable>();

            validPosition.Clear();
            validPosition.AddRange(_checkValidBoxToMove.CheckValidBoxToMove());

            //if (validPosition.Count == 0)
            //{
            //    Debug.Log($"Chose {temporaryPiece.name} but this piece cant move, wait bot select again");
            //}
        } while (validPosition.Count == 0);

        GameObject _temporaryGameobjectMoveTo = new GameObject("_temporaryGameobject");
        _temporaryGameobjectMoveTo.transform.position = validPosition[UnityEngine.Random.Range(0, validPosition.Count)];

        MoveToValidBoxAndDoSomeLogic(_temporaryGameobjectMoveTo);

        if (IsKingDead(remainingWhitePiece))
        {
            GameOverMessage("Black");
        }

        if (temporaryPiece.name.Contains("pawn") && temporaryPiece.transform.position.y < 1)
        {
            ReplacePawn(fourChoice[UnityEngine.Random.Range(0, fourChoice.Length)]);
        }

        _botTurn = !_botTurn;
        _whiteTurn = !_whiteTurn;

    }

    public void GameOverMessage(string _blackOrWhite)
    {
        _isGameOver = true;

        messageArea.SetActive(true);
        TextMeshProUGUI message = messageArea.GetComponentInChildren<TextMeshProUGUI>();
        message.text = $"{_blackOrWhite} win !!!";
    }

    async Task UpgradingPawn(GameObject button)
    {

        button.SetActive(true);
        do
        {
            await Task.Yield();
        } while (isUpgradeing == true);

    }

    public void ReplacePawn(GameObject gob)
    {

        int _XPosition = Mathf.RoundToInt(temporaryPiece.transform.position.x);
        int _YPosition = Mathf.RoundToInt(temporaryPiece.transform.position.y);

        temporaryPiece.SetActive(false);
        temporaryPiece = Instantiate(gob, new Vector3(_XPosition, _YPosition), Quaternion.identity);

        isUpgradeing = false;

    }
    public void TurnOffSelectOption(GameObject gob)
    {
        gob.SetActive(false);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }

    bool IsKingDead(List<GameObject> list)
    {
        foreach (var item in list)
        {
            if (item.name.Contains("king"))
            {
                return false;
            }
        }

        return true;
    }
}

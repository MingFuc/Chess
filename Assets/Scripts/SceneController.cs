using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public bool playWithBot;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnePlayer()
    {
        playWithBot = true;
        SceneManager.LoadScene(1);
    }

    public void TwoPlayer()
    {
        playWithBot = false;
        SceneManager.LoadScene(1);
    }

}

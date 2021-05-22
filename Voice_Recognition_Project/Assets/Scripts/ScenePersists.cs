using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersists : MonoBehaviour
{
    private int startUpScene = 0;
    private void Awake()
    {
        int nunScenePersists = FindObjectsOfType<ScenePersists>().Length;
        if(nunScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        startUpScene = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if(currentScene != startUpScene)
        {
            Destroy(gameObject);
        }
    }
}

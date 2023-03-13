using System;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.SceneManagement;
 
// by @kurtdekker - to make a Unity singleton that has some
// prefab-stored, data associated with it, eg a music manager
//
// To use: access with SingletonViaPrefab.Instance
//
// To set up:
//  - Copy this file (duplicate it)
//  - rename class SingletonViaPrefab to your own classname
//  - rename CS file too
//  - create the prefab asset associated with this singleton
//      NOTE: read docs on Resources.Load() for where it must exist!!
//
// DO NOT DRAG THE PREFAB INTO A SCENE! THIS CODE AUTO-INSTANTIATES IT!
//
// I do not recommend subclassing unless you really know what you're doing.
 
public class GameManager : MonoBehaviour
{
    private  int counter;
    // This is really the only blurb of code you need to implement a Unity singleton
    private static GameManager _Instance;
    public static GameManager Instance
    {
        get
        {
            if (!_Instance)
            {
                // NOTE: read docs to see directory requirements for Resources.Load!
                var prefab = Resources.Load<GameObject>("GameManager");
                // create the prefab in your scene
                var inScene = Instantiate<GameObject>(prefab);
                // try find the instance inside the prefab
                _Instance = inScene.GetComponentInChildren<GameManager>();
                // guess there isn't one, add one
                if (!_Instance) _Instance = inScene.AddComponent<GameManager>();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.transform.root.gameObject);
            }
            return _Instance;
        }
    }
 
    // implement your Awake, Start, Update, or other methods here...

    public void OnGoal()
    {
        Debug.Log("GOAL REACHED!");
        LoadNextLevel();
    }

    public void StartGame()
    {
        Debug.Log("Start game");
        LoadNextLevel();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Cursor.visible = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
    }

    public void LoadNextLevel()
    {
        var current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current + 1);
        //Cursor.visible = true;
    }
}

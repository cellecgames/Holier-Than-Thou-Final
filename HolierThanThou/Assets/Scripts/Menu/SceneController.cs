﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private int levelBuildIndexStart;
    [SerializeField] private int SPECIFIC_LEVEL = 2;
    [SerializeField] private bool randomize = true;
    [SerializeField] private string seed;
    private List<int> scenes = new List<int>();
    private Deck<int> deckOfScenes = new Deck<int>();
    private static bool isInitialized = false;

    void Awake()
    {
        if (!isInitialized)
        {
            SceneManager.activeSceneChanged += ChangedActiveScene;
            isInitialized = true;
#if UNITY_EDITOR
            int intSeed = seed != null ? (int)System.DateTime.Now.Ticks : System.Convert.ToInt32(seed);
            deckOfScenes.Seed(intSeed);
#endif
            for (int i = levelBuildIndexStart; i < SceneManager.sceneCountInBuildSettings - 1; i++)
            {
                deckOfScenes.Add(i, false);
            }
            deckOfScenes.Shuffle();
        }
    }

    private void ResetMenus()
    {
        //If the current scene is the Main Menu
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            //turn on the Main Menu.
            MainMenu[] Canvas = Resources.FindObjectsOfTypeAll<MainMenu>();
            Canvas[0].ResetNameText();
            Canvas[0].gameObject.SetActive(true);
        }
        //else
        //{
        //	//turn off the Main Menu.
        //	transform.GetChild(0).gameObject.SetActive(false);
        //}
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        if (scenes.Count > 0)
        {
            //If we're going to the main menu from a level, just reset.
            //TODO figure out a better way of doing this.
            if (next.buildIndex == 0 && scenes[scenes.Count - 1] > levelBuildIndexStart)
            {
                scenes.Clear();
                scenes.Add(SceneManager.GetActiveScene().buildIndex);
            }
            if (scenes.Count > 0)
            {
                //if the scene being changed to is not the current scene (i.e., if you're restarting a level)
                if (!(scenes[scenes.Count - 1] == SceneManager.GetActiveScene().buildIndex))
                {
                    //add it to the list of scenes.
                    scenes.Add(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
        else
        {
            scenes.Add(SceneManager.GetActiveScene().buildIndex);
        }
#if DEBUG
#if UNITY_EDITOR
        DestroyCompetingAudioManagerScript();
#endif
#endif
        ResetMenus();
    }

    private void DestroyCompetingAudioManagerScript()
    {
        AudioManager[] AMs = FindObjectsOfType<AudioManager>();
        foreach (AudioManager audioManager in AMs)
        {
            if (audioManager.gameObject.GetComponent<DoNotDestroy>() == null)
            {
                Destroy(audioManager.gameObject);
            }
        }
    }

    public void GoToPreviousScene()
    {
        //When you're going back a scene, remove the current last element of the scenes list.
        if (scenes.Count > 1)
        {
            //Remove the last two entries, since the previous scene will be added to the scenes list in the Start() function.
            scenes.RemoveAt(scenes.Count - 1);
            scenes.RemoveAt(scenes.Count - 1);
        }
        //If there are at least two scenes, you can go back one scene.
        if (scenes.Count >= 2)
        {
            //scenes.Count - 1 is the index for the current scene.
            //scenes.Count will always at least be 1, because it is incremented in the Start() method.
            SceneManager.LoadSceneAsync(scenes[scenes.Count - 2]);
        }
        // if there is only one scene, it SHOULD be the main scene, but this will load the main menu in any other case.
        else
        {
            SceneManager.LoadSceneAsync(0);
        }
    }


       
        public void GoToScene()
        {
            if (randomize)
            {
            SceneManager.LoadSceneAsync(8);
            SceneManager.LoadSceneAsync(deckOfScenes.Draw());
        }
            else
            {
                
                SceneManager.LoadSceneAsync(SPECIFIC_LEVEL);
            }
        }

        public void GoToScene(int buildIndex)
        {
            SceneManager.LoadSceneAsync(buildIndex);
        }

        public void GoToScene(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }

        public void GoToNextScene()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
    
}

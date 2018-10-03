using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int score;
    public static GameManager instance;
    public int highScore = 0;
    public int currentLevel = 1;
    public int highestLevel = 2;
    HudManager hudManager;

    private void Awake()
    {
        print("GameManager Awake");
        if (instance == null)
        {
            print("GameManager Awake, instance null");
            instance = this;
        }
        else if (instance != this)
        {
            // An alternative method to OnLevelFinishedLoading fix to wrong hudMananger on level load
            // new levels hudManager is correct one so assign it to the instance's hudManager (which is referring to the previous level's hudManager)
            // instance.hudManager = FindObjectOfType<HudManager>();

            print("GameManager Awake, instance non null");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    // https://academy.zenva.com/course/master-unity-game-development-ultimate-beginners-course/#comment-1541588
    // https://answers.unity.com/questions/1174255/since-onlevelwasloaded-is-deprecated-in-540b15-wha.html
    // Fix for hudManager's linked canvas text being null on death or next level
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        hudManager = FindObjectOfType<HudManager>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        if (hudManager != null)
        {
            hudManager.ResetHud();
        }
        else
        {
            print("hudManager is null");
        }
            
        print("new score: " + score);

        if (score > highScore)
        {
            highScore = score;
            print("New high score! " + highScore);
        }
    }

    public void ResetGame()
    {
        score = 0;
        if (hudManager != null)
            hudManager.ResetHud();
        currentLevel = 1;
        SceneManager.LoadScene("Level1");
    }

    public void IncreaseLevel()
    {
        if (currentLevel < highestLevel)
        {
            currentLevel++;
        }
        else
        {
            currentLevel = 1;
        }
        SceneManager.LoadScene("Level" + currentLevel);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");

    }
}

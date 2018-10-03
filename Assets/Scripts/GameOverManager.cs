using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

    public Text scoreValue;
    public Text highScoreValue;

	// Use this for initialization
	void Start () {
        scoreValue.text = GameManager.instance.score.ToString();
        highScoreValue.text = GameManager.instance.highScore.ToString();
    }
	
	public void RestartGame()
    {
        GameManager.instance.ResetGame();
        SceneManager.LoadScene("Level1");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour {

    public Text scoreLabel;

	// Use this for initialization
	void Start () {
        print("HudManager Start");
        ResetHud();
	}

    public void ResetHud()
    {
        scoreLabel.text = "Score: " + GameManager.instance.score;
    }
	
}

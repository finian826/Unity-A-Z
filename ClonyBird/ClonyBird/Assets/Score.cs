using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	static int score = 0;
	static int highScore=0;

	static Score instance;

	public static void AddScore(){
		if (instance.bird.dead)
			return;
		score++;

		if (score > highScore)
			highScore = score;
	}

	BirdMovement bird;

	void Start(){
		instance = this;
		GameObject player_go = GameObject.FindGameObjectWithTag("Player");
		bird = player_go.GetComponent<BirdMovement>();
		score = 0;
		PlayerPrefs.GetInt ("highScore", 0);
	}

	void OnDestroy(){
		instance = null;
		PlayerPrefs.SetInt ("highScore", highScore);
	}


	// Update is called once per frame
	void Update () {
		guiText.text = score+"\n"+highScore;
	}
}

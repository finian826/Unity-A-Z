using UnityEngine;
using System.Collections;

public class ScorePoint : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "Player") {
			Score.AddScore();
		}
	}
}

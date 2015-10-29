using UnityEngine;
using System.Collections;

public class StartScreenScript : MonoBehaviour {
	
	static bool sawOnce = false;
	bool started = false;
	
	// Use this for initialization
	void Start () {
		if(!sawOnce) {
			GetComponent<SpriteRenderer>().enabled = true;
			Time.timeScale = 0;
			sawOnce = true;
			started=true;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeScale==0 && started &&(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) ) {
			Time.timeScale = 1;
			GetComponent<SpriteRenderer>().enabled = false;
			started=false;
		}
	}
}

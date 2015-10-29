using UnityEngine;
using System.Collections;

public class BGLooperScript : MonoBehaviour {

	int numBGPanels = 6;
	float pipeMax= 0.01f;
	float pipeMin= -0.59f;


	// Use this for initialization
	void Start(){
		GameObject[] pipes = GameObject.FindGameObjectsWithTag ("Pipes");

		foreach (GameObject pipe in pipes) {
			Vector3 pos = pipe.transform.position;			
			pos.y= Random.Range(pipeMin, pipeMax);
			pipe.transform.position = pos;
		}
	}


	void OnTriggerEnter2D(Collider2D collider){
		Debug.Log("Triggered: "+ collider.name);
		float widthOfBGObject = ((BoxCollider2D)collider).size.x;
		Vector3 pos = collider.transform.position;

		pos.x += numBGPanels * widthOfBGObject;

		if (collider.tag == "Pipes") {
			pos.y= Random.Range(pipeMin, pipeMax);
		}

		collider.transform.position = pos;
	}


}

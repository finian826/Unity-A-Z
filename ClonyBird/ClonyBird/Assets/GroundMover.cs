using UnityEngine;
using System.Collections;

public class GroundMover : MonoBehaviour {
	Rigidbody2D player;

	// Use this for initialization
	void Start () {
		
		GameObject player_go = GameObject.FindGameObjectWithTag ("Player");
		
		if (player_go == null) {
			Debug.LogError("Can't find object with tag 'Player'!");
			return;
		}
		
		player = player_go.rigidbody2D;
	}

	void FixedUpdate () {
		float vel = player.velocity.x * 0.75f;
		transform.position = transform.position + Vector3.right * vel * Time.deltaTime;

	}
}

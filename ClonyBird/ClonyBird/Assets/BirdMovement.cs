using UnityEngine;
using System.Collections;

public class BirdMovement : MonoBehaviour {

	//Vector3 velocity = Vector3.zero;
	bool didFlap = false;
	public float flapSpeed = 175f; 
	public float forwardSpeed = 1.2f;
	Animator animator;
	public bool dead=false;
	float deathCooldown;
	public bool godMode = false;
	public Texture pauseButton;
	public Texture restart;
	bool paused=false;


	// Use this for initialization
	void Start () {
		animator = GetComponentInChildren<Animator>();
	
	}

	//Do graphics & input updates here
	void Update(){
		if (dead) {
			deathCooldown-= Time.deltaTime;
			if(Input.GetKeyDown (KeyCode.Space) || Input.GetMouseButtonDown(0)){
				Application.LoadLevel(Application.loadedLevel);
			}

		}
		if (!paused && (Input.GetKeyDown (KeyCode.Space) || Input.GetMouseButtonDown(0))) {
			didFlap = true;
		}
	}
	
	// Do phyics engine update here
	void FixedUpdate () {
		if (dead)
			return;

		if(!paused)
			rigidbody2D.AddForce (Vector2.right * forwardSpeed);

		if (didFlap && !paused) {
			GetComponentInChildren<ParticleSystem>().Play();
			GetComponent<AudioSource>().Play();
			rigidbody2D.AddForce (Vector2.up * flapSpeed);
			animator.SetTrigger("DoFlap");
			didFlap = false;
		}

		if (rigidbody2D.velocity.y > 0)
			transform.rotation = Quaternion.Euler (0, 0, 10);
		else {
			float angle = Mathf.Lerp (0, -90, (-rigidbody2D.velocity.y / 3f));
			transform.rotation= Quaternion.Euler(0,0,angle);
		}


	}

	void OnCollisionEnter2D(Collision2D collision){
		if (godMode)
			return;
		if (collision.gameObject.tag != "Top") {
			animator.SetTrigger ("Death");
			dead = true;
			deathCooldown = 0.5f;
		}
	}

	
	void OnGUI(){
		if (GUI.Button (new Rect (Screen.width * 0.8f, Screen.height * 0.025f, Screen.width * 0.5f, Screen.height * 0.1f), restart, "")) {
			Application.LoadLevel("wone");
		}

		if (!paused) {
			if (GUI.Button (new Rect (Screen.width * 0.55f, Screen.height * 0.025f, Screen.width * 0.5f, Screen.height * 0.1f), pauseButton, "")) {
				Time.timeScale = 0.0f;
				paused = true;
			}
		}
		else{
			if (GUI.Button (new Rect (Screen.width * 0.55f, Screen.height * 0.025f, Screen.width * 0.5f, Screen.height * 0.1f), pauseButton, "")) {
				Time.timeScale = 1.0f;
				paused = false;
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class DragDropMatchingItem : MonoBehaviour
{
		private bool startDraging;//start draging flag
		private bool clickedOnDest;//clicked on a destination flag
		private GameObject currentMatchingItem;//current matching item
		private bool matchDone;//match done flag
		private Vector3 clickPoint;//click point
		private Vector3 offset;//offset vector
		public AudioClip youWin;
		public AudioSource audioSource;//audio source
		public Animator youWinAnimator;//you win animator
	 
		void Start ()
		{
				if (audioSource == null) {
						audioSource = Camera.main.GetComponent<AudioSource> ();//get the audio source
				}
		}

		// Update is called once per frame
		void Update ()
		{
				if (matchDone) {
						return;
				}

				CheckMatchDone ();//check matching

				if (matchDone) {
						audioSource.Stop ();
						//When the entire Match Done
						if (youWinAnimator != null) {
								audioSource.clip = youWin;
								audioSource.Play();//play win audio clip
								youWinAnimator.SetBool ("isRunning", true);//show you win on the screen
						}
				}

				if (Input.GetMouseButtonDown (0)) { 
						RayCast2D (Input.mousePosition);
				} else if (Input.GetMouseButtonUp (0)) {
			      		 //on release
						if (!clickedOnDest) {
								if (currentMatchingItem != null) {
										if (currentMatchingItem.GetComponent<MatchingItem> ().itemType == MatchingItem.ItemType.SOURCE) {
												currentMatchingItem.collider2D.enabled = true;//enable the collider for the current matching item
												currentMatchingItem.GetComponent<FollowTarget> ().isRunning = true;//let the current matching item follows the target
										}
								}
						}

						//reset attributes
						startDraging = false;
						clickedOnDest = false;
						currentMatchingItem = null;
				}


				if (startDraging) {//drag the matching item
						clickPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);//get the click point
						clickPoint.z = 5;
						currentMatchingItem.transform.position = clickPoint - offset;//calculate the offset
						RayCast2D (Input.mousePosition);//raycast
				}
		}

		//2d screen raycast
		private void RayCast2D (Vector3 pos)
		{
		
				pos = Camera.main.ScreenToWorldPoint (pos);//get click position
		
				RaycastHit2D rayCastHit2D = Physics2D.Raycast (pos, Vector2.zero);
		
				if (rayCastHit2D.collider == null) {
						return;
				}
		
				if (rayCastHit2D.collider.tag == "MatchingItem" && ! startDraging) {
						if (!clickedOnDest) {
								currentMatchingItem = rayCastHit2D.collider.gameObject;//get the current matching item
								if (currentMatchingItem.GetComponent<MatchingItem> ().connected || currentMatchingItem.GetComponent<MatchingItem> ().itemType != MatchingItem.ItemType.SOURCE) {//skip if the current matching item is connect or it's not a source
										return;
								}
								//On source click
								currentMatchingItem.GetComponent<FollowTarget> ().isRunning = false;//stop follow target
								currentMatchingItem.collider2D.enabled = false;//disable the collider for the current matching item
								startDraging = true;//trigger start dragging
								clickPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);//get click point position
								clickPoint.z = 5;
								offset = clickPoint - currentMatchingItem.transform.position;//calculate the offset
								audioSource.clip = currentMatchingItem.GetComponent<MatchingItem> ().dragAudioClip;
								audioSource.Play ();//play drag audio clip
						}
				} else if (rayCastHit2D.collider.tag == "MatchingItem" && startDraging) {
						if (currentMatchingItem.GetComponent<MatchingItem> ().id == rayCastHit2D.collider.GetComponent<MatchingItem> ().id && rayCastHit2D.collider.GetComponent<MatchingItem> ().itemType == MatchingItem.ItemType.DESTINATION) {
								//correct match
								clickedOnDest = true;//trigger clickedOnDest
								startDraging = false;//reset startDragging flag
								rayCastHit2D.collider.GetComponent<MatchingItem> ().connected = true;//trigger connected flag for the destination
								currentMatchingItem.GetComponent<MatchingItem> ().connected = true;//trigger connected flag for the source
								currentMatchingItem.transform.position = rayCastHit2D.collider.transform.position;//set the position of the source to the position of the destination
								currentMatchingItem.transform.localScale = rayCastHit2D.collider.transform.localScale;//set the scale of the source as the scale of the destination
								rayCastHit2D.collider.gameObject.SetActive (false);//hide the destination
								audioSource.clip = currentMatchingItem.GetComponent<MatchingItem> ().dropAudioClip;
								audioSource.Play ();//play drop audio clip
						} else {
								//wrong match;
						}
				} 
		}

		//Check if the matching for the entire matching items is done or not.
		private void CheckMatchDone ()
		{
				MatchingItem [] matchingItems = (MatchingItem[])GameObject.FindObjectsOfType (typeof(MatchingItem));
		
				matchDone = true;
				foreach (MatchingItem item in matchingItems) {
						if (!item.connected) {
								matchDone = false;
								return;
						}
				}

		}
}
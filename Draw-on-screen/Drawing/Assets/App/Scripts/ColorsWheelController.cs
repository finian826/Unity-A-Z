using UnityEngine;
using System.Collections;

/// <summary>
/// Colors wheel controller.
/// </summary>
public class ColorsWheelController : MonoBehaviour
{
		private float ZPos = 0;
		private float thetaOffset;//angle offset between current angle and click angle
		private Vector3 lastAngle, prevAngle;//previous and last angles for mouse position point
		private bool isClickedOnWheel;//is clicked on wheel
		public static bool isWheelRotating;//is wheel rotating
		private bool isClickReleased;//is click released
		private bool isLerpingToTarget;//is already lerping to a target
		private float rotationSpeed;//speed of rotation
		public Camera cam;

		// Use this for initialization
		void Start ()
		{
				lastAngle = prevAngle = transform.eulerAngles;
				if (cam == null) {
						cam = Camera.main;
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetMouseButtonDown (0)) {//when click began
						Vector3 wantedPos = cam.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, ZPos - cam.transform.position.z));
						RaycastHit2D hit2d = Physics2D.Raycast (wantedPos, Vector2.zero);
		
						if (hit2d.collider != null) {
								if (hit2d.collider.tag == "ColorsWheel" || hit2d.collider.tag == "WheelColor") {
										if (isWheelRotating) {//if wheel is rotating
												StartCoroutine ("DecreasingRotationSpeed");//decrease rotation speed
										} else {
												CalculateThetaOffset ();//calculate theta offset
												lastAngle = prevAngle = transform.eulerAngles;
												isClickedOnWheel = true;
												if (hit2d.collider.tag == "WheelColor") {//if there is a click on wheelcolor
														StopAllCoroutines ();
														isLerpingToTarget = false;
												} else if (hit2d.collider.tag == "ColorsWheel") {//if there is a click on colorswheel
														StopAllCoroutines ();
														isLerpingToTarget = false;
												}
										}
								}
						}
				} else if (Input.GetMouseButtonUp (0)) {//when click released

						Vector3 wantedPos = cam.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, ZPos - cam.transform.position.z));
						RaycastHit2D hit2d = Physics2D.Raycast (wantedPos, Vector2.zero);
			
						if (hit2d.collider != null) {
								if (isLerpingToTarget) {
										StopAllCoroutines ();
										isLerpingToTarget = false;
								} else if (hit2d.collider.tag == "WheelColor") {
										StopAllCoroutines ();
										isLerpingToTarget = false;
										if (Mathf.Abs (lastAngle.z - prevAngle.z) <= 0.1f) {
												LerpToTraget (hit2d.collider.gameObject);
										}
								} else if (hit2d.collider.tag == "ColorsWheel") {
										StopAllCoroutines ();
										isLerpingToTarget = false;
								}
						}

						if (isClickedOnWheel) {
								isClickReleased = true;
								isClickedOnWheel = false;
						}
				}
		}

		void FixedUpdate ()
		{
				if (isClickedOnWheel) {//if there is click on wheel

						Vector3 eulerAngles = transform.eulerAngles;
						float tragetAngle = CalculateTheta () + thetaOffset;
						float wantedAngle = Mathf.LerpAngle (eulerAngles.z, tragetAngle, 15 * Time.deltaTime);
						eulerAngles.z = wantedAngle;
						transform.eulerAngles = eulerAngles;
						prevAngle = lastAngle;
						lastAngle = eulerAngles;
				} else if (isClickReleased) {//if click on wheel released
						if (!isWheelRotating) {
								isClickReleased = false;
								StartCoroutine ("OnScrollingRelease");
						}
				}
		}

		//Lerp to a target
		private void LerpToTraget (GameObject traget)
		{
				Vector3 targetPos = traget.transform.position;
				Vector3 sourcePositon = transform.position;
				float angle1 = Mathf.Atan2 (targetPos.y - sourcePositon.y, targetPos.x - sourcePositon.x) * Mathf.Rad2Deg - 90;

				float thetaOffset = angle1;
				if (isLerpingToTarget) {
						StopAllCoroutines ();
				}
				StartCoroutine (LerpToAngle (transform.eulerAngles.z - thetaOffset));
		}

		//On Wheel Scroll Release
		private IEnumerator OnScrollingRelease ()
		{
				isWheelRotating = true;
				Vector3 eulerAngles = transform.eulerAngles;
				int rotationSign = 1;
				float distance = lastAngle.z - prevAngle.z;//distance(offset) between (prev Angle,last Angle)
				float decreamentSpeed = 0.4f;

				rotationSpeed = Mathf.Clamp (Mathf.Abs (distance), 0, 15);

				if (Mathf.Sign (distance) != 1) {
						rotationSign = -1;
				}

				while (rotationSpeed > 0) {
						eulerAngles.z += rotationSign * rotationSpeed;
						transform.eulerAngles = eulerAngles;
						rotationSpeed -= decreamentSpeed;
						if (rotationSpeed <= 0) {
								rotationSpeed = 0;
								isWheelRotating = false;
								yield break;
						}
						yield return new WaitForSeconds (0.01f);
				}

				rotationSpeed = 0;
				isWheelRotating = false;
		}

		//Decreasing Rotation Speed of Wheel Per Time
		private IEnumerator DecreasingRotationSpeed ()
		{
				float decreamentSpeed = 0.12f;

				while (rotationSpeed >0) {
						rotationSpeed -= decreamentSpeed;
						if (rotationSpeed < 0) {
								rotationSpeed = 0;
								isWheelRotating = false;
								isClickedOnWheel = true;
								CalculateThetaOffset ();
								yield break;
						}
						yield return new WaitForSeconds (0.01f);
				}
				CalculateThetaOffset ();
				rotationSpeed = 0;
				isClickedOnWheel = true;
		}

		//Lerp to angle
		private IEnumerator LerpToAngle (float tragetAngle)
		{
 
				Vector3 tempEulerAngle = transform.eulerAngles;
				float speed = 3;

				isLerpingToTarget = true;
				while (!Mathf.Approximately (tempEulerAngle.z, tragetAngle)) {
						if (!isLerpingToTarget) {
								break;
						}
						tempEulerAngle.z = Mathf.LerpAngle (tempEulerAngle.z, tragetAngle, Time.deltaTime * speed);
						transform.eulerAngles = tempEulerAngle;
						yield return 0;
				}

				isLerpingToTarget = false;
		}

		//Scrolling Release Event
		public void ScrollingRelease (float lastangle, float prevangle)
		{
				rotationSpeed = 0;
				isWheelRotating = false;
				StopAllCoroutines ();
				this.lastAngle.z = lastangle;
				this.prevAngle.z = prevangle;
				StartCoroutine ("OnScrollingRelease");
		}

		//Calculate the offset between current angle of wheel,click point
		private void CalculateThetaOffset ()
		{
				thetaOffset = transform.eulerAngles.z - CalculateTheta ();
		}

		//Calculate the angle between (click point,center of wheel).
		private float CalculateTheta ()
		{
				Vector3 tragetPostion = cam.ScreenToWorldPoint (Input.mousePosition);//target position
				Vector3 sourcePositon = transform.position;//source position
				return Mathf.Atan2 (tragetPostion.y - sourcePositon.y, tragetPostion.x - sourcePositon.x) * Mathf.Rad2Deg - 90;
		}
}
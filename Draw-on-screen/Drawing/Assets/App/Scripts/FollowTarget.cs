using UnityEngine;
using System.Collections;

/// <summary>
/// Follow target.
/// </summary>
public class FollowTarget: MonoBehaviour
{
		public Transform[] traget;//set of targets
		public float tragetDistanceOffset = 0.1f;//offset between current gameobject and follow target gameobject
		public bool reachedTarget;//is current gameobject reached the target
		public bool isRunning = true;
		public float speed = 10;//follow speed
		public bool followX, followY, followZ;
		public int tragetIndex = 0;
		private float xpos, ypos, zpos;
		private float smoothDampVelocity = 0;//smooth damp velocity
		public FollowMethod followMethod = FollowMethod.LERP;//follow method

		void Start ()
		{
				if (traget == null) {
						traget = new Transform[1];
				}
		}

		// Update is called once per frame
		void Update ()
		{
				if (!isRunning || reachedTarget || traget [tragetIndex] == null) {
						return;
				}

				if (followX) {
						xpos = GetValue (transform.position.x, traget [tragetIndex].position.x);//get x-positon follow value
				} else {
						xpos = transform.position.x;
				}

				if (followY) {
			ypos = GetValue (transform.position.y, traget [tragetIndex].position.y);//get y-positon follow value
				} else {
						ypos = transform.position.y;
				}

				if (followZ) {
			zpos = GetValue (transform.position.z, traget [tragetIndex].position.z);//get z-positon follow value
				} else {
						zpos = transform.position.z;
				}

				transform.position = new Vector3 (xpos, ypos, zpos);//apply current position on the current gameobject

				if (Mathf.Abs (Vector3.Distance (traget [tragetIndex].position, transform.position)) <= tragetDistanceOffset) {
						this.reachedTarget = true;//taget is reached
				}
		}
	
		public enum FollowMethod
		{
				LERP,
				SMOOTH_DAMP,
				SMOOTH_STEP,
				MOVE_TOWARD
	}
		;

		//Get Follow Value
		private float GetValue (float currentValue, float targetValue)
		{
				float returnValue = 0;
				if (followMethod == FollowMethod.LERP) {
						returnValue = Mathf.Lerp (currentValue, targetValue, speed * Time.deltaTime);
				} else if (followMethod == FollowMethod.MOVE_TOWARD) {
						returnValue = Mathf.MoveTowards (currentValue, targetValue, speed * Time.deltaTime);
				} else if (followMethod == FollowMethod.SMOOTH_DAMP) {
						returnValue = Mathf.SmoothDamp (currentValue, targetValue, ref smoothDampVelocity, speed);
				} else if (followMethod == FollowMethod.SMOOTH_STEP) {
						returnValue = Mathf.SmoothStep (currentValue, targetValue, speed * Time.deltaTime);
				}
				return returnValue;
		}
}
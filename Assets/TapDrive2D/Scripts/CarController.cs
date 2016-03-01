using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TapDrive2D.Vehicles.Car
{
	public class CarController : MonoBehaviour
	{
		[Range (0.1f, 1f)]
		public float sensitivityMult = 0.6f;
		public float maxSpeed;
		public float minSpeed;
		public float acceleration;
		public float speed;
		public List<Vector3> wayPoints;

		public int laps = 0;

		public Wheel2D[] wheels;

		bool canDrive = false;
		private Rigidbody2D rb;
		private Vector3 nextWayPoint = Vector3.zero;
		private int nextWayPointIndex = -1;

		// plotter
		public Object checkpoint;
		float lastTimeCreatedWayPoint = 0f;

		void SetNextWayPoint (Vector3 currentPosition)
		{
			var dist = Vector2.Distance (currentPosition, nextWayPoint);
			if (wayPoints.Count > 0 && (nextWayPoint == Vector3.zero || dist <= sensitivityMult)) {
//				if (speed >= maxSpeed) {
//					nextWayPointIndex = nextWayPointIndex + 3;
//				} else if (speed >= maxSpeed / 2f) {
//					nextWayPointIndex = nextWayPointIndex + 2;
//				} else {
//					nextWayPointIndex = nextWayPointIndex + 1;
//				}
				nextWayPointIndex = nextWayPointIndex + 1;
				if (nextWayPointIndex == wayPoints.Count) {
					laps++;
				}
				nextWayPointIndex = nextWayPointIndex % wayPoints.Count;
				nextWayPoint = wayPoints [nextWayPointIndex];
//				Instantiate (checkpoint, transform.position, Quaternion.identity);
//				Debug.Log ("next waypoint index: " + nextWayPointIndex + " - point: " + nextWayPoint);
			}
		}

		//		void SetRotation ()
		//		{
		//			// http://answers.unity3d.com/questions/650460/rotating-a-2d-sprite-to-face-a-target-on-a-single.html
		//			Vector3 vectorToTarget = nextWayPoint - transform.position;
		//			float angle = Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
		//			Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		//			transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime * speed * 100f);
		//		}

		void Drive ()
		{
//			SetRotation ();
//			rb.MovePosition (Vector3.Lerp (transform.position, nextWayPoint, speed * Time.deltaTime));
//			rb.MovePosition (Vector3.MoveTowards (transform.position, nextWayPoint, speed * Time.deltaTime));
//			var force = transform.up * speed * Time.deltaTime;
//			rb.AddForce (force);


			// if cross >= 30 turn right & move forward. increase speed on 
			// if cross <= -30 turn left & move forward
			// else: move forward
			var targetDir = nextWayPoint - transform.position;
//			float angle180 = Vector2.Angle (targetDir, transform.up);
			Vector3 cross = Vector3.Cross (targetDir, transform.up);
//			var angle360 = cross.z > 0 ? 360f - angle180 : angle180;
//			Debug.Log ("angle180: " + angle180 + " - angle360: " + angle360 + " - cross: " + cross.z);

			foreach (var wheel in wheels) {
				if (wheel != null) {
					var torque = 1f;
					Debug.Log ("wheel: " + name + " - cross: " + cross.z);
//					if (wheel.WheelSide == WheelSide.RIGHT && cross.z >= 30f) {
//						torque = 10000f * Mathf.Abs (cross.z);
//						Debug.Log (torque);
//					} else if (wheel.WheelSide == WheelSide.LEFT && cross.z <= -30f) {
//						torque = 10000f * Mathf.Abs (cross.z);
//						Debug.Log (torque);
//					}
					var force = (nextWayPoint - transform.position) * torque * speed * Time.deltaTime;
//					var force = Vector3.Lerp (nextWayPoint, transform.position, speed * Time.deltaTime) * angle360 / 360f;
					wheel.Drive (force);
				}
			}
		}

		void Interact ()
		{
			var hasTouch = false;
			if (Input.touchCount > 0) {
				var touch = Input.GetTouch (0);
				hasTouch = touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved;
			}
			if (Input.GetKey (KeyCode.UpArrow) || hasTouch) {
				if (speed < maxSpeed) {
					speed += acceleration * Time.deltaTime;
					speed = speed > maxSpeed ? maxSpeed : speed;
				}
			} else {
				if (speed > minSpeed) {
					speed -= (acceleration / 2f) * Time.deltaTime;
					speed = speed < 0 ? 0 : speed;
				}
			}
		}

		void SetupWayPoints ()
		{
			if (!canDrive) {
				canDrive = wayPoints.Count > 0 && Input.GetMouseButtonDown (1);
				if (canDrive) {
					foreach (var waypoint in GameObject.FindObjectsOfType<WayPoint>()) {
						waypoint.GetComponent<SpriteRenderer> ().enabled = false;
					}
				}
			}
			if (!canDrive && Input.GetMouseButton (0)) {
				if (Time.time - lastTimeCreatedWayPoint >= 0.1f) {
					var pos = Input.mousePosition;
					pos.z = 10;
					pos = Camera.main.ScreenToWorldPoint (pos);
					if (!wayPoints.Contains (pos)) {
						Instantiate (checkpoint, pos, Quaternion.identity);
						wayPoints.Add (pos);
						lastTimeCreatedWayPoint = Time.time;
					}
				}
			}
		}

		// Use this for initialization
		void Start ()
		{
			rb = GetComponent<Rigidbody2D> ();
			wayPoints = new List<Vector3> ();
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
			Interact ();
			SetupWayPoints ();
			if (canDrive) {
				SetNextWayPoint (transform.position);
				Drive ();
				Debug.DrawLine (transform.position, nextWayPoint, Color.red);
			}
		}
	}
}
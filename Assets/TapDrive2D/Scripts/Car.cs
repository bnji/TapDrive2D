using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace com.huldagames.TapDrive2D
{
	[ExecuteInEditMode]
	public class Car : MonoBehaviour, IObstacle
	{
		public CarProperties properties;
		public Wheel2D[] wheels;
		public AudioClip audioDriving;
		public AudioClip audioAcceleration;
		public AudioClip audioDeceleration;

		public bool IsEngineOn {
			get { return isEngineOn; }
		}

		WayPointHandler waypointHandler;
		CarScanner scanner;
		AudioSource audioSource;
		float speed;
		bool isEngineOn = false;
		private Rigidbody2D rb;

		public Object redDot;
		public Object blueDot;

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
			var targetDir = waypointHandler.NextWayPoint - transform.position;
//			float angle180 = Vector2.Angle (targetDir, transform.up);
			Vector3 cross = Vector3.Cross (targetDir, transform.up);
//			var angle360 = cross.z > 0 ? 360f - angle180 : angle180;
//			Debug.Log ("angle180: " + angle180 + " - angle360: " + angle360 + " - cross: " + cross.z);

			foreach (var wheel in wheels) {
				if (wheel != null) {
					var torque = 1f;
//					Debug.Log ("wheel: " + name + " - cross: " + cross.z);
//					if (wheel.WheelSide == WheelSide.RIGHT && cross.z >= 30f) {
//						torque = 10000f * Mathf.Abs (cross.z);
//						Debug.Log (torque);
//					} else if (wheel.WheelSide == WheelSide.LEFT && cross.z <= -30f) {
//						torque = 10000f * Mathf.Abs (cross.z);
//						Debug.Log (torque);
//					}
					var heading = waypointHandler.NextWayPoint - transform.position; 
					var direction = heading / heading.magnitude;
					var force = direction * torque * speed * Time.deltaTime * 120f;
					wheel.Drive (force);
				}
			}
		}

		void HandleUserInput ()
		{
			if (Input.GetKey (KeyCode.UpArrow) || (Input.touchCount > 0 && (Input.GetTouch (0).phase == TouchPhase.Stationary || Input.GetTouch (0).phase == TouchPhase.Moved))) {
				if (!audioSource.isPlaying) {
					if (speed == 0f) {
						audioSource.clip = audioAcceleration;
						audioSource.Play ();
					} else {
						audioSource.clip = audioDriving;
						audioSource.Play ();
					}
				}
				if (speed < properties.maxSpeed) {
					speed += properties.acceleration * Time.deltaTime;
					speed = speed > properties.maxSpeed ? properties.maxSpeed : speed;
				}
			} else {
				if (audioSource.clip != audioDeceleration && speed > 0) {
					audioSource.clip = audioDeceleration;
					audioSource.Play ();
				}
				if (speed > properties.minSpeed) {
					speed -= (properties.acceleration / 2f) * Time.deltaTime;
					speed = speed < 0 ? 0 : speed;
				}
			}
			if (Input.touchCount >= 1 && Input.GetTouch (0).tapCount >= 2) {
				GameManager.Instance.ReloadScene (0.5f);
			}
		}

		void HandleManualControl ()
		{
			if (Input.GetKey (KeyCode.UpArrow)) {
				rb.freezeRotation = false;
				foreach (var wheel in wheels) {
					if (wheel != null) {
						wheel.Move (WheelDirection.FORWARD);
					}
				}
			}
			if (Input.GetKey (KeyCode.DownArrow)) {
				rb.freezeRotation = true;
				foreach (var wheel in wheels) {
					if (wheel != null) {
						wheel.Move (WheelDirection.BACKWARD);
					}
				}
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				foreach (var wheel in wheels) {
					if (wheel != null) {
						wheel.TurnLeft ();
					}
				}
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				foreach (var wheel in wheels) {
					if (wheel != null) {
						wheel.TurnRight ();
					}
				}
			}
		}

		RaceTrackPlotter plotter;

		void Start ()
		{
			audioSource = GetComponent<AudioSource> ();
			rb = GetComponent<Rigidbody2D> ();
			// make sure auto mass is false
			rb.useAutoMass = false;
			plotter = GameObject.FindObjectOfType<RaceTrackPlotter> ();
			waypointHandler = new WayPointHandler (scanner);
		}

		void FixedUpdate ()
		{
//			HandleManualControl ();
//			return;
			if (isEngineOn) {
				HandleUserInput ();
				if (waypointHandler.SetNextWayPoint (transform.position, properties.sensitivityMult, true)) {
					Instantiate (redDot, waypointHandler.NextWayPoint, Quaternion.identity);
				}
				Drive ();
			} else {
				SetupWaypoints ();
			}
		}

		void Update ()
		{
			rb.mass = properties.weight;
			FunWithVectors ();
		}

		void SetupWaypoints ()
		{
			if (plotter != null) {
				waypointHandler.WayPoints = plotter.SetupWayPoints ();
				if (waypointHandler.WayPoints != null) {
					isEngineOn = true;
				}
			}
		}

		void OnScannerFoundItem (CarScanner.CarScannerHitResult result)
		{
			var hit = result.Hit;
			if (hit.distance >= 1f && hit.distance <= properties.obstacleMinDistanceBeforeDetection) {// && hit.collider.gameObject.GetType ().IsAssignableFrom (typeof(IObstacle))) {
				scanner = result.Scanner;
				scanner.IsActive = false;
//				var closesetWayPointIndex = GetClosesetWayPointIndex (hit.transform.position);
//				Debug.Log ("closeset waypoint index: " + closesetWayPointIndex + ", next waypoint index: " + nextWayPointIndex);
				var tempWaypoints = waypointHandler.ManipulateWayPoints (0, hit.transform, properties.obstaclePathSteps);
				foreach (var tempWaypoint in tempWaypoints) {
					Instantiate (blueDot, tempWaypoint, Quaternion.identity);
				}
//				Debug.Log ("Player hit '" + hit.collider.name + "'. Distance: " + hit.distance);
			}
		}

		void FunWithVectors ()
		{
			//			var targetDir = testWaypoint.position - testObstacle.position;
			//			var first = testWaypoint.position;
			//			var second = testObstacle.position;
			//			var newVec = first - second;
			//			var newVector = Vector3.Cross (newVec, Vector3.right);
			//			newVector.Normalize ();
			//			var newPoint = newVector + second;
			//			var newPoint2 = -newVector + second;
			//			testWaypointCopy.position = new Vector2 (newPoint2.x, newPoint2.y);
			//			Debug.Log (Vector2.Dot ((testObstacle.position - testWaypoint.position), (testObstacle.position - testObstacle.up)));
			//			Debug.DrawLine (testObstacle.position, testWaypoint.position * Vector2.Dot ((testObstacle.position - testWaypoint.position), (testObstacle.position - testObstacle.up)), Color.green);
			//			Debug.DrawLine (second, newVector);
			//			var deltaX = testWaypoint.position.x - testObstacle.position.x;
			//			var deltaY = testWaypoint.position.y - testObstacle.position.y;
			//			var delta = deltaY / deltaX;
			//			Vector3 cross = Vector3.Cross (targetDir.normalized, testObstacle.up);
			//			Debug.DrawLine (testObstacle.position, perp, Color.red);
			//			Debug.Log (Vector2.Angle (targetDir, testObstacle.up) - 180f);
			//			Debug.Log (Mathf.Clamp (deltaX, -1f, 1f) + ", " + Mathf.Clamp (deltaY, -1f, 1f));
			//			Debug.Log (Mathf.Clamp (delta, 0f, 1f));
			//			Debug.Log ("dx: " + deltaX + " - dy: " + deltaY + " - delta: " + Mathf.Clamp (delta, 0f, 1f) + " - cross.z: " + cross.z);
			//			Debug.Log (targetDir + " - " + cross.z);
		}
	}
}
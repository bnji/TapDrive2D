using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace TapDrive2D.Vehicles.Car
{
	[System.Serializable]
	public class CarProperties
	{
		[Range (1f, 5f)]
		[SerializeField]
		public float weight = 1f;

		[Range (1f, 25f)]
		[SerializeField]
		public int obstaclePathSteps = 7;

		[Range (1f, 10f)]
		[SerializeField]
		public float obstacleMinDistanceBeforeDetection = 7f;

		[Range (1f, 150f)]
		[SerializeField]
		public float maxSpeed = 100f;

		[Range (0f, 150f)]
		[SerializeField]
		public float minSpeed = 0f;

		[SerializeField]
		public float acceleration = 100f;
	}

	[ExecuteInEditMode]
	public class CarController : MonoBehaviour, IObstacle
	{
		public CarProperties properties;
		[Range (0.1f, 1f)]
		public float sensitivityMult = 0.6f;
		public int laps = 0;
		public Wheel2D[] wheels;
		public AudioClip audioDriving;
		public AudioClip audioAcceleration;
		public AudioClip audioDeceleration;

		public bool CanDrive {
			get { return _canDrive; }
		}

		AudioSource audio;
		float speed;
		bool _canDrive = false;
		private Rigidbody2D rb;
		// waypoints
		private List<Vector3> tempWayPoints;
		private Dictionary<int, Vector3> oldWaypoints = new Dictionary<int, Vector3> ();
		private Vector3[] wayPoints;
		private Vector3 nextWayPoint = Vector3.zero;
		private int nextWayPointIndex = -1;
		// plotter
		public float plotInterval = 200;
		public Object blueDot;
		public Object redDot;
		public Object checkpoint;
		float lastTimeCreatedWayPoint = 0f;

		int GetClosesetWayPointIndex (Vector3 position)
		{
			var index = nextWayPointIndex;
			var lowestDistanceRegistered = float.MaxValue;
			for (int i = 0; i < wayPoints.Length; i++) {
				var wp = wayPoints [i];
				var distance = Vector2.Distance (wp, transform.position);
				if (distance < lowestDistanceRegistered) {
					lowestDistanceRegistered = distance;
					index = i;
				}
			}
			return index;
		}

		void SetNextWayPoint (Vector3 currentPosition)
		{
			var dist = Vector2.Distance (currentPosition, nextWayPoint);
			if (wayPoints.Length > 0 && (nextWayPoint == Vector3.zero || dist <= sensitivityMult)) {

				if (nextWayPoint == Vector3.zero) {
					nextWayPointIndex = GetClosesetWayPointIndex (transform.position);
				}

//				if (speed >= properties.maxSpeed) {
//					nextWayPointIndex = nextWayPointIndex + 3;
//				} else if (speed >= properties.maxSpeed / 2f) {
//					nextWayPointIndex = nextWayPointIndex + 2;
//				} else {
//					nextWayPointIndex = nextWayPointIndex + 1;
//				}
				nextWayPointIndex = nextWayPointIndex + 1;
				if (nextWayPointIndex == wayPoints.Length) {
					laps++;
				}
				nextWayPointIndex = nextWayPointIndex % wayPoints.Length;
				nextWayPoint = wayPoints [nextWayPointIndex];
				Instantiate (redDot, nextWayPoint, Quaternion.identity);


				if (scanner != null && !scanner.IsActive) {
//					Debug.Log ("nextWayPointIndex: " + nextWayPointIndex + ", oldwaypoints: " + oldWaypoints.Count + ", lastModifiedWaypointIndexStart: " + lastModifiedWaypointIndexStart);
					if (nextWayPointIndex >= oldWaypoints.Count + lastModifiedWaypointIndexStart) {
						foreach (KeyValuePair<int, Vector3> kvp in oldWaypoints) {
							wayPoints [kvp.Key] = kvp.Value;
						}
						scanner.IsActive = true;
					}
				}
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
//					Debug.Log ("wheel: " + name + " - cross: " + cross.z);
//					if (wheel.WheelSide == WheelSide.RIGHT && cross.z >= 30f) {
//						torque = 10000f * Mathf.Abs (cross.z);
//						Debug.Log (torque);
//					} else if (wheel.WheelSide == WheelSide.LEFT && cross.z <= -30f) {
//						torque = 10000f * Mathf.Abs (cross.z);
//						Debug.Log (torque);
//					}
					var heading = nextWayPoint - transform.position; 
					var direction = heading / heading.magnitude;
					var force = direction * torque * speed * Time.deltaTime * 120f;
					wheel.Drive (force);
				}
			}
		}

		void HandleUserInput ()
		{
			var hasTouch = false;
			if (Input.touchCount > 0) {
				var touch = Input.GetTouch (0);
				hasTouch = touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved;
			}
			if (Input.GetKey (KeyCode.UpArrow) || hasTouch) {
				if (!audio.isPlaying) {
					if (speed == 0f) {
						audio.clip = audioAcceleration;
						audio.Play ();
					} else {
						audio.clip = audioDriving;
						audio.Play ();
					}
				}
				if (speed < properties.maxSpeed) {
					speed += properties.acceleration * Time.deltaTime;
					speed = speed > properties.maxSpeed ? properties.maxSpeed : speed;
				}
			} else {
				if (audio.clip != audioDeceleration && speed > 0) {
					audio.clip = audioDeceleration;
					audio.Play ();
				}
				if (speed > properties.minSpeed) {
					speed -= (properties.acceleration / 2f) * Time.deltaTime;
					speed = speed < 0 ? 0 : speed;
				}
			}
			if (Input.touchCount >= 1 && Input.GetTouch (0).tapCount >= 2) {
				StartCoroutine (ReloadScene ());
			}
		}

		IEnumerator ReloadScene ()
		{
			yield return new WaitForSeconds (0.5f);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			yield return new WaitForEndOfFrame ();
		}

		bool hasRegisteredFingers = false;

		void SetupWayPoints ()
		{
			if (!_canDrive) {
				_canDrive = tempWayPoints.Count > 0 && (Input.GetMouseButtonDown (1) || (hasRegisteredFingers && Input.touchCount >= 1 && Input.GetTouch (0).phase == TouchPhase.Ended));
				if (_canDrive) {
					wayPoints = tempWayPoints.ToArray ();// Helpers.MakeSmoothCurve (tempWayPoints, 5f);
					tempWayPoints = null;
//					foreach (var waypoint in GameObject.FindObjectsOfType<WayPoint>()) {
//						waypoint.GetComponent<SpriteRenderer> ().enabled = false;
//					}
				}
			}
			if (!_canDrive && (Input.GetMouseButton (0) || (Input.touchCount >= 1 && (Input.GetTouch (0).phase == TouchPhase.Began || Input.GetTouch (0).phase == TouchPhase.Moved)))) {
				if ((Time.time - lastTimeCreatedWayPoint) * 1000f >= plotInterval) {
					var pos = Input.mousePosition;
					pos.z = 10;
					pos = Camera.main.ScreenToWorldPoint (pos);
					if (!tempWayPoints.Contains (pos)) {
						Instantiate (checkpoint, pos, Quaternion.identity);
						tempWayPoints.Add (pos);
						lastTimeCreatedWayPoint = Time.time;
						hasRegisteredFingers = true; // temp
					}
				}
			}
		}

		void Start ()
		{
			audio = GetComponent<AudioSource> ();
			rb = GetComponent<Rigidbody2D> ();
			// make sure auto mass is false
			rb.useAutoMass = false;
			tempWayPoints = new List<Vector3> ();
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

		void FixedUpdate ()
		{
//			HandleManualControl ();
//			return;
			SetupWayPoints ();
			if (_canDrive) {
				HandleUserInput ();
				SetNextWayPoint (transform.position);
				Drive ();
				Debug.DrawLine (transform.position, nextWayPoint, Color.red);
			}
		}

		void Update ()
		{
			rb.mass = properties.weight;
			FunWithVectors ();
		}

		CarScanner scanner;
		int lastModifiedWaypointIndexStart = -1;

		void OnScannerFoundItem (CarScannerHitResult result)
		{
			var hit = result.Hit;
			if (hit.distance >= 1f && hit.distance <= properties.obstacleMinDistanceBeforeDetection) {// && hit.collider.gameObject.GetType ().IsAssignableFrom (typeof(IObstacle))) {
				scanner = result.Scanner;
				scanner.IsActive = false;
//				var closesetWayPointIndex = GetClosesetWayPointIndex (hit.transform.position);
//				Debug.Log ("closeset waypoint index: " + closesetWayPointIndex + ", next waypoint index: " + nextWayPointIndex);
				manipulateWayPoints (nextWayPointIndex - 2, hit.transform, properties.obstaclePathSteps);
//				Debug.Log ("Player hit '" + hit.collider.name + "'. Distance: " + hit.distance);
			}
		}

		void manipulateWayPoint (int index, Transform t)
		{
			var pos = t.position;
			var rot = t.rotation.eulerAngles.z;
			var tempWP = wayPoints [index];
			rot = rot <= 0 || rot >= 360f ? 180f : rot;
			var rotMult = rot / 180f;
			var mult = 0.5f;
			var xt = 1 - Mathf.Abs (rotMult);
			var yt = 1 - xt;
			var rotMultSign = Mathf.Sign (rotMult);
			xt = xt * mult * rotMultSign;
			yt = yt * mult * rotMultSign;
//			Debug.Log ("xt: " + xt + ", yt: " + yt + ", rotmult: " + rotMult + ", rot: " + rot);
			wayPoints [index] = new Vector3 (tempWP.x + xt, tempWP.y + yt, nextWayPoint.z);
			Instantiate (blueDot, wayPoints [index], Quaternion.identity);
//			Debug.Log ("WayPoint changed from: " + tempWP + " to " + wayPoints [index]);
		}

		void manipulateWayPoints (int index, Transform t, int amount)
		{
			oldWaypoints.Clear ();
			lastModifiedWaypointIndexStart = index;
			for (int i = 0; i < amount; i++) {
				var newIndex = (index + i) % wayPoints.Length;
				if (!oldWaypoints.ContainsKey (newIndex)) {
					oldWaypoints.Add (newIndex, wayPoints [newIndex]);
					manipulateWayPoint (newIndex, t);
				}
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
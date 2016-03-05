using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace com.huldagames.TapDrive2D
{
	[ExecuteInEditMode]
	[RequireComponent (typeof(Rigidbody2D))]
	public class Car : MonoBehaviour, IObstacle
	{
		public bool isAI = false;
		public Object redDot;
		public Object blueDot;
		public CarProperties properties;
		public Wheel2D[] wheels;

		public bool IsEngineOn {
			get { return isEngineOn; }
			set { isEngineOn = value; }
		}

		public float Speed {
			get { return speed; }
			set { speed = value; }
		}

		public CarProperties Properties {
			get { return properties; }
		}

		public AudioHandler AudioHandler {
			get { return audioHandler; }
		}

		public Rigidbody2D RigidBody2D {
			get { return rb; }
		}

		private IInputController userController;
		private IInputController aiController;
		private AudioHandler audioHandler;
		private Rigidbody2D rb;
		private float speed;
		private bool isEngineOn = false;

		void Start ()
		{
			rb = GetComponent<Rigidbody2D> ();
			// make sure auto mass is false
			rb.useAutoMass = false;
			audioHandler = GetComponentInChildren<AudioHandler> ();
			aiController = new AIInputController (this);
			userController = new UserInputController (this);
		}

		void Update ()
		{
			rb.mass = properties.weight;
//			FunWithVectors ();
		}

		void FixedUpdate ()
		{
			if (isAI) {
				aiController.ProcessInput ();
			} else {
				userController.ProcessInput ();
			}
		}

		void OnNextWayPoint (Vector3 nextWayPoint)
		{
			Instantiate (redDot, nextWayPoint, Quaternion.identity);
		}

		void OnScannerFoundItem (CarScanner.HitResult result)
		{
			if (isAI) {
				var tempWaypoints = aiController.HandleScannerItem (result);
//				Debug.Log (tempWaypoints);
				if (tempWaypoints != null) {
					foreach (var tempWaypoint in tempWaypoints) {
						Instantiate (blueDot, tempWaypoint, Quaternion.identity);
					}
				}
			}
		}

		void OnMovingForward ()
		{
			if (speed < properties.maxSpeed) {
				speed += properties.acceleration * Time.deltaTime;
				speed = speed > properties.maxSpeed ? properties.maxSpeed : speed;
			}
			if (audioHandler != null) {
//				audioHandler.HandleAcceleration (speed);
			}

		}

		void OnMovingBackwards ()
		{
			if (speed > properties.minSpeed) {
				speed -= (properties.acceleration / 2f) * Time.deltaTime;
				speed = speed < 0 ? 0 : speed;
			}
			if (audioHandler != null) {
//				audioHandler.HandleDeceleration (speed);
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



		//		void SetRotation ()
		//		{
		//			// http://answers.unity3d.com/questions/650460/rotating-a-2d-sprite-to-face-a-target-on-a-single.html
		//			Vector3 vectorToTarget = nextWayPoint - transform.position;
		//			float angle = Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
		//			Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		//			transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime * speed * 100f);
		//		}


	}
}
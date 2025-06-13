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
		public Object redDot;
		public Object blueDot;
		public CarProperties _properties;
		public Wheel2D[] wheels;

		public bool IsEngineOn {
			get { return _isEngineOn; }
			set { _isEngineOn = value; }
		}

		public float Speed {
			get { return _speed; }
			set { _speed = value; }
		}

		public CarProperties Properties {
			get { return _properties; }
		}

		public AudioHandler AudioHandler {
			get { return _audioHandler; }
		}

		public Rigidbody2D RigidBody2D {
			get { return _rigidBody2D; }
		}

		public IInputController InputController {
			get { return _inputController; }
		}

		private IInputController _inputController;
		private AudioHandler _audioHandler;
		private Rigidbody2D _rigidBody2D;
		private float _speed;
		private bool _isEngineOn = false;

		void Start ()
		{
			_rigidBody2D = GetComponent<Rigidbody2D> ();
			// make sure auto mass is false
			_rigidBody2D.useAutoMass = false;
			_audioHandler = GetComponentInChildren<AudioHandler> ();
            _inputController = new AIInputController (this);
            //_inputController = new UserInputController(this);
            //			_inputController = new FollowMouseInputController (this);
        }

		void OnValidate ()
		{
//			Debug.Log ("??");
		}

		void Update ()
		{
			_rigidBody2D.mass = _properties.weight;
//			FunWithVectors ();
		}

		void FixedUpdate ()
		{
			_inputController.ProcessInput ();
		}

		void OnNextWayPoint (Vector3 nextWayPoint)
		{
			Instantiate (redDot, nextWayPoint, Quaternion.identity);
		}

		void OnScannerFoundItem (CarScanner.HitResult result)
		{
			var tempWaypoints = _inputController.HandleScannerItem (result);
//				Debug.Log (tempWaypoints);
			if (tempWaypoints != null) {
				foreach (var tempWaypoint in tempWaypoints) {
					Instantiate (blueDot, tempWaypoint, Quaternion.identity);
				}
			}
		}

		void OnMovingForward ()
		{
			if (_speed < _properties.maxSpeed) {
				_speed += _properties.acceleration * Time.deltaTime;
				_speed = _speed > _properties.maxSpeed ? _properties.maxSpeed : _speed;
			}
			if (_audioHandler != null) {
//				audioHandler.HandleAcceleration (speed);
			}

		}

		void OnMovingBackwards ()
		{
			if (_speed > _properties.minSpeed) {
				_speed -= (_properties.acceleration / 2f) * Time.deltaTime;
				_speed = _speed < 0 ? 0 : _speed;
			}
			if (_audioHandler != null) {
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
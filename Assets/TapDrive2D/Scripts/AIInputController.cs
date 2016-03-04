using UnityEngine;
using System.Collections;

namespace com.huldagames.TapDrive2D
{
	public class AIInputController : IInputController
	{
		private RaceTrackPlotter plotter;
		private CarScanner scanner;
		private WayPointHandler waypointHandler;
		private Car car;

		public AIInputController (Car _car)
		{
			waypointHandler = new WayPointHandler (scanner);
			car = _car;
			plotter = GameObject.FindObjectOfType<RaceTrackPlotter> ();
		}

		public void ProcessInput ()
		{
			if (Input.GetKey (KeyCode.UpArrow) || (Input.touchCount > 0 && (Input.GetTouch (0).phase == TouchPhase.Stationary || Input.GetTouch (0).phase == TouchPhase.Moved))) {
				if (car.AudioHandler != null) {
					car.AudioHandler.HandleAcceleration (car.Speed);
				}
				if (car.Speed < car.Properties.maxSpeed) {
					car.Speed += car.Properties.acceleration * Time.deltaTime;
					car.Speed = car.Speed > car.Properties.maxSpeed ? car.Properties.maxSpeed : car.Speed;
				}
			} else {
				if (car.AudioHandler != null) {
					car.AudioHandler.HandleDeceleration (car.Speed);
				}
				if (car.Speed > car.Properties.minSpeed) {
					car.Speed -= (car.properties.acceleration / 2f) * Time.deltaTime;
					car.Speed = car.Speed < 0 ? 0 : car.Speed;
				}
			}
			if (Input.touchCount >= 1 && Input.GetTouch (0).tapCount >= 2) {
				GameManager.Instance.ReloadScene (0.5f);
			}

			if (car.IsEngineOn) {
				if (waypointHandler.SetNextWayPoint (car.transform.position, car.Properties.sensitivityMult, true)) {
					car.SendMessage ("OnNextWayPoint", waypointHandler.NextWayPoint, SendMessageOptions.DontRequireReceiver);
				}
				Drive ();
			} else {
				SetupWaypoints ();
			}
		}

		void SetupWaypoints ()
		{
			if (plotter != null) {
				waypointHandler.WayPoints = plotter.SetupWayPoints ();
				if (waypointHandler.WayPoints != null) {
					car.IsEngineOn = true;
				}
			}
		}

		void Drive ()
		{
			foreach (var wheel in car.wheels) {
				if (wheel != null) {
					// Calculate force
					var heading = waypointHandler.NextWayPoint - car.transform.position;
					var direction = heading / heading.magnitude;
					var force = direction * car.Speed * Time.deltaTime * 60f;
					// Apply force to wheels
					wheel.ApplyForce (force);
				}
			}
		}

		public Vector3[] HandleScannerItem (CarScanner.HitResult result)
		{
			var hit = result.Hit;
			if (hit.distance >= 1f && hit.distance <= car.Properties.obstacleMinDistanceBeforeDetection) {
				scanner = result.Scanner;
				scanner.IsActive = false;
				waypointHandler.Scanner = scanner;
				var tempWaypoints = waypointHandler.ManipulateWayPoints (0, hit.transform, car.Properties.obstaclePathSteps);
				return tempWaypoints;
			}
			return null;
		}
	}
}
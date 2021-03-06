﻿using UnityEngine;
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
			if (car.IsEngineOn) {
				if (Input.GetKey (KeyCode.UpArrow) || (Input.touchCount > 0 && (Input.GetTouch (0).phase == TouchPhase.Stationary || Input.GetTouch (0).phase == TouchPhase.Moved))) {
					car.SendMessage ("OnMovingForward", SendMessageOptions.DontRequireReceiver);
				} else {
					car.SendMessage ("OnMovingBackwards", SendMessageOptions.DontRequireReceiver);
				}
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
			if (plotter != null && !car.IsEngineOn) {
				LoadWayPoints (plotter.SetupWayPoints ());
			}
		}

		public void LoadWayPoints (Vector3[] wayPoints)
		{
			waypointHandler.WayPoints = wayPoints;
			if (waypointHandler.WayPoints != null) {
				car.IsEngineOn = true;
			}
		}

		void Drive ()
		{
			// Calculate force
			var power = Vector2.Dot (car.transform.up.normalized, (waypointHandler.NextWayPoint - car.transform.position).normalized);
			var heading = waypointHandler.NextWayPoint - car.transform.position;
			var direction = heading / heading.magnitude;
			var force = direction * car.Speed * Time.deltaTime * 60f * Mathf.Clamp (power, 0.3f, 1f);
			foreach (var wheel in car.wheels) {
				if (wheel != null) {
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
				var tempWayPoints = waypointHandler.ManipulateWayPoints (0, hit.transform, car.Properties.obstaclePathSteps);
//				tempWayPoints = Helpers.MakeSmoothCurve (tempWayPoints, 5f);
				return tempWayPoints;
			}
			return null;
		}
	}
}
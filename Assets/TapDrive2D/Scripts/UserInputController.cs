﻿using UnityEngine;
using System.Collections;

namespace com.huldagames.TapDrive2D
{
	public class UserInputController : IInputController
	{
		private Car car;

		public UserInputController (Car _car)
		{
			car = _car;
		}

		public void ProcessInput ()
		{
			if (Input.GetKey (KeyCode.UpArrow)) {
				car.RigidBody2D.freezeRotation = false;
				Drive (1f);
				car.SendMessage ("OnMovingForward", SendMessageOptions.DontRequireReceiver);
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				car.RigidBody2D.freezeRotation = true;
				Drive (-0.5f);
				car.SendMessage ("OnMovingBackwards", SendMessageOptions.DontRequireReceiver);
			} else {
				foreach (var wheel in car.wheels) {
					wheel.ResetWheelPosition (3f);	
				}
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				foreach (var wheel in car.wheels) {
					if (wheel != null) {
						wheel.TurnLeft (car.Properties.turnSpeed * 500f);
					}
				}
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				foreach (var wheel in car.wheels) {
					if (wheel != null) {
						wheel.TurnRight (car.Properties.turnSpeed * 500f);
					}
				}
			}
		}

		void Drive (float dir)
		{
			foreach (var wheel in car.wheels) {
				if (wheel != null) {
					var force = dir * wheel.transform.up * car.Speed * Time.deltaTime * 32f;
					if (car.Properties.isFourWheelDrive) {
						wheel.ApplyForce (force);
					} else {
						if (!wheel.isRearWheel) {
							wheel.ApplyForce (force);
						}
					}
				}
			}
		}

		public void LoadWayPoints (Vector3[] wayPoints)
		{
			
		}

		public Vector3[] HandleScannerItem (CarScanner.HitResult result)
		{
			return null;
		}
	}
}
using UnityEngine;
using System.Collections;

namespace com.huldagames.TapDrive2D
{
	public enum WheelSide
	{
		LEFT,
		RIGHT
	}

	public enum WheelDirection
	{
		FORWARD = 1,
		BACKWARD = -1
	}

	[RequireComponent (typeof(Rigidbody2D))]
	[RequireComponent (typeof(HingeJoint2D))]
	public class Wheel2D : MonoBehaviour
	{
		public WheelProperties properties;
		public float torque = 100f;
		public bool isRearWheel = false;
		public bool isActive = true;
		public WheelSide WheelSide;

		Rigidbody2D rb;
		float angle = 0f;
		float speedMult = 1f;
		float targetDrag = 0f;
		float targetSpeedMult = 1f;

		public Rigidbody2D RigidBody2D {
			get { return rb; }
		}

		public void TurnLeft ()
		{
			Turn (1f, 50f);
		}

		public void TurnRight ()
		{
			Turn (-1f, 50f);
		}

		private void Turn (float direction, float turnPower)
		{
			if (Mathf.Abs (angle) <= properties.maxTurnAngle) {
				var newAngle = angle + direction * Time.deltaTime;
				angle = Mathf.Abs (newAngle) < properties.maxTurnAngle ? newAngle : angle;
//				transform.Rotate (new Vector3 (transform.localRotation.eulerAngles.x, transform.localRotation.y, angle));
				transform.localRotation = Quaternion.Euler (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, angle);
//				Debug.Log (angle + " - " + Mathf.Round (transform.localRotation.eulerAngles.z) + " - " + rot);
			}
		}

		public void Move (WheelDirection direction)
		{
			var force = (float)direction * transform.up * torque * Time.deltaTime;
			rb.AddForce (force);
		}

		public void Drive (Vector3 force)
		{
			if (isActive) {
				if (isRearWheel) {
					return;
				}
				rb.AddForce (force * targetSpeedMult);
			}
		}

		// Use this for initialization
		void Start ()
		{
			rb = GetComponent<Rigidbody2D> ();
			targetDrag = rb.drag;
		}

		void FixedUpdate ()
		{
			if (!isRearWheel) {
				AdjustParametersToGround ();
			}
		}

		void AdjustParametersToGround ()
		{
			var hit = Physics2D.Raycast (transform.position, Vector2.down);
			if (hit.collider.tag.Equals ("RaceTrack")) {
				switch (hit.collider.name) {
				case "racetrack2_dirt":
					targetDrag = 25f;
					speedMult = 0.7f;
					break;
				case "racetrack2_grass":
					targetDrag = 8f;
					speedMult = 0.3f;
					break;
				case "racetrack2_road":
					targetDrag = 10f;
					speedMult = 1f;
					break;
				}
			}
			rb.drag = targetDrag;
//			if (rb.drag <= targetDrag) {
//				rb.drag += 5f * Time.deltaTime;
//			} else if (rb.drag >= targetDrag) {
//				rb.drag -= 5f * Time.deltaTime;
//			}
			if (targetSpeedMult <= speedMult) {
				targetSpeedMult += 3f * Time.deltaTime;
			} else if (targetSpeedMult >= speedMult) {
				targetSpeedMult -= 3f * Time.deltaTime;
			}
		}
	}
}
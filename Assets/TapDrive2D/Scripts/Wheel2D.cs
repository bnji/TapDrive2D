using UnityEngine;
using System.Collections;

namespace com.huldagames.TapDrive2D
{
	[ExecuteInEditMode]
	[RequireComponent (typeof(Rigidbody2D))]
	[RequireComponent (typeof(HingeJoint2D))]
	public class Wheel2D : MonoBehaviour
	{
		public WheelProperties properties;
		public float torque = 100f;
		public bool isRearWheel = false;
		public bool isActive = true;

		Rigidbody2D rb;
		float angle = 0f;
		float speedMult = 1f;
		float targetDrag = 0f;
		float targetSpeedMult = 1f;
		bool canTurn = true;

		public Rigidbody2D RigidBody2D {
			get { return rb; }
		}

		public void TurnLeft (float turnPowerMult)
		{
			Turn (1f * turnPowerMult);
		}

		public void TurnRight (float turnPowerMult)
		{
			Turn (-1f * turnPowerMult);
		}

		public void ResetWheelPosition (float friction = 3f)
		{
			rb.drag = friction;
		}

		private void Turn (float turnPower)
		{
			if (canTurn && rb.velocity.magnitude > 0f) {
				var newAngle = angle + turnPower * Time.deltaTime;
				newAngle = Mathf.Abs (newAngle) >= properties.maxTurnAngle ? properties.maxTurnAngle : Mathf.Abs (newAngle);
				Debug.Log (newAngle);
				if (Mathf.Abs (newAngle) <= properties.maxTurnAngle) {
					angle = Mathf.Abs (newAngle) < properties.maxTurnAngle ? newAngle : angle;
					rb.MoveRotation (rb.rotation + turnPower * Time.fixedDeltaTime);// + angle);
//				transform.Rotate (new Vector3 (transform.localRotation.eulerAngles.x, transform.localRotation.y, angle));
//				transform.localRotation = Quaternion.Euler (transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, angle);
//				Debug.Log (angle + " - " + Mathf.Round (transform.localRotation.eulerAngles.z));
				}
			}
		}

		public void ApplyForce (Vector3 force)
		{
			if (isActive) {
				canTurn = Vector2.Dot (transform.up.normalized, force.normalized) > 0f;
				rb.AddForce (force * speedMult);
			}
		}

		void Update ()
		{
			GetComponent<Rigidbody2D> ().drag = properties.friction;
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
			if (hit.collider != null) {
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
}
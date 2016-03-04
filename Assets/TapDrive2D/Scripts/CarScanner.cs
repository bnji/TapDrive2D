using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.huldagames.TapDrive2D
{
	public class CarScanner : MonoBehaviour
	{
		[Tooltip ("Scan interval in milliseconds")]
		public float scanInterval = 10f;
		public List<string> validTags;

		public bool IsActive {
			get;
			set;
		}

		Car car;
		float lastHitTime = 0f;

		// Use this for initialization
		void Start ()
		{
			IsActive = true;
			car = GetComponentInParent<Car> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
			Debug.DrawRay (transform.position, transform.up);
			if (IsActive && car != null && car.IsEngineOn && (Time.time - lastHitTime) * 1000f >= scanInterval) {
				var hasHit = false;
				var hits = Physics2D.RaycastAll (transform.position, transform.up);
				foreach (var hit in hits) {
					hasHit = CheckHit (hit);
				}
				if (!hasHit) {
					var hits2 = Physics2D.RaycastAll (transform.position + new Vector3 (0.5f, 0f), transform.up);
					foreach (var hit in hits2) {
						hasHit = CheckHit (hit);
					}
				}
				if (!hasHit) {
					var hits3 = Physics2D.RaycastAll (transform.position + new Vector3 (-0.5f, 0f), transform.up);
					foreach (var hit in hits3) {
						hasHit = CheckHit (hit);
					}
				}
				lastHitTime = Time.time;
			}
		}


		bool CheckHit (RaycastHit2D hit)
		{
			if (hit.collider != null) {
				if (validTags.Contains (hit.collider.tag)) {
					car.SendMessage ("OnScannerFoundItem", new HitResult () { Hit = hit, Scanner = this }, SendMessageOptions.DontRequireReceiver);
					return true;
				}
			}
			return false;
		}

		public struct HitResult
		{
			public RaycastHit2D Hit {
				get;
				set;
			}

			public CarScanner Scanner {
				get;
				set;
			}
		}
	}
}
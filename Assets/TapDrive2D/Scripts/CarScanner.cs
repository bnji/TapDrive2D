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

		Car carController;
		float lastHitTime = 0f;

		// Use this for initialization
		void Start ()
		{
			IsActive = true;
			carController = GetComponentInParent<Car> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (IsActive && carController != null && carController.IsEngineOn && (Time.time - lastHitTime) * 1000f >= scanInterval) {
				var hits = Physics2D.RaycastAll (transform.position, transform.up, 100f);
				foreach (var hit in hits) {
					CheckHit (hit);
				}
//				var hit = Physics2D.Raycast (transform.position, transform.up);
//				var hit2 = Physics2D.Raycast (transform.position + new Vector3 (0.5f, 0f), transform.up);
//				var hit3 = Physics2D.Raycast (transform.position + new Vector3 (-0.5f, 0f), transform.up);
//				if (!CheckHit (hit)) {
//					if (!CheckHit (hit2)) {
//						if (!CheckHit (hit3)) {
//							//...
//						}
//					}
//				}
				lastHitTime = Time.time;
			}
		}


		bool CheckHit (RaycastHit2D hit)
		{
			if (hit.collider != null) {
				if (validTags.Contains (hit.collider.tag)) {
					Debug.Log (hit.collider.tag);
					carController.SendMessage ("OnScannerFoundItem", new CarScannerHitResult () { Hit = hit, Scanner = this }, SendMessageOptions.DontRequireReceiver);
					return true;
				}
			}
			return false;
		}

		public struct CarScannerHitResult
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
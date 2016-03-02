using UnityEngine;
using System.Collections;

namespace com.huldagames.TapDrive2D
{
	public interface IInputController
	{
		void Handle ();

		Vector3[] HandleScannerItem (CarScanner.CarScannerHitResult result);
	}
}
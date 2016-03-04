using UnityEngine;
using System.Collections;

namespace com.huldagames.TapDrive2D
{
	public interface IInputController
	{
		void ProcessInput ();

		Vector3[] HandleScannerItem (CarScanner.HitResult result);
	}
}
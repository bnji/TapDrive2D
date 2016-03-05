using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace com.huldagames.TapDrive2D
{
	public class GameManager : MonoSingleton<GameManager>
	{
		public void ReloadScene (float delay = 0f)
		{
			StartCoroutine (_ReloadScene (delay));
		}

		static IEnumerator _ReloadScene (float delay)
		{
			yield return new WaitForSeconds (delay);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			yield return new WaitForEndOfFrame ();
		}

		public static Vector3[] LoadWayPoints ()
		{
			var path = System.IO.Path.Combine (Application.persistentDataPath, "waypoints.xml");
			var wayPoints = WayPointContainer.Load (path);
			if (wayPoints != null) {
				return wayPoints.WayPoints;
			}
			return null;
		}

		public static void SaveWayPoints (Vector3[] wayPoints)
		{
			var path = System.IO.Path.Combine (Application.persistentDataPath, "waypoints.xml");
			new WayPointContainer (wayPoints).Save (path);
		}
	}
}
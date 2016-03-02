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
	}
}
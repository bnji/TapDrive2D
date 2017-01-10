using UnityEngine;
using System.Collections;

public class CubeContainer : MonoBehaviour {

    public TextHandler textHandler;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTimeElapsed(int time)
    {
        if (this.textHandler != null)
        {
            this.textHandler.SetText("" + time);
        }
    }
}

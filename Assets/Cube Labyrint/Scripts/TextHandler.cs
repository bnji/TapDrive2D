using UnityEngine;
using System.Collections;

public class TextHandler : MonoBehaviour {

    public TextMesh text;

    public void SetText(string text)
    {
        this.text.text = text;
    }
}

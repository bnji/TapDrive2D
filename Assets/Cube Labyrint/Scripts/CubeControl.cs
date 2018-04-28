using UnityEngine;
using System.Collections;
using System;

public class CubeControl : MonoBehaviour
{

    public float minSwipeDistY;
    public float minSwipeDistX;
    private Vector2 startPos;

    void Update()
    {
        //Debug.Log(Input.touchCount);
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.gameObject.SendMessage("OnSwipeUp");
            //Debug.Log("up");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.gameObject.SendMessage("OnSwipeDown");
            //Debug.Log("down");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.gameObject.SendMessage("OnSwipeRight");
            //Debug.Log("right");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.gameObject.SendMessage("OnSwipeLeft");
            //Debug.Log("left");
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Ended:
                    {
                        float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
                        float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
//                        Debug.Log(swipeDistHorizontal);
                        if (swipeDistVertical > minSwipeDistY)
                        {

                            float swipeValue = Mathf.Sign(touch.position.y - startPos.y);
                            if (swipeValue > 0f)
                            {
                                this.gameObject.SendMessage("OnSwipeUp");
                            }
                            else if (swipeValue < 0f)
                            {
                                this.gameObject.SendMessage("OnSwipeDown");
                            }
                        }
                        if (swipeDistHorizontal > minSwipeDistX)
                        {
                            float swipeValue = Mathf.Sign(touch.position.x - startPos.x);
                            if (swipeValue > 0f)
                            {
                                this.gameObject.SendMessage("OnSwipeRight");
                            }
                            else if (swipeValue < 0f)
                            {
                                this.gameObject.SendMessage("OnSwipeLeft");
                            }
                        }
                    }
                    break;
            }
        }
    }
}
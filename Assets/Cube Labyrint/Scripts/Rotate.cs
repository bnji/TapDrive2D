using UnityEngine;
using System.Collections;
using System;

public class Rotate : MonoBehaviour
{
    public RotationDirection rotationDirection = RotationDirection.CLOCKWISE;
    public float rotationInterval = 1f;
    public float rotationDegrees = 90f;
    public float rotationDelta = 5f;

    DateTime lastRotation;
    float tempRotation = 0f;
    Vector3 rotation;
    float lastTime = 0f;
    float tempTimeCounter = 0f;

    // Use this for initialization
    void Start()
    {
        this.lastRotation = DateTime.UtcNow;
        this.tempRotation = transform.eulerAngles.z;
        this.rotation = transform.eulerAngles;
        this.lastTime = Time.time;
        this.tempTimeCounter = this.rotationInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (DateTime.UtcNow.Subtract(this.lastRotation).Seconds > this.rotationInterval)
        {
            this.tempTimeCounter = this.rotationInterval;
            if (Mathf.Abs(transform.eulerAngles.z - this.tempRotation % 360f) < this.rotationDegrees)
            {
                this.rotation.z = (rotationDirection == RotationDirection.COUNTER_CLOCKWISE ? 1f : -1f) * (this.rotationDelta * Time.deltaTime) % 360f;
                this.transform.Rotate(this.rotation);
            }
            else
            {
                this.tempRotation = Mathf.Floor(this.transform.eulerAngles.z);
                var tmpRotation = this.tempRotation - (this.tempRotation % this.rotationDegrees);
                this.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, tmpRotation));
                this.rotation = transform.eulerAngles;
                this.lastRotation = DateTime.UtcNow;
            }
        }
        else
        {
            if (Time.time - lastTime > 1f)
            {
                this.lastTime = Time.time;
                this.gameObject.SendMessageUpwards("OnTimeElapsed", this.tempTimeCounter, SendMessageOptions.DontRequireReceiver);
                this.tempTimeCounter--;
            }
        }
    }
}

public enum RotationDirection { CLOCKWISE, COUNTER_CLOCKWISE }
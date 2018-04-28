using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class CubePlayer : MonoBehaviour
{

    public float movementForce = 1f;
    public Rigidbody2D rigidBody;

    public bool IsInitialized { get; private set; }

    bool isMoving = false;
    Transform cubeCenter = null;
    float minDistRegistered = int.MaxValue;
    int lastCubeHashCode = -1;
    int cubeHitCount = 0;

    void Start()
    {
        IsInitialized = false;
    }

    void FixedUpdate()
    {
        if (cubeCenter != null)
        {
            if (cubeHitCount > 1 && lastCubeHashCode != cubeCenter.GetHashCode())
            {
                var currentDistance = Mathf.Abs(Vector2.Distance(new Vector2(cubeCenter.position.x, cubeCenter.position.y), new Vector2(this.transform.position.x, this.transform.position.y)));
                if (currentDistance < minDistRegistered)
                {
                    minDistRegistered = currentDistance;
                }
                else
                {
                    this.minDistRegistered = int.MaxValue;
                    this.rigidBody.velocity = Vector2.zero;
                    this.transform.parent = cubeCenter;
                    this.transform.localPosition = cubeCenter.localPosition;
                    isMoving = false;
                    lastCubeHashCode = cubeCenter.GetHashCode();
//                    Debug.Log(currentDistance);
                }
            }
        }
    }

    internal void Initialize(Transform t)
    {
        if (!this.IsInitialized)
        {
            this.transform.parent = t;
            this.transform.position = t.position;
            this.IsInitialized = true;
        }
    }

    void OnSwipeUp()
    {
        if (isMoving)
            return;
        this.rigidBody.AddForce(Vector2.up * 1000f * movementForce * Time.deltaTime);
        isMoving = true;
    }

    void OnSwipeDown()
    {
        if (isMoving)
            return;
        this.rigidBody.AddForce(-1f * Vector2.up * 1000f * movementForce * Time.deltaTime);
        isMoving = true;
    }

    void OnSwipeLeft()
    {
        if (isMoving)
            return;
        this.rigidBody.AddForce(-1f * Vector2.right * 1000f * movementForce * Time.deltaTime);
        isMoving = true;
    }

    void OnSwipeRight()
    {
        if (isMoving)
            return;
        this.rigidBody.AddForce(Vector2.right * 1000f * movementForce * Time.deltaTime);
        isMoving = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.name.Equals("Center") && !collider.name.StartsWith("Cube Spawner"))
        {
            SceneManager.LoadScene(1);
        }
    }

    void OnHitCubeCenter(Transform t)
    {
        this.cubeCenter = t;
        this.cubeHitCount++;
    }
}
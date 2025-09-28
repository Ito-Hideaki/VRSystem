using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : MonoBehaviour
{
    const float maxSpeed = 30.0f;
    float speed = 0;
    bool finallyMoving = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.position = new Vector3(0, WallGenerator.groundY + 3, 0);
    }

    public void StartMove()
    {
        finallyMoving = true;
    }

    public void StopMove()
    {
        finallyMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        //speed control
        //accelerate if moving, deccelerate if not
        float acceleration = (finallyMoving ? 5 : speed * -0.5f - 1);
        speed = Mathf.Max(0, Mathf.Min(maxSpeed, speed + acceleration * Time.deltaTime));

        //move foward
        this.transform.Translate(0, 0, Time.deltaTime * -1 * speed);
    }
}

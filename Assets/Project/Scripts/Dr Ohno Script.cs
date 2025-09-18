using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrOhnoScript : MonoBehaviour
{
    [SerializeField] Minecart minecart;

    bool chasing = false;
    float floatingProportion = 0; //0~1
    const float FLOAT_Y = -3;
    const float AVERAGE_DIFFERENCE_Z = 10f;
    int tick = 0;
    Vector3 floatingShake = Vector3.zero;
    Vector3 floatingPosition = Vector3.zero;
    Vector3 position;
    bool finallyFloating = false;

    Vector3 floatingVelocity = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        tick++;
        //set floating shake
        if(tick % 75 == 0)
        {
            floatingShake.z = Random.Range(-2f, 2f);
        }
        if (tick % 75 == 25)
        {
            floatingShake.y = Random.Range(-1f, 1f);
        }
        if (tick % 75 == 50)
        {
            floatingShake.x = Random.Range(-1f, 1f);
        }
        //progress floating proportion
        {
            float floatingAddition = (finallyFloating ? 1 : -1) * ((1 - floatingProportion) * (floatingProportion) * 2.0f + 0.05f) * Time.deltaTime;
            this.floatingProportion += floatingAddition;
            this.floatingProportion = Mathf.Clamp(this.floatingProportion, 0, 1f);
            if(floatingProportion <= 0) floatingPosition = Vector3.zero;
        }
        
        //y coordinate
        float currentFloatingY = WallGenerator.groundY + floatingProportion * (FLOAT_Y - WallGenerator.groundY);
        position.y = currentFloatingY;

        //z chasing coordinate
        if(chasing) {
            float differenceZ = this.position.z - minecart.transform.position.z;
            float velocityZ = (AVERAGE_DIFFERENCE_Z - differenceZ) * 5f * Time.deltaTime;
            position.z += velocityZ;
        }

        //x coordinate
        position.x = 0;

        //progress floating velocity
        {
            Vector3 difference = floatingPosition - floatingShake;
            Vector3 acceleration = -difference * 0.015f * Time.deltaTime;
            floatingVelocity += acceleration;
            floatingVelocity *= (1f - Time.deltaTime * 1f);
        }

        //progress floating position
        floatingPosition += floatingVelocity;

        //set actual position
        Vector3 dest = position + floatingPosition * floatingProportion;
        this.transform.Translate(dest - this.transform.position);

        //rotation
        //transform.rotation = Quaternion.Euler(floatingProportion * -50, 0, 0);
    }

    public void StartChase()
    {
        chasing = true;
    }

    public void StopChase()
    {
        chasing = false;
    }

    public void StartFloat()
    {
        finallyFloating = true;
    }

    public void StopFloat()
    {
        finallyFloating = false;
    }

    public bool FloatingJobEnd()
    {
        if (finallyFloating)
        {
            return floatingProportion >= 1f;
        } else
        {
            return floatingProportion <= 0;
        }
    }
}

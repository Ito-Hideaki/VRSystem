using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrOhnoScript : MonoBehaviour
{
    [SerializeField] Minecart minecart;
    [SerializeField] TangYeController tangYe;

    bool chasing = false;
    float floatingProportion = 0; //0~1
    const float FLOAT_Y = -3;
    const float AVERAGE_DIFFERENCE_Z = 10f;
    int tick = 0;
    Vector3 floatingShake = Vector3.zero;
    Vector3 floatingPosition = Vector3.zero;
    Vector3 position;
    bool finallyFloating = false;

    public float coordinateVelocityZ = 0;
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
        if(tick % 300 == 0)
        {
            floatingShake.z = Random.Range(-2f, 2f);
        }
        if (tick % 300 == 100)
        {
            floatingShake.y = Random.Range(-1f, 1f);
        }
        if (tick % 300 == 200)
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
            coordinateVelocityZ = (AVERAGE_DIFFERENCE_Z - differenceZ) * 5f;
            
        } else
        {
            coordinateVelocityZ = 0;
        }
        position.z += coordinateVelocityZ * Time.deltaTime;

        //x coordinate
        position.x = 0;

        //progress floating velocity
        {
            Vector3 difference = floatingPosition - floatingShake;
            Vector3 acceleration = -difference * 1f;
            floatingVelocity += acceleration * Time.deltaTime;
            floatingVelocity *= (1f - Time.deltaTime * 1f);
        }

        //progress floating position
        floatingPosition += floatingVelocity * Time.deltaTime;

        //set actual position
        Vector3 dest = position + floatingPosition * floatingProportion;
        this.transform.Translate(dest - this.transform.position);
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
        tangYe.SetStatus(TangYeController.Status.Released);
    }

    public void StopFloat()
    {
        finallyFloating = false;
        tangYe.SetStatus(TangYeController.Status.Stopped);
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

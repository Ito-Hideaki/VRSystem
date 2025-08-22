using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{

    public GameObject bottomWallPrefab;
    public GameObject minecart;

    float lastWallZ = 0;
    float wallGenerationLimit = 100.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //generate walls as minecart move
        while (lastWallZ < wallGenerationLimit + minecart.transform.position.z)
        {
            createWall();
        }
    }

    void createWall()
    {
        GameObject bottomWall = Instantiate(bottomWallPrefab) as GameObject;
        bottomWall.transform.position = new Vector3(0, -5, lastWallZ);
        lastWallZ += 15;
    }
}

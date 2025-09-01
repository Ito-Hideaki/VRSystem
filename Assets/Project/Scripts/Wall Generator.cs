using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class WallGenerator : MonoBehaviour
{

    [SerializeField] GameObject bottomWallPrefab;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject minecart;

    float lastWallZ = 0;
    float lastBottomWallZ = 0;
    float wallGenerationLimit = 1000f;
    const float wallZLength = 100f;
    const float bottomWallZLength = 50f;

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
            createPolygonCaveWall();
            lastWallZ += wallZLength;
        }
        //generate bottom walls as previous
        while (lastBottomWallZ < wallGenerationLimit + minecart.transform.position.z)
        {
            createBottomCaveWall();
            lastBottomWallZ += bottomWallZLength;
        }
    }

    void createBottomCaveWall()
    {
        GameObject bottomWall = Instantiate(bottomWallPrefab) as GameObject;
        bottomWall.transform.position = new Vector3(0, -8, lastBottomWallZ);
    }

    void createPolygonCaveWall()
    {
        float angle = 0;
        const int vertexNumber = 10;
        for(var i = 0; i < vertexNumber; i++)
        {
            if (9 <= i || i <= 1) continue;

            angle = (Mathf.PI / -2) + (Mathf.PI * 2 / vertexNumber) * i;
            float radius = 20f;
            float posX = Mathf.Cos(angle) * radius;
            float posY = Mathf.Sin(angle) * radius;
            GameObject wall = Instantiate(wallPrefab) as GameObject;
            wall.transform.position = new Vector3(posX, posY, lastWallZ);
            wall.transform.rotation = Quaternion.Euler(0, 0, i * 360 / vertexNumber);
        }
    }
}

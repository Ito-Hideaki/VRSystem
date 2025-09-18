using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    public const float groundY = -8f;

    [SerializeField] GameObject bottomWallPrefab;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject minecart;
    [SerializeField] GameObject ceilLightPrefab;

    float lastWallZ = 300f;
    float lastBottomWallZ = 300f;
    float lastCeilLightZ = 300f;
    float wallGenerationLimit = 300f;
    const float wallZLength = 100f;
    const float bottomWallZLength = 50f;
    const float ceilLightZLength = 50f;

    List<GameObject> walls = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //generate walls as minecart move
        while (wallGenerationLimit + lastWallZ > minecart.transform.position.z)
        {
            createPolygonCaveWall();
            lastWallZ -= wallZLength;
        }
        //generate bottom walls as previous
        while (wallGenerationLimit + lastBottomWallZ > minecart.transform.position.z)
        {
            createBottomCaveWall();
            lastBottomWallZ -= bottomWallZLength;
        }
        //generate ceil lights as previous
        while (wallGenerationLimit + lastCeilLightZ > minecart.transform.position.z)
        {
            CreateCeilLight();
            lastCeilLightZ -= ceilLightZLength;
        }

        //delete too far walls
        while (walls.Count > 0 && walls[0].transform.position.z > minecart.transform.position.z + wallGenerationLimit)
        {
            GameObject.Destroy(walls[0]);
            walls.RemoveAt(0);
        }
    }

    void createBottomCaveWall()
    {
        GameObject bottomWall = Instantiate(bottomWallPrefab) as GameObject;
        bottomWall.transform.position = new Vector3(0, groundY, lastBottomWallZ);
        walls.Add(bottomWall);
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
            wall.transform.rotation = Quaternion.Euler(0, 0, i * 360f / vertexNumber);
            walls.Add(wall);
        }
    }

    void CreateCeilLight()
    {
        GameObject light = Instantiate(ceilLightPrefab) as GameObject;
        light.transform.position = new Vector3(0, 17, lastCeilLightZ);
        walls.Add(light);
    }
}

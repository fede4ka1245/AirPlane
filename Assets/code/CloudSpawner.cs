using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] int cloudsPrefabs;
    [SerializeField] GameObject cloud;
    GameObject[] clouds;
    [SerializeField] Vector2 minAndMaxSpawnPosY;
    [SerializeField] Vector2 distanceBetweenClouds;
    float posBetweenClouds = 0;
    [SerializeField] float[] yPosesForSpawn;
    [SerializeField] float destroyPos;
    float respawnPos;
    [SerializeField] Vector2 translatePos;
    [SerializeField]public float speed;
    [SerializeField] Game game;

    private void Start()
    {
        for (int i = 0; i <= yPosesForSpawn.Length - 1; i++)
        {
            yPosesForSpawn[i] = Random.Range(minAndMaxSpawnPosY.x, minAndMaxSpawnPosY.y);
        }

        float xPos = -10;
        for (int i = 0; i < cloudsPrefabs; i++) 
        {   
            xPos += posBetweenClouds;
            posBetweenClouds = Random.Range(distanceBetweenClouds.x, distanceBetweenClouds.y);

            Instantiate(cloud, new Vector3(xPos, yPosesForSpawn[Random.Range(0, yPosesForSpawn.Length)], 0), Quaternion.identity);
        }

        clouds = GameObject.FindGameObjectsWithTag("cloud");
        respawnPos = xPos;
    }

    private void FixedUpdate()
    { 
        for (int i = 0; i <= clouds.Length - 1; i++)
        {
            if (clouds[i].transform.position.x <= destroyPos)
            {
                clouds[i].transform.position = new Vector3(respawnPos, clouds[i].transform.position.y, 0);
            }

            clouds[i].transform.Translate(translatePos * game.SpeedWithBoost / 10);
        }
        
    }
}

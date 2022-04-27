using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsAndCoinsSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] TrapsAndCoinsPrefabs;
    [SerializeField] GameObject[] TrapsAndCoins;
    [SerializeField] Vector2 distanceBetweenTrapsAndCoins;
    float posBetweenTrapsAndCoins = 0;
    [SerializeField] float destroyPos;
    float respawnPos;
    [SerializeField] Vector2 translatePos;
    [SerializeField] float speed;
    [SerializeField] Vector2 startPoses;
    [SerializeField] Game game;

    private void Start()
    {
        float startPos = Random.Range(startPoses.x, startPoses.y);
        float xPos = startPos;

        for (int i = 0; i <= TrapsAndCoins.Length - 1; i++)
        { 
            xPos += posBetweenTrapsAndCoins;
            posBetweenTrapsAndCoins = Random.Range(distanceBetweenTrapsAndCoins.x, distanceBetweenTrapsAndCoins.y);

            int numPrefabInTrapsAndCoinsPrefabs = Random.Range(0, TrapsAndCoinsPrefabs.Length);

            Instantiate(TrapsAndCoinsPrefabs[numPrefabInTrapsAndCoinsPrefabs],
                new Vector3(xPos, TrapsAndCoinsPrefabs[numPrefabInTrapsAndCoinsPrefabs].transform.position.y, 0), Quaternion.identity);
        }

        TrapsAndCoins = GameObject.FindGameObjectsWithTag("TrapCoin");
        respawnPos = xPos;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i <= TrapsAndCoins.Length - 1; i++)
        {
            if (TrapsAndCoins[i].transform.position.x <= destroyPos)
            {
                TrapsAndCoins[i].transform.position = new Vector3(respawnPos, TrapsAndCoins[i].transform.position.y, 0);
            }

            TrapsAndCoins[i].transform.Translate(translatePos * game.SpeedWithBoost);
        }

    }
}


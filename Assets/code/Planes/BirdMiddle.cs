using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMiddle : MonoBehaviour
{
    [SerializeField] Vector2 boarding;
    [SerializeField] Vector2 translatePos;
    bool isFalling;

    private void FixedUpdate()
    {
        if (transform.position.y >= boarding.x || transform.position.y <= boarding.y)
        {
            isFalling = !isFalling;
        }
        if (isFalling)
        {
            transform.Translate(translatePos);
        }
        else
        {
            transform.Translate(-translatePos);
        }
    }

}

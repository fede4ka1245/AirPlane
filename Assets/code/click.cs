using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class click : MonoBehaviour
{
    public bool isClicked = false;

    public void ButtonDown()
    {
        isClicked = true;
    }

    public void ButtonUp()
    {
        isClicked = false;
    }
}

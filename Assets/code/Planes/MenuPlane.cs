using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlane : MonoBehaviour
{
    public int IsOpend;
    public string Name;
    public int Price;
    public string About;

    Save sv;

    void Start()
    {
        openCopter();
        if (PlayerPrefs.HasKey(Name))
        {
            IsOpend = PlayerPrefs.GetInt(Name);
        }
        else
        {
            PlayerPrefs.SetInt(Name, IsOpend);
        }
    }

    void openCopter()
    {
        if (PlayerPrefs.HasKey("Save"))
        {
            sv = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("Save"));
            if (sv.BestScore >= 15000)
                PlayerPrefs.SetInt("Copter", 1);
        }
    }
}

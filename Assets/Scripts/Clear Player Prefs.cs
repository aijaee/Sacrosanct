using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPlayerPrefs : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}

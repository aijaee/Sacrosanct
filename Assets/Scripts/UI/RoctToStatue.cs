using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ROCKTOSTATOO : MonoBehaviour
{
    public GameObject crowStatoo;
    public GameObject crowSaBato;
    void Update()
    {
        if (PlayerPrefs.HasKey("Dialogue_Water"))
        {
            crowSaBato.SetActive(false);
            crowStatoo.SetActive(true);
        }
    }
}

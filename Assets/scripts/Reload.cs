using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reload : MonoBehaviour
{
    public void push()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("reload");
    }
}


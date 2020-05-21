using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMake : MonoBehaviour
{
    public bool hostpush2;
    public void push()
    {
        hostpush2 = true;
        Debug.Log("roomclose");
    }
}

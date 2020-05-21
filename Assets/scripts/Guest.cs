using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guest : MonoBehaviour
{
    public bool guestpush;
    Dropdown dd;
    void Start()
    {
        dd = GameObject.Find("RoomDropdown").GetComponent<Dropdown>();
    }
    
    public void push()
    {
        if (dd.value > 0)
        {
            guestpush = true;
            Debug.Log("guestin");
        }
    }
}
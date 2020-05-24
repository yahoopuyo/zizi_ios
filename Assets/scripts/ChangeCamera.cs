using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeCamera : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject sourceCamera;
    [SerializeField] private GameObject canvas4;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void switchCamera()
    {
        mainCamera.SetActive(!mainCamera.activeSelf);
        sourceCamera.SetActive(!sourceCamera.activeSelf);
        canvas4.SetActive(!canvas4.activeSelf);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switchCamera();
        }
    }
}

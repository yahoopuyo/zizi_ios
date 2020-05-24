using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;
using System;

public class FlickOpenSource : MonoBehaviour
{

    [SerializeField] public FlickGesture flickGesture;
    [SerializeField] private ChangeCamera cc;
    [SerializeField] private OpenSourceOnline oso;
    [SerializeField] private ShowSource os; //for solo
    // Start is called before the first frame update
    void Start()
    {

    }

    //for flick input
    private void OnEnable()
    {
        flickGesture.Flicked += OnFlicked;
    }

    private void OnDisable()
    {
        flickGesture.Flicked -= OnFlicked;
    }

    private void OnFlicked( object sender, EventArgs e )
    {
        if(oso != null) oso.updateSource();
        cc.switchCamera();

    }

    // Update is called once per frame
    void Update()
    {

    }
}

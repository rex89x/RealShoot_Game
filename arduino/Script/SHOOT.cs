using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;

public class SHOOT : MonoBehaviour {

    private SerialPort sp;

    // Use this for initialization
    void Start () {
        sp = new SerialPort("COM4", 9600);
        sp.ReadTimeout = 500;
        sp.Open();
    }
	
	// Update is called once per frame
	void Update () {
        String strRec = sp.ReadLine();
        Debug.Log(strRec);
    }
}

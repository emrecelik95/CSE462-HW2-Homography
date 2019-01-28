using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCameraController : MonoBehaviour {

    private Camera cam;
    private float zSelf;
	void Start () {
        cam = Camera.main;
        zSelf = transform.position.z;
	}
	

	void Update () {
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = zSelf;
        transform.position = pos;
       
	}
}

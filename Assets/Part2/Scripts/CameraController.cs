using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float moveSpeed = 50f;
    public float scrollSpeed = 200;

    public float distanceMin = 1;
    public float distanceMax = 25;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        transform.position += Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        transform.position += Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSpeed;

        cam.orthographicSize = (cam.orthographicSize > distanceMax) ? distanceMax : cam.orthographicSize;
        cam.orthographicSize = (cam.orthographicSize < distanceMin) ? distanceMin : cam.orthographicSize;

    }
}

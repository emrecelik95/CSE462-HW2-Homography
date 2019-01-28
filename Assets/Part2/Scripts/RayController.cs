using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayController : MonoBehaviour {

    public LayerMask raycastLayer;

    private Camera cam;
    private int layerMask;

    private void Awake()
    {
        cam = Camera.main;
        layerMask = raycastLayer.value;
    }

    void Update()
    {    
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, layerMask))
            {
                Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);

                HImage img = hit.transform.root.GetComponent<HImage>();
                HManager.instance.currentImg = img;

                if(img.placeMode)
                    img.CreateAnchor(hit.point);
            }
        }
    }
}

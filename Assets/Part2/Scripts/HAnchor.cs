using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HAnchor : MonoBehaviour {

    public Vector3 pos
    {
        get
        {
            return transform.localPosition;
        }
        set
        {
            transform.localPosition = value;
        }
    }

}

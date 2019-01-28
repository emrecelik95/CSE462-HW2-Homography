using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HImage : MonoBehaviour {

    [SerializeField]
    private HAnchor anchorPrefab;

    public Transform origin;

    public List<HAnchor> anchors;
    public bool placeMode = false;


    private void Awake()
    {
      
    }
    
    public void ResetAllAnchors()
    {
        foreach (HAnchor a in anchors)
            if(a != null)
                Destroy(a.gameObject);

        anchors = new List<HAnchor>();
    }

    public void CreateAnchor(Vector3 worldPose)
    {
        HAnchor a = (Instantiate(anchorPrefab, origin));
        Vector3 pos = origin.InverseTransformPoint(worldPose);
        pos.z = 0;

        a.pos = pos;

        anchors.Add(a);
    }

    public void ReSelectPoints()
    {
        HManager.instance.currentImg = this;
        placeMode = true;

        ResetAllAnchors();
        if (HManager.instance.refImg == this)
            return;

        Transform[] trs = origin.transform.GetComponentsInChildren<Transform>();
        foreach (Transform tr in trs)
            if(tr != origin)
                Destroy(tr.gameObject);
    }

}

using UnityEngine;

public class HManager : MonoBehaviour {

    public static HManager instance;

    public HImage refImg;
    public HImage currentImg; // not using for now
    public HImage[] imgs = null;
    // point count required for homograpgy calculation
    public int pointCountForHmg = 5;

    // augmented 3d game object transform in reference image
    // must be a child of origin in the reference image 
    public Transform augmentedObject;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        imgs = FindObjectsOfType<HImage>();
    }
    
    public void ApplyProjection(HImage img)
    {
        Debug.Log("Applying Projection to image '" + img.name + "'");
        double[,] r = ImgToDoubleArr(refImg);
        double[,] d = ImgToDoubleArr(img);
        double[,] hm = Homography.CalcHomographyMatrix(r, d);


        Vector3 objPoint = augmentedObject.transform.localPosition;
        double[,] p = new double[,] { { objPoint.x }, { objPoint.y }, { 1 } };
        double[,] uv = Homography.CalcProjection(hm, p, true);

        Transform newOrigin = new GameObject("NewOrigin").transform;
        newOrigin.SetParent(img.origin);

        
        //obj.localPosition = new Vector3((float)uv[0, 0], (float)uv[1, 0], 0);


        // project origin
        double[,] o = new double[3, 1] { { 0 }, { 0 }, { 1 } };
        double[,] or = Homography.CalcProjection(hm, o, true);
        newOrigin.localPosition = new Vector3((float)or[0, 0], (float)or[1, 0], 0);

        // find angle
        Vector3 refDir = refImg.origin.transform.position - objPoint;
        Vector3 projDir = newOrigin.localPosition - (new Vector3((float)uv[0, 0], (float)uv[1, 0], 0));
        print("refdir : " + refDir + " , projdir : " + projDir);

        //float zAngle = Vector2.SignedAngle(new Vector2(refDir.x, refDir.y), new Vector2(projDir.x, projDir.y));
        float zAngle = Vector2.SignedAngle(refDir, projDir);
        Debug.Log("Angle : " + zAngle);

        // distance between object and origin
        float dist1 = Mathf.Sqrt(Mathf.Pow((float)(p[0, 0]) , 2) + Mathf.Pow((float)(p[1, 0]), 2));
        // distance between projected object and projected origin
        float dist2 = Mathf.Sqrt(Mathf.Pow((float)(uv[0, 0] - or[0, 0]) , 2) + Mathf.Pow((float)(uv[1, 0] - or[1, 0]), 2));
        float scaleFactor = dist2 / dist1;

        Debug.Log("Scale Factor : " + scaleFactor);


        //newOrigin.localScale *= scaleFactor;
        //newOrigin.rotation = Quaternion.Euler(0, 0, zAngle);

        // projected object
        Transform obj = Instantiate(augmentedObject, img.origin);
        obj.localScale *= scaleFactor;
        obj.RotateAround(newOrigin.transform.position, Vector3.forward, zAngle);
        obj.localPosition = new Vector3((float)uv[0,0], (float)uv[1,0], 0);

    }

    public void ApplyProjectionToAll()
    {
        foreach (HImage img in imgs){
            if (img.anchors.Count == pointCountForHmg && img != refImg)
                ApplyProjection(img);
        }

        ResetPlaceModes();
    }

    public double[,] ImgToDoubleArr(HImage img)
    {
        double[,] arr = new double[5, 2];
        int i = 0;
        foreach (HAnchor a in img.anchors) {
            arr[i, 0] = a.pos.x;
            arr[i, 1] = a.pos.y;

            i++;
        }
        return arr;
    }

    public void ResetPlaceModes()
    {
        foreach (HImage img in imgs)
            img.placeMode = false;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}

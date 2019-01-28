using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHomography : MonoBehaviour {

    public double[,] s = new double[4, 2] { { 1, 2 }, { 5, 3 }, { 4, 10 }, { 1, 6 } };
    public double[,] d = new double[4, 2] { { 1, 2 }, { 5, 3 }, { 4, 10 }, { 1, 6 } };


    private void Start() {
        double[,] hm = Homography.CalcHomographyMatrix(s, d);

        string log = "";

        for (int i = 0; i < hm.GetLength(0); i++)
        {
            for (int j = 0; j < hm.GetLength(1); j++)
                log += hm[i, j] + " ";
            log += "\n";
        }

        Debug.Log("Calculated Homography Matrix : \n\n" + log);
        
        // TEST
        double[,] xy = new double[3, 1] { { s[2,0] }, { s[2,1] } , { 1 } };
        Homography.CalcProjection(hm, xy, true);
        // TEST
        double[,] uv = new double[3, 1] { { d[1, 0] }, { d[1, 1] }, { 1 } };
        Homography.CalcInverseProjection(hm, uv, true);
    }

}

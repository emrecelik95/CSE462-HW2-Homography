using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHomography2 : MonoBehaviour {

    public double[,] s = new double[5, 2] { { 100, 100 }, { 300, 200 }, { 100, 400 }, { 200, 500 }, { 200, 300 } };

    // image 1
    public double[,] d1 = new double[5, 2] { { 755, 706 }, { 1221, 933 }, { 767, 1393 }, { 997, 1624 } , { 994, 1166 } };
    // image 2
    public double[,] d2 = new double[5, 2] { { 740, 508 }, { 1208, 732 }, { 724, 1208 }, { 960, 1448 }, { 968, 968 } };
    // image 3
    public double[,] d3 = new double[5, 2] { { 876, 748 }, { 1376, 980 }, { 924, 1460 }, { 1168, 1664 }, { 1148, 1220 } };

    //  1.4, 1.5, 1.6
    private void Start()
    {
        //  1.4
        ///////////////////////////////////////////////////////////////////
        List<double[,]> P = new List<double[,]>();  // points (x, y)
        List<double[,]> aP = new List<double[,]>(); // actual point correspondences (u, v)
        double[,] hm1 = Homography.CalcHomographyMatrix(s, d1);
        double[,] hm2 = Homography.CalcHomographyMatrix(s, d2);
        double[,] hm3 = Homography.CalcHomographyMatrix(s, d3);
 
        P.Add(new double[3, 1] { { 900 }, { 100 }, { 1 } });
        P.Add(new double[3, 1] { { 800 }, { 300 }, { 1 } });
        P.Add(new double[3, 1] { { 700 }, { 400 }, { 1 } });
        
        /// Image 1 
        Debug.Log("Projecting Image 1");
        aP.Add(new double[3, 1] { { 2612 }, { 681 }, { 1 } });
        aP.Add(new double[3, 1] { { 2379 }, { 1151 }, { 1 } });
        aP.Add(new double[3, 1] { { 2145 }, { 1381 }, { 1 } });

        CalculatePointMatchesAndErrors(hm1, s, d1, P, aP);

        /// Image 2
        Debug.Log("Projecting Image 2");
        aP = new List<double[,]>(); // actual point correspondences (u, v)

        aP.Add(new double[3, 1] { { 2612 }, { 476 }, { 1 } });
        aP.Add(new double[3, 1] { { 2396 }, { 948 }, { 1 } });
        aP.Add(new double[3, 1] { { 2160 }, { 988 }, { 1 } });

        CalculatePointMatchesAndErrors(hm2, s, d2, P, aP);


        /// Image 3 
        Debug.Log("Projecting Image 3");
        aP = new List<double[,]>(); // actual point correspondences (u, v)

        aP.Add(new double[3, 1] { { 2664 }, { 712 }, { 1 } });
        aP.Add(new double[3, 1] { { 2444 }, { 1152 }, { 1 } });
        aP.Add(new double[3, 1] { { 2240 }, { 1376 }, { 1 } });

        CalculatePointMatchesAndErrors(hm3, s, d3, P, aP);

        //  1.5
        /////////////////////////////////////////////////////////////////
        double[,] p1 = new double[3, 1] { { 7.5f }, { 5.5f }, { 1 } };
        double[,] p2 = new double[3, 1] { { 6.3f }, { 3.3f }, { 1 } };
        double[,] p3 = new double[3, 1] { { 0.1f }, { 0.1f }, { 1 } };
            
        Debug.Log("Projection For Image 1");
        Homography.CalcProjection(hm1, p1, true);
        Homography.CalcProjection(hm1, p2, true);
        Homography.CalcProjection(hm1, p3, true);
        Debug.Log("Projection For Image 2");
        Homography.CalcProjection(hm2, p1, true);
        Homography.CalcProjection(hm2, p2, true);
        Homography.CalcProjection(hm2, p3, true);
        Debug.Log("Projection For Image 3");
        Homography.CalcProjection(hm3, p1, true);
        Homography.CalcProjection(hm3, p2, true);
        Homography.CalcProjection(hm3, p3, true);

        //  1.6
        /////////////////////////////////////////////////////////////////
        double[,] i1 = new double[3, 1] { { 500 }, { 400 }, { 1 } };
        double[,] i2 = new double[3, 1] { { 86 }, { 167 }, { 1 } };
        double[,] i3 = new double[3, 1] { { 10 }, { 10 }, { 1 } };

        Debug.Log("Inverse Projection For Image 1");
        Homography.CalcInverseProjection(hm1, i1, true);
        Homography.CalcInverseProjection(hm1, i2, true);
        Homography.CalcInverseProjection(hm1, i3, true);

        Debug.Log("Inverse Projection For Image 2");
        Homography.CalcInverseProjection(hm2, i1, true);
        Homography.CalcInverseProjection(hm2, i2, true);
        Homography.CalcInverseProjection(hm2, i3, true);

        Debug.Log("Inverse Projection For Image 3");
        Homography.CalcInverseProjection(hm3, i1, true);
        Homography.CalcInverseProjection(hm3, i2, true);
        Homography.CalcInverseProjection(hm3, i3, true);

    }

    private void CalculatePointMatchesAndErrors(double[,] hm, double[,] s, double[,] d, List<double[,]> P, List<double[,]> actualP)
    {
        string log = "";

        for (int i = 0; i < hm.GetLength(0); i++)
        {
            for (int j = 0; j < hm.GetLength(1); j++)
                log += hm[i, j] + " ";
            log += "\n";
        }

        Debug.Log("Calculated Homography Matrix : \n\n" + log);
        Debug.Log("Calculating Projection of points...");

        int k = 0;
        foreach (double[,] xy in P)
        {
            double[,] uv = actualP[k++];
            var res = Homography.CalcProjection(hm, xy, true);     // projection
            Debug.Log("Error : %" + CalcProjectionError(res, uv)); // error
        }
    }

    // Percentage
    public float CalcProjectionError(double[,] result, double[,] actual)
    {
        var x1 = result[0, 0];
        var x2 = actual[0, 0];

        var y1 = result[1, 0];
        var y2 = actual[1, 0];

        float resDif = Mathf.Sqrt(Mathf.Pow((float)(x1), 2) + Mathf.Pow((float)(y1), 2));
        float actualDif = Mathf.Sqrt(Mathf.Pow((float)(x2), 2) + Mathf.Pow((float)(y2), 2));

        return Mathf.Abs((actualDif - resDif) / actualDif * 100);
    }

}

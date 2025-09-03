using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;
    void Awake()
    {
        points = new Transform[transform.childCount + 1];
        for(int i = 0; i < points.Length - 1; i++)
        {
            points[i] = transform.GetChild(i);
        }
        points[points.Length - 1] = GameObject.Find("End").transform;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using Pcx;

public class closest_point : MonoBehaviour
{
    [SerializeField] PointCloudData _sourceData = null;
    // [SerializeField] PointCloudData _sourceData1 = null;
    // [SerializeField] ComputeShader _computeShader = null;
    public GameObject pointcloud;

    private Vector3 pointF;
    public float distanceF1;
    public float distanceF2;
    public float distanceF3;
    public float distanceF4;

    public Vector3 closest_p1;
    public Vector3 closest_p2;
    public Vector3 closest_p3;
    public Vector3 closest_p4;

    public float distance1;
    public float distance2;
    public float distance3;
    public float distance4;
    public Transform kp1;
    public Transform kp2;
    public Transform kp3;
    public Transform kp4;

   

    ComputeBuffer _pointBuffer;

    // void OnDisable()
    // {
    //     if (_pointBuffer != null)
    //     {
    //         _pointBuffer.Release();
    //         _pointBuffer = null;
    //     }
    // }
    void Start()
    {
        pointF = Vector3.positiveInfinity;

    }

    // Update is called once per frame
    void Update()
    {
        if (_sourceData == null) return;

      
        // print("dta?"+pointcloud.transform.TransformPoint(_sourceData.pointPos[1].position));

        distanceF1 = Vector3.Distance(kp1.transform.position,pointF);
        distanceF2 = Vector3.Distance(kp2.transform.position,pointF);
        distanceF3 = Vector3.Distance(kp3.transform.position,pointF);
        distanceF4 = Vector3.Distance(kp4.transform.position,pointF);

        for (int i = 0; i < _sourceData.pointPos.Length; i++)
            {
                // print("suoyin"+i);
                distance1 = Vector3.Distance(pointcloud.transform.TransformPoint(_sourceData.pointPos[i].position), kp1.transform.position);
                distance2 = Vector3.Distance(pointcloud.transform.TransformPoint(_sourceData.pointPos[i].position), kp2.transform.position);
                distance3 = Vector3.Distance(pointcloud.transform.TransformPoint(_sourceData.pointPos[i].position), kp3.transform.position);
                distance4 = Vector3.Distance(pointcloud.transform.TransformPoint(_sourceData.pointPos[i].position), kp4.transform.position);
                // print("bianli"+pointcloud.transform.TransformPoint(_sourceData.pointPos[1].position));
                // print("juli"+distance);
                
                // print("FFFFF"+distanceF);
                if (distance1 < distanceF1){
                    distanceF1=distance1;
                    closest_p1 = pointcloud.transform.TransformPoint(_sourceData.pointPos[i].position);
                }
                 if (distance2 < distanceF2){
                    distanceF2=distance2;
                    closest_p2 = pointcloud.transform.TransformPoint(_sourceData.pointPos[i].position);
                }
                 if (distance3 < distanceF3){
                    distanceF3=distance3;
                    closest_p3 = pointcloud.transform.TransformPoint(_sourceData.pointPos[i].position);
                }
                 if (distance4 < distanceF4){
                    distanceF4=distance4;
                    closest_p4 = pointcloud.transform.TransformPoint(_sourceData.pointPos[i].position);
                }
                
            }

        
        // ClosestObject();
        
    }

    // private void print(Action<Array> getData)
    // {
    //     throw new NotImplementedException();
    // }
    // void ClosestObject()
    // {
    //     distanceF = 10000;

    //     int j = 0;
    //     foreach (Transform target in Targets.transform)
    //     {
    //         Vector3[] verts = (Vector3[]) vertex[j].Clone();
    //         Debug.Log("YOUYOUYOUYOUYOUYOUhhhhhhhhhhhhh"+verts.Length);



    //         for (int i = 0; i < verts.Length; i++)
    //         {
    //             verts[i] = target.TransformPoint(verts[i]);
    //             Debug.Log(i);
    //            float distance = Vector3.Distance(transform.position, verts[i]);
    //             Debug.Log(distance);
    //            if(distance<distanceF){
    //             distanceF= distance;

    //            }

    //         }

    //         j++;
    //     }





    // }
}

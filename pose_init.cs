using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;

public class pose_init : MonoBehaviour
{
    pose_init_sub pose;
    private bool isMessageReceived;

    // Start is called before the first frame update
    void Start()
    {
        pose = GetComponent<pose_init_sub>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMessageReceived){
            // // 
                transform.localPosition = pose.position;
                transform.localRotation = pose.rotation;
                if (transform.localPosition.x!=0f)
                    isMessageReceived = true;
            }
        
    }
}

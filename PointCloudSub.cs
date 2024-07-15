using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
public class PointCloudSub : UnitySubscriber<MessageTypes.Sensor.PointCloud2>
{
    private bool isMessageReceived;
    protected override void Start()
    {
			base.Start();
	}
    private void Update()
    {
        if (isMessageReceived)
            ProcessMessage();
    }
    protected override void ReceiveMessage(MessageTypes.Sensor.PointCloud2 message)
    {
        isMessageReceived = true;
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    private void ProcessMessage()
        {
            // print("SHOUDAOLE");
        }
}
}

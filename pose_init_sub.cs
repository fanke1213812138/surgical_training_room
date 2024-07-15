/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class pose_init_sub : UnitySubscriber<MessageTypes.Geometry.PoseStamped>
    {
        public Transform SetTransform;

        public Vector3 position;
        public Quaternion rotation;
        private bool isMessageReceived;

        protected override void Start()
        {
			base.Start();
            // position = None;
            // isMessageReceived = true;
            
            
		}
		
        private void Update()
        {
            // if (isMessageReceived){
            // // 
                // ProcessMessage();
                // isMessageReceived = false;
            // }
        }

        protected override void ReceiveMessage(MessageTypes.Geometry.PoseStamped message)
        {
            
            position = GetPosition(message).Ros2Unity();
            rotation = GetRotation(message).Ros2Unity();
            // print("shoudao?"+isMessageReceived);
            // if (!isMessageReceived){
            //     // position/=1000f;
            //     SetTransform.localPosition = new Vector3(position.x, position.y, position.z);
            //     // PublishedTransform.localRotation = rotation;
            //    print("zheli?"+position);
            //     ProcessMessage();
            //      isMessageReceived=true;
            // }
            
        }

        private void ProcessMessage()
        { 
            // PublishedTransform.localPosition = position;
            // PublishedTransform.localRotation = rotation;
            // print("init"+PublishedTransform.localPosition);
            // print("init"+isMessageReceived);
        }

        private Vector3 GetPosition(MessageTypes.Geometry.PoseStamped message)
        {
            return new Vector3(
                (float)message.pose.position.x,
                (float)message.pose.position.y,
                (float)message.pose.position.z);
        }

        private Quaternion GetRotation(MessageTypes.Geometry.PoseStamped message)
        {
            return new Quaternion(
                (float)message.pose.orientation.x,
                (float)message.pose.orientation.y,
                (float)message.pose.orientation.z,
                (float)message.pose.orientation.w);
        }
    }
}
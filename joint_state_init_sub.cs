/*
© Siemens AG, 2017-2019
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

namespace RosSharp.RosBridgeClient
{
    public class joint_state_init_sub: UnitySubscriber<MessageTypes.Sensor.JointState>
    {
        public GameObject psm;
        public GameObject JS_pub;
        public List<string> JointNames;
        public List<JointStateWriter> JointStateWriters;
        private bool isMessageReceived;
        // public List<float> JS;


        protected override void ReceiveMessage(MessageTypes.Sensor.JointState message)
        {
            int index;
            if (!isMessageReceived){
                print("JS"+isMessageReceived);
                    for (int i = 0; i < message.name.Length; i++)
                    {
                        index = JointNames.IndexOf(message.name[i]);
                        if (index != -1)
                            JointStateWriters[index].Write((float) message.position[i]);
                    }
                    isMessageReceived = true;
                    // psm.enabled = true;
                    psm.GetComponent<psm_control>().enabled = true; 
                    print("youmeiyou?");
                    JS_pub.GetComponent<JointStatePublisher>().enabled = true;
                    // JS_pub.enabled= true;
            }
        }

       
    }
}


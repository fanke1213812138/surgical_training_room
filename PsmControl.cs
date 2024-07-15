using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosSharp.Urdf;
using RosSharp.RosBridgeClient;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using DVRK;

public class PsmControl : Agent
{
    // public List<JointStateWriter> JSwriter;
    public List<JointStateReader> JSreader;
    // public PrismaticStateWriter prmtcwriter;
    // public List<Transform> joints; 
    public List<URDFJoint> joint;
    public List<float> js_pos;

     public Transform kp1, kp2, kp3, kp4, plane, ECM, psm, targ, targ_head;

    public Vector3 psm_tip, dis;
    Quaternion joint1_init,joint2_init,joint4_init,joint5_init,joint6_init;
    Vector3 joint3_init, ecm_init_rot,ecm_init_pos;
   
      public float j1_a, j2_a, j3_a, j4_a, j5_a, j6_a, distanceToTarget, reward;
      public int j1,j2, j3, j4, j5, j6;
    public int step;
    // Start is called before the first frame update
    void Start()
    {
        js_pos = new List<float>{0f,0f,0f,0f,0f,0f};
        psm_tip  = new Vector3((kp3.transform.position.x+kp4.transform.position.x)/2f,
                                (kp3.transform.position.y+kp4.transform.position.y)/2f,
                                (kp3.transform.position.z+kp4.transform.position.z)/2f );
        // predist = Vector3.Distance(ECM.transform.InverseTransformPoint(psm_tip), ECM.transform.InverseTransformPoint(targ.transform.position));
        ecm_init_rot = ECM.transform.localEulerAngles;
        ecm_init_pos = ECM.transform.localPosition;
    //    
        
    }
    public override void OnEpisodeBegin()
    {
       step=0;
        joint[0].SetJointValue(Random.Range(-50f,80f));
        joint[1].SetJointValue(Random.Range(-30f,30f));
        joint[2].SetJointValue(Random.Range(0f,0.1f));
        joint[3].SetJointValue(Random.Range(-50f,50f));
        joint[4].SetJointValue(Random.Range(-50f,50f));
        joint[5].SetJointValue(Random.Range(-50f,50f));
    }

       public override void CollectObservations(VectorSensor sensor)
{
   

        sensor.AddObservation(psm_tip);//3
        sensor.AddObservation(targ.transform.position);//3
        sensor.AddObservation(distanceToTarget);//1


}

public override void OnActionReceived(ActionBuffers actionBuffers)
{
    step+=1;
     var disAct = actionBuffers.DiscreteActions;

            j1 = disAct[0];
            j2 = disAct[1];
            j3 = disAct[2];
            j4 = disAct[3];
            j5 = disAct[4];
            j6 = disAct[5];
        
        switch (j1)
        {
            case 0:
                   j1_a=0f;
                break;
            case 1:
               j1_a = -1f;
                break;
            case 2:
               j1_a = 1f;
                break;
        }

        switch (j2)
        {
            case 0:
                 j2_a=0f;
                break;
            case 1:
                j2_a=-1f;
                break;
            case 2:
                j2_a=1f;
                break;
        }

        switch (j3)
        {
            case 0:
                 j3_a=0f;
                break;
            case 1:
                j3_a=-1f;
                break;
            case 2:
                j3_a=1f;
                break;
        }

        switch (j4)
        {
            case 0:
                 j4_a=0f;
                break;
            case 1:
                j4_a = -1f;
                break;
            case 2:
                j4_a = 1f;
                break;
        }

        switch (j5)
        {
            case 0:
                 j5_a=0f;
                break;
            case 1:
                j5_a=-1f;
                break;
            case 2:
                j5_a=1f;
                break;
        }

        switch (j6)
        {
            case 0:
                 j6_a=0f;
                break;
            case 1:
                j6_a=-1f;
                break;
            case 2:
                j6_a=1f;
                break;
        }
        
        joint[0].SetJointValue(joint[0].currentJointValue+j1_a*1f);
        joint[1].SetJointValue(joint[1].currentJointValue+j2_a*1f);
        joint[2].SetJointValue(joint[2].currentJointValue+j3_a*0.001f);
        joint[3].SetJointValue(joint[3].currentJointValue+j4_a*1f);
        joint[4].SetJointValue(joint[4].currentJointValue+j5_a*1f);
        joint[5].SetJointValue(joint[5].currentJointValue+j6_a*1f);
        // joint[0].SetJointValue(joint[0].currentJointValue+1f);
        // joint[1].SetJointValue(joint[1].currentJointValue+1f);
        // joint[2].SetJointValue(joint[2].currentJointValue+0.001f);
        // joint[3].SetJointValue(joint[3].currentJointValue+1f);
        // joint[4].SetJointValue(joint[4].currentJointValue+1f);
        // joint[5].SetJointValue(joint[5].currentJointValue+1f);

        

        distanceToTarget = Vector3.Distance(psm_tip, targ.transform.position);
      
        reward =1f-distanceToTarget;
        // print("reward"+reward);

       
            AddReward(reward);
            AddReward(-5f/MaxStep);
        if (distanceToTarget < 0.004f)
        {
            AddReward(20.0f);
            // JSwriter[0].Write(0f);
            // JSwriter[1].Write(0f);
            
            EndEpisode();
            print("jieshu2");
        }
            
        //    predist = distanceToTarget;
        

    if (
            js_pos[0]>1.57 || js_pos[0]<-1.57f
            || js_pos[1]>0.785f ||js_pos[1]<-0.785f
            || js_pos[2]>0.18f ||js_pos[2]<-0.05f
            || js_pos[3]>1.57f ||js_pos[3]<-1.57f
            || js_pos[4]>1.39f ||js_pos[4]<-1.39f
            || js_pos[5]>1.22f ||js_pos[5]<-1.74f
        )
        {
            AddReward(-1f);
            //  reset =true;
            EndEpisode();
            print("stop1");
            //  print("超越极限,,"+js_pos[0]);
            // print("超越极限1,,"+js_pos[1]);
            // print("超越极限2,,"+js_pos[2]);
            // print("超越极限3,,"+js_pos[3]);
            // print("超越极限4,"+js_pos[4]);
            // print("超越极限5,,"+js_pos[5]);
        
            
            }

}

    // Update is called once per frame
    void Update()
    {
        psm_tip  = new Vector3((kp3.transform.position.x+kp4.transform.position.x)/2f,
                                (kp3.transform.position.y+kp4.transform.position.y)/2f,
                                (kp3.transform.position.z+kp4.transform.position.z)/2f );
                                Debug.DrawLine(psm_tip,targ.transform.position,Color.yellow);
         
        JSreader[0].Read(
                out string name,
                out float position,
                out float velocity,
                out float effort);
        js_pos[0] = position;
        JSreader[1].Read(
                out string name1,
                out float position1,
                out float velocity1,
                out float effort1);
        js_pos[1] = position1;
        JSreader[2].Read(
                out string name2,
                out float position2,
                out float velocity2,
                out float effort2);
        js_pos[2] = position2;
        JSreader[3].Read(
                out string name3,
                out float position3,
                out float velocity3,
                out float effort3);
        js_pos[3] = position3;
        JSreader[4].Read(
                out string name4,
                out float position4,
                out float velocity4,
                out float effort4);
        js_pos[4] = position4;
        JSreader[5].Read(
                out string name5,
                out float position5,
                out float velocity5,
                out float effort5);
        js_pos[5] = position5;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosSharp.Urdf;
using RosSharp.RosBridgeClient;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class psm_agent_control: Agent
{
    public List<UrdfJoint> urdfJoint;
    public List<JointStateWriter> JSwriter;
    public List<JointStateReader> JSreader;
    // public List<Transform> joints; 
    public Transform kp1; 
    public Transform kp2; 
    public Transform kp3;
    public Transform kp4;

    public string FrameId = "Unity";

    public GameObject ECM;

    public GameObject psm;

    public Vector3 psm_tip;
    public Transform psm_base;

    public GameObject targ;
    public GameObject targ_head;
    public GameObject plane;

    public Transform limit1;
    public Transform limit2;

    // public closest_point closest_point;

    public float distanceToTarget;
    float predist;
    public float dis_kp2Topcd;
    public float dis_kp3Topcd;
    public float dis_kp4Topcd;
    public float reward;
    public float reward_h;
    public float o_reward;
    public float o_reward2;
    public float o_reward3;
    public float reward_w;
    public int j1;
    public int j2;
    public int j3;
    public int j4;
    public int j5;
    public int j6;
 
    List<float> js_pos;

    float distance_PC;
    Quaternion quaternion_psm;

    public float step;
    public float j1_a, j2_a, j3_a, j4_a, j5_a, j6_a;
    // Start is called before the first frame update——

    Vector3 ecm_init_pos;
    Vector3 ecm_init_rot;
    void Start()
    {
        js_pos = new List<float>{0f,0f,0f,0f,0f,0f};
        // predist = Vector3.Distance();
        psm_tip  = new Vector3((kp3.transform.position.x+kp4.transform.position.x)/2f,
                                (kp3.transform.position.y+kp4.transform.position.y)/2f,
                                (kp3.transform.position.z+kp4.transform.position.z)/2f );
        predist = Vector3.Distance(ECM.transform.InverseTransformPoint(psm_tip), ECM.transform.InverseTransformPoint(targ.transform.position));
        ecm_init_rot = ECM.transform.localEulerAngles;
        ecm_init_pos = ECM.transform.localPosition;
        
    
    }

    public override void OnEpisodeBegin()
    {
        step=0f;
        // joint——state
        JSwriter[0].Write(Random.Range(-1.2f,1.2f));
        JSwriter[1].Write(Random.Range(-0.7f,0.7f));
        JSwriter[2].Write(Random.Range(0f,0.1f));
        JSwriter[3].Write(Random.Range(-1f,1f));
        JSwriter[4].Write(Random.Range(-0.3f,0.3f));
        JSwriter[5].Write(Random.Range(-0.3f,0.3f));
       

        JSwriter[6].Write(-0.8f);
        JSwriter[7].Write(0.8f);
      
        




    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // for (int i = 0; i < JSreader.Count; i++)
        // js_pos[i]= urdfJoint[0].GetPosition();
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
        sensor.AddObservation(js_pos[0]); //6
        sensor.AddObservation(js_pos[1]);
        sensor.AddObservation(js_pos[2]);
        sensor.AddObservation(js_pos[3]);
        sensor.AddObservation(js_pos[4]);
        sensor.AddObservation(js_pos[5]);
     
        
        // psm_tip
        sensor.AddObservation(ECM.transform.InverseTransformPoint(psm_tip));//3
      
        sensor.AddObservation(ECM.transform.InverseTransformPoint(targ.transform.position));//3
        
        sensor.AddObservation(distanceToTarget);//1
        

    
        


    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    { step= step+1f;
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
        JSwriter[0].Write(js_pos[0]);
        JSwriter[1].Write(js_pos[1]);
     
        JSwriter[2].Write(js_pos[2]);
        
        JSwriter[3].Write(js_pos[3]);
        JSwriter[4].Write(js_pos[4]);
        JSwriter[5].Write(js_pos[5]);
        
        // JSwriter[0].Write(js_pos[0]+0.001f);
        // JSwriter[1].Write(js_pos[1]+0.001f);
     
        // JSwriter[2].Write(js_pos[2]+0.0005f);
        
        // JSwriter[3].Write(js_pos[3]+0.001f);
        // JSwriter[4].Write(js_pos[4]+0.001f);
        // JSwriter[5].Write(js_pos[5]+0.001f);

        print("action333la333la"+(js_pos[2]));

       JSreader[2].Read(
                out string name22,
                out float position22,
                out float velocity22,
                out float effort22);
        js_pos[2] = position22;
        print("action333la"+(position22));

        // psm_tip  = new Vector3((kp3.transform.position.x+kp4.transform.position.x)/2f,
        //                         (kp3.transform.position.y+kp4.transform.position.y)/2f,
        //                         (kp3.transform.position.z+kp4.transform.position.z)/2f );
        // distanceToTarget = Vector3.Distance(ECM.transform.InverseTransformPoint(psm_tip), ECM.transform.InverseTransformPoint(targ.transform.position));
    
        //  Debug.DrawLine(psm_tip,targ.transform.position,Color.yellow);
        
  
        // reward =-distanceToTarget;  
       
      
        // if (distanceToTarget < 0.004f)
        // {
        //     AddReward(1.0f);
        //     // JSwriter[0].Write(0f);
        //     // JSwriter[1].Write(0f);
            
        //     EndEpisode();
        //     print("stop3");
        // }
        // // else{
        //     AddReward(reward);
        //     AddReward(-5f/MaxStep);
        // // }
    
        // if (plane.transform.InverseTransformPoint(psm_tip).y<plane.transform.InverseTransformPoint(targ.transform.position).y)
        // {
        //     AddReward(-1.0f);
        //     EndEpisode();
        //    print("stop2");
       

        // }

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
            EndEpisode();
            print("stop1");
             print("超越极限,,"+js_pos[0]);
            print("超越极限1,,"+js_pos[1]);
            print("超越极限2,,"+js_pos[2]);
            print("超越极限3,,"+js_pos[3]);
            print("超越极限4,"+js_pos[4]);
            print("超越极限5,,"+js_pos[5]);
        
            
            }
    }

    void Update()
    {
        psm_base.localEulerAngles=new Vector3(0f, 180f, 0f);
        
      
        
      

           
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosSharp.Urdf;
using RosSharp.RosBridgeClient;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class psm_control: Agent
{
    public List<UrdfJoint> urdfJoint;
    public List<JointStateWriter> JSwriter;
    public List<Transform> joints; 
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

    public closest_point closest_point;

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
    float j1_a, j2_a, j3_a, j4_a, j5_a, j6_a;
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
        //joint——state
        joints[0].localEulerAngles = new Vector3(Random.Range(-60f, -120f), 0f, -90f); 
        joints[1].localEulerAngles = new Vector3(Random.Range(-115f, -65f),0f , 90f); 
        joints[2].localPosition = new Vector3(Random.Range(-0.38f, -0.3f), 0f, 0f); 
        joints[3].localEulerAngles = new Vector3(0f, Random.Range(-60f, 60f), 0f); 
        joints[4].localEulerAngles = new Vector3(Random.Range(-30f, -150f), 0f, 90f); 
        joints[5].localEulerAngles = new Vector3(Random.Range(-30f, -150f), 0f, 90f); 

        JSwriter[0].Write(-0.8f);
        JSwriter[1].Write(0.8f);
        // psm-whole  can be the same with real world;
        // psm.transform.localEulerAngles = new Vector3(Random.Range(10f, 30f), Random.Range(-10f, -40f),90f);
        // psm.transform.localPosition = new Vector3(Random.Range(-2f, -3f),Random.Range(1f, 1.5f),Random.Range(0f, -0.5f));
        // quaternion_psm = Quaternion.Euler(transform.eulerAngles);
        // ECM   needs some randomization
        ECM.transform.localEulerAngles = new Vector3(ecm_init_rot.x+Random.Range(-20f, 20f), ecm_init_rot.y+Random.Range(-20f, 20f),ecm_init_rot.z+Random.Range(-20f, 20f));
        ECM.transform.localPosition = new Vector3(ecm_init_pos.x+Random.Range(-0.003f, 0.003f),ecm_init_pos.y+Random.Range(-0.0031f,0.003f),ecm_init_pos.z+Random.Range(-0.0031f,0.003f));


        //targ
        targ_head.transform.localPosition = new Vector3(Random.Range(-0.11f, 0.18f),Random.Range(0.165f, 0.23f),Random.Range(-0.2f, 0.14f));





    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //GET jointstates
        for (int i = 0; i < urdfJoint.Count; i++)
        js_pos[i]= urdfJoint[0].GetPosition();
        // joint_state
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

        //obstacles
        // sensor.AddObservation(ECM.transform.InverseTransformPoint(kp2.transform.position));//3
        // sensor.AddObservation(ECM.transform.InverseTransformPoint(closest_point.closest_p2));//3
        // sensor.AddObservation(dis_kp2Topcd);//1
        // sensor.AddObservation(ECM.transform.InverseTransformPoint(kp3.transform.position));//3
        // sensor.AddObservation(ECM.transform.InverseTransformPoint(closest_point.closest_p3));//3
        // sensor.AddObservation(dis_kp3Topcd);//1
        // sensor.AddObservation(ECM.transform.InverseTransformPoint(kp4.transform.position));//3
        // sensor.AddObservation(ECM.transform.InverseTransformPoint(closest_point.closest_p4));//3       
        // sensor.AddObservation(dis_kp4Topcd);//1
    
        // sensor.AddObservation(distanceToTarget);//1
        


    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    { step= step+1f;
        var continuousActions = actionBuffers.ContinuousActions;
        // var i = -1;
        j1_a = Mathf.Clamp(continuousActions[0],-1f,1f);
        j2_a = Mathf.Clamp(continuousActions[1],-1f,1f);
        j3_a = Mathf.Clamp(continuousActions[2],-1f,1f);
        j4_a = Mathf.Clamp(continuousActions[3],-1f,1f);
        j5_a = Mathf.Clamp(continuousActions[4],-1f,1f);
        j6_a = Mathf.Clamp(continuousActions[5],-1f,1f);
        // Pick a new target joint rotation
        // var disAct = actionBuffers.DiscreteActions;

        //     j1 = disAct[0];
        //     j2 = disAct[1];
        //     j3 = disAct[2];
        //     j4 = disAct[3];
        //     j5 = disAct[4];
        //     j6 = disAct[5];
        
        // switch (j1)
        // {
        //     case 0:
        //            j1_a=0f;
        //         break;
        //     case 1:
        //        j1_a = -1f;
        //         break;
        //     case 2:
        //        j1_a = 1f;
        //         break;
        // }

        // switch (j2)
        // {
        //     case 0:
        //          j2_a=0f;
        //         break;
        //     case 1:
        //         j2_a=-1f;
        //         break;
        //     case 2:
        //         j2_a=1f;
        //         break;
        // }

        // switch (j3)
        // {
        //     case 0:
        //          j3_a=0f;
        //         break;
        //     case 1:
        //         j3_a=-1f;
        //         break;
        //     case 2:
        //         j3_a=1f;
        //         break;
        // }

        // switch (j4)
        // {
        //     case 0:
        //          j4_a=0f;
        //         break;
        //     case 1:
        //         j4_a = -1f;
        //         break;
        //     case 2:
        //         j4_a = 1f;
        //         break;
        // }

        // switch (j5)
        // {
        //     case 0:
        //          j5_a=0f;
        //         break;
        //     case 1:
        //         j5_a=-1f;
        //         break;
        //     case 2:
        //         j5_a=1f;
        //         break;
        // }

        // switch (j6)
        // {
        //     case 0:
        //          j6_a=0f;
        //         break;
        //     case 1:
        //         j6_a=-1f;
        //         break;
        //     case 2:
        //         j6_a=1f;
        //         break;
        // }
        float maxdis= Vector3.Distance(ECM.transform.InverseTransformPoint(limit1.position),ECM.transform.InverseTransformPoint(limit2.position));
        psm_tip  = new Vector3((kp3.transform.position.x+kp4.transform.position.x)/2f,
                                (kp3.transform.position.y+kp4.transform.position.y)/2f,
                                (kp3.transform.position.z+kp4.transform.position.z)/2f );
        // predist = Vector3.Distance(ECM.transform.InverseTransformPoint(psm_tip), ECM.transform.InverseTransformPoint(targ.transform.position));
        // urdfJoint[0].UpdateJointState(j1_a*0.001f);
        // urdfJoint[1].UpdateJointState(j2_a*0.001f);
        // urdfJoint[2].UpdateJointState(j3_a*0.0002f);
        // urdfJoint[3].UpdateJointState(j4_a*0.002f);
        // urdfJoint[4].UpdateJointState(j5_a*0.002f);
        // urdfJoint[5].UpdateJointState(j5_a*0.002f); 
        urdfJoint[0].UpdateJointState(j1_a*0.0019f);
        urdfJoint[1].UpdateJointState(j2_a*0.0019f);
        urdfJoint[2].UpdateJointState(j3_a*0.00019f);
        urdfJoint[3].UpdateJointState(j4_a*0.0019f);
        urdfJoint[4].UpdateJointState(j5_a*0.0019f);
        urdfJoint[5].UpdateJointState(j6_a*0.0019f); 
        distanceToTarget = Vector3.Distance(ECM.transform.InverseTransformPoint(psm_tip), ECM.transform.InverseTransformPoint(targ.transform.position));
        // reward = (predist - distanceToTarget)*1000f;
        // reward = 1f-10f*distanceToTarget/maxdis;
        

        // tip1 and tip2 horizontal
        reward_h= -Mathf.Pow((plane.transform.InverseTransformPoint(kp3.position).y- plane.transform.InverseTransformPoint(kp4.position).y),2f)*100f;
        // print("kp3.y"+plane.transform.InverseTransformPoint(kp3.position));
        // print("kp4.y"+plane.transform.InverseTransformPoint(kp4.position));
        
    // Update is called once per frame

        dis_kp2Topcd = Vector3.Distance(ECM.transform.InverseTransformPoint(kp2.transform.position), ECM.transform.InverseTransformPoint(closest_point.closest_p2));
        dis_kp3Topcd = Vector3.Distance(ECM.transform.InverseTransformPoint(kp2.transform.position), ECM.transform.InverseTransformPoint(closest_point.closest_p3));
        dis_kp4Topcd = Vector3.Distance(ECM.transform.InverseTransformPoint(kp2.transform.position), ECM.transform.InverseTransformPoint(closest_point.closest_p4));

         Debug.DrawLine(psm_tip,targ.transform.position,Color.yellow);
         Debug.DrawLine(kp2.position,closest_point.closest_p2,Color.magenta);
         Debug.DrawLine(kp3.position,closest_point.closest_p3,Color.magenta);
         Debug.DrawLine(kp4.position,closest_point.closest_p4,Color.magenta);
  
        reward =-2f*distanceToTarget/maxdis;  
        // reward = -5*Mathf.Pow(distanceToTarget,2f);
        o_reward = 0.5f*(1f-(100f*dis_kp2Topcd-1f)/Mathf.Sqrt(0.005f+Mathf.Pow((100f*dis_kp2Topcd-1f),2f)));
        o_reward2 = 0.5f*(1f-(100f*dis_kp3Topcd-1f)/Mathf.Sqrt(0.005f+Mathf.Pow((100f*dis_kp3Topcd-1f),2f)));
        o_reward3 = 0.5f*(1f-(100f*dis_kp4Topcd-1f)/Mathf.Sqrt(0.005f+Mathf.Pow((100f*dis_kp4Topcd-1f),2f)));

        // reward_w=reward;//- 0.2f*step/5000f;// - 0.9f*o_reward;//  ;
// 
        // AddReward(reward_w);
        // AddReward(-5f/MaxStep);

        // print(plane.transform.InverseTransformPoint(psm_tip.transform.position));
        // AddReward();
      
        if (distanceToTarget < 0.004f)
        {
            AddReward(100.0f);
            JSwriter[0].Write(0f);
            JSwriter[1].Write(0f);
            
            EndEpisode();
            print("jieshu2");
        }else{
            AddReward(reward*1f-0.001f);
            AddReward(-5f/MaxStep);
           predist = distanceToTarget;
        }
        // else{
        //     AddReward(-5f/MaxStep);
        //     predist = distanceToTarget;
        // }

        if (plane.transform.InverseTransformPoint(psm_tip).y<plane.transform.InverseTransformPoint(targ.transform.position).y)
        {
            AddReward(-10.0f);
            EndEpisode();
            print("jieshu1");
            // print("jieshu2"+plane.transform.InverseTransformPoint(psm_tip).y);

        }

        //  if (reward_h > -0.015f)
        // {
        //     // AddReward(0.010f);
            
        //     // EndEpisode();
        //     // print("jieshu1");
        // }else{
            // AddReward(reward_h);
        // }

         if (dis_kp2Topcd < 0.01f || dis_kp3Topcd < 0.01f||dis_kp4Topcd < 0.01f)
        {
            // AddReward(-10f);
            
            // EndEpisode();
            // print("jieshu1");
        }else{
            // AddReward((-o_reward-o_reward2-o_reward3)*0.3f);

        }

     
        
    }

    void Update()
    {
        psm_base.localEulerAngles=new Vector3(0f, 180f, 0f);
        
      
        if (
            joints[0].localEulerAngles.x>359f || joints[0].localEulerAngles.x<181f
            || joints[1].localEulerAngles.x>315f ||joints[1].localEulerAngles.x<225f
            || joints[2].localPosition.x>-0.215f ||joints[2].localPosition.x<-0.44f
            // || joints[3].localEulerAngles.y>89f ||joints[3].localEulerAngles.y<271f
            || joints[4].localEulerAngles.x>330f ||joints[4].localEulerAngles.x<210f
            || joints[5].localEulerAngles.x>330f ||joints[5].localEulerAngles.x<210f
        )
        {
            AddReward(-10f);
            EndEpisode();
            print("超越极限"+joints[0].localEulerAngles.x);
            print("超越极限1"+joints[1].localEulerAngles.x);
            print("超越极限2"+joints[2].localPosition.x);
            print("超越极限3"+joints[4].localEulerAngles.x);
            print("超越极限4"+joints[5].localEulerAngles.x);
            }

    }

}

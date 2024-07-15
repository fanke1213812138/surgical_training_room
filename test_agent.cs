using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RosSharp.Urdf;
using RosSharp.RosBridgeClient;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class test_agent: Agent
{
    

    public GameObject targ;
   

    // public closest_point closest_point;

    public float distanceToTarget;
    
    public float reward;
    
    public int j1;
    public int j2;
    public int j3;
    public int j4;
    public int j5;
    public int j6;
 
    List<float> js_pos;

    
    public float step;
    public float j1_a, j2_a, j3_a, j4_a, j5_a, j6_a;
    // Start is called before the first frame update——

    Vector3 ecm_init_pos;
    Vector3 ecm_init_rot;
    void Start()
    {
        js_pos = new List<float>{0f,0f,0f,0f,0f,0f};
        // predist = Vector3.Distance();
      
        ecm_init_rot = transform.localEulerAngles;
        ecm_init_pos = transform.localPosition;
        
    
    }

    public override void OnEpisodeBegin()
    {
        step=0f;
        transform.localPosition = ecm_init_pos;
      
        




    }

    public override void CollectObservations(VectorSensor sensor)
    {
       
        // sensor.AddObservation(js_pos[0]); //6
        // sensor.AddObservation(js_pos[1]);
        // sensor.AddObservation(js_pos[2]);
        // sensor.AddObservation(js_pos[3]);
        // sensor.AddObservation(js_pos[4]);
        // sensor.AddObservation(js_pos[5]);
     
        
        // psm_tip
        sensor.AddObservation( transform.localPosition);//3
      
        sensor.AddObservation(targ.transform.localPosition);//3
        
        sensor.AddObservation(distanceToTarget);//1
        

    
        


    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    { step= step+1f;
        var disAct = actionBuffers.DiscreteActions;

            j1 = disAct[0];
            j2 = disAct[1];
            j3 = disAct[2];
            // j4 = disAct[3];
            // j5 = disAct[4];
            // j6 = disAct[5];
        
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

        transform.localPosition = new Vector3(transform.localPosition.x+j1_a*0.02f,transform.localPosition.y+j2_a*0.02f,transform.localPosition.z+j3_a*0.02f);
        
       
        
       
        distanceToTarget = Vector3.Distance(transform.localPosition, targ.transform.localPosition);
    

       

         Debug.DrawLine(transform.localPosition,targ.transform.localPosition,Color.yellow);
        
  
        reward =-distanceToTarget;  
       
      
        if (distanceToTarget < 0.04f)
        {
            AddReward(1.0f);
           
            
            EndEpisode();
            print("stop3");
        }
        // else{
            AddReward(reward);
            AddReward(-5f/MaxStep);
        // }
    
        

        
    }

    void Update()
    {
       
           
    }

}

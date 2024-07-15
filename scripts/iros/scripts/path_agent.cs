using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.Serialization;

//using System.Diagnostics;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.Sentis.Layers;



public class path_agent : Agent
{
    public GameObject ee;
    public GameObject ee_kf;
    public GameObject limit1;
    public GameObject limit2;
    public GameObject targ1;
    public GameObject targ2;
    public GameObject targ3;
    public GameObject targ4;
    public GameObject targ5;
    Vector3 target0;
    public GameObject ground;
    Rigidbody ee_rb;
     Vector3 ori_pos;
    Vector3 ori_pos_t;

    public Vector3 dirToGo;
    Vector3 dirTot;
    Vector3 dirToo;
    Vector3 closest_p;
    public float distance;
    public float reward;
    public float o_reward;
  
    public float cost;

    float xMove;
    float yMove;
    float zMove;
    public float deltaMove;

    public float maxDis;
    public int a;
    public bool start;

    public int step;

    // Start is called before the first frame update
    void Start()
    {
        ori_pos = ee.transform.localPosition;
        ori_pos_t = targ1.transform.localPosition;

        ee_kf.transform.position = ee.transform.position;
        start=false;
        // ee_rb = ee.GetComponent<Rigidbody>();
        // ee_rb.freezeRotation = true;

        
    }
    public override void OnEpisodeBegin()
    {
        step=0;
        // ee.transform.localPosition = new Vector3(UnityEngine.Random.Range(-0.24f,-0.1f),UnityEngine.Random.Range(0f,0.06f),UnityEngine.Random.Range(-0.06f,0.06f));

        // targ1.transform.localPosition = new Vector3(UnityEngine.Random.Range(-1.5f,2.5f), UnityEngine.Random.Range(19.2f,2.5f), UnityEngine.Random.Range(-591.3f,-578.05f));

        // ee.transform.localPosition = ori_pos;
        // new Vector3(UnityEngine.Random.Range(-20f,40f), 28f ,UnityEngine.Random.Range(-20f,20f));

        // targ1.transform.localPosition = ori_pos_t;
        // new Vector3(UnityEngine.Random.Range(-20f,40f), 0f ,UnityEngine.Random.Range(-20f,20f));

        target0  = new Vector3(0f,0f,0f);
        if (start == true){
        switch (a)
        {
            case 0:
                target0  = targ1.transform.position;
                break;
            case 1:
                target0  = targ2.transform.position;
                break;
            case 2:
                target0  = targ3.transform.position;
                break;
            case 3:
                target0  = targ4.transform.position;
                break;
            case 4:
                target0  = targ5.transform.position;
                break;
        }
        }
        // a = Random.Range(0, 5);
        // a=10;
        // switch (a)
        // {
        //     case 0:
        //         target0  = targ1.transform;
        //         break;
        //     case 1:
        //         target0  = targ2.transform;
        //         break;
        //     case 2:
        //         target0  = targ3.transform;
        //         break;
        //     case 3:
        //         target0  = targ4.transform;
        //         break;
        //     case 4:
        //         target0  = targ5.transform;
        //         break;
        // }
    
// 
    }

     public override void CollectObservations(VectorSensor sensor)
    {
        if(start==true){
        sensor.AddObservation(distance);//1
        // sensor.AddObservation(GetComponent<closest_point>().distanceF);//1

        sensor.AddObservation(ground.transform.InverseTransformPoint(target0)); //3
        sensor.AddObservation(ground.transform.InverseTransformPoint(closest_p));//3
        sensor.AddObservation(ground.transform.InverseTransformPoint(ee.transform.position));//3
        sensor.AddObservation(ground.transform.InverseTransformPoint(dirTot));//3
        sensor.AddObservation(ground.transform.InverseTransformPoint(dirToo));//3
        sensor.AddObservation(ground.transform.InverseTransformPoint(dirToGo));
        }//3
       
        //3
        

    
      

    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {   
        
        if(start==true){
        step++;
        
    
        var disAct = actionBuffers.DiscreteActions;

        var xAxis = disAct[0];
        var yAxis = disAct[1];
        var zAxis = disAct[2];
        

        switch (xAxis)
        {
            case 0:
                 xMove=0f;
                break;
            case 1:
                xMove = -1f;
                break;
            case 2:
                xMove = 1f;
                break;
        }

        switch (yAxis)
        {
            case 0:
                 yMove=0f;
                break;
            case 1:
                yMove=-1f;
                break;
            case 2:
                yMove=1f;
                break;
        }

        switch (zAxis)
        {
            case 0:
                 zMove=0f;
                break;
            case 1:
                zMove=1f;
                break;
            case 2:
                zMove=-1f;
                break;
        }

        // target0 = targ1.transform;
        Debug.DrawLine(ee.transform.position,target0,Color.green);
        dirTot = target0 - ee.transform.position;
        // if(ee.transform.position==targ1.transform.position){ target0 = targ1.transform;} 
        

        ee.transform.position += new Vector3(xMove*deltaMove, yMove*deltaMove, zMove*deltaMove);
        dirToGo = new Vector3(xMove*deltaMove, yMove*deltaMove, zMove*deltaMove).normalized;
        // print("action");
        distance = Vector3.Distance(target0, ee.transform.position);
        // float dis_ob = GetComponent<closest_point>().distanceF;
        //  closest_p = GetComponent<closest_point>().closest_p;
        Debug.DrawLine(ee.transform.position,closest_p,Color.red);
        dirToo = closest_p - ee.transform.position;
        maxDis = Vector3.Distance(limit1.transform.position, limit2.transform.position);
        
        reward =1 - (distance/maxDis);
        // o_reward = 0.4f*(1f-(100f*dis_ob-0.5f)/Mathf.Sqrt(0.005f+Mathf.Pow((100f*dis_ob-0.5f),2f)));
        // reward = Mathf.Pow(1 - Mathf.Pow(reward / 1f, 2), 2);
        SetReward(reward-o_reward-0.1f*step/MaxStep);
        
        // if (dis_ob < 0.004f){
        //     AddReward(-1f);
        //     // EndEpisode();
        // }

        if (distance > 2*maxDis){
            SetReward(-1f);
            EndEpisode();

        }
        // if (dis_ob < 0.003){
            SetReward(-1f);
            // EndEpisode();

        }

        if (ee.transform.position.y<target0.y){
            SetReward(-1f);
            // EndEpisode();

        }
        if (Vector3.Distance(target0, ee.transform.position) < 0.002f) {
            
                SetReward(100f);
                
                EndEpisode();




            } 

        if(step>0.999f*MaxStep){
            SetReward(-2f);
            EndEpisode();

        } 

        }

        
        
     

        // ee_rb.AddForce(dirToGo/10f,
            // ForceMode.VelocityChange);
    }
    // void FixedUpdate()


    
// }
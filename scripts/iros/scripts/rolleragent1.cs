using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Security.Cryptography;
using Unity.Sentis.Layers;
// using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class rolleragent1 : Agent
{
    Rigidbody rBody;
    void Start () {
        rBody = GetComponent<Rigidbody>();

    }

    public Transform Target;
    public Transform obstacle;

    public Transform limit1;
    public Transform limit2;
   public float distanceToTarget;
   public float distanceToobstc;

   public float reward;
   public float O_reward;
   public Vector3 controlSignal;
   public Vector3 direct_A_T;
   public Vector3 direct_A_O;
   public float agl;

   float step;
    public override void OnEpisodeBegin()
    { step = 0f;
    
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
        
        this.transform.localPosition= new Vector3(Random.Range(-3f,3f),
                                          0f,
                                           -8.5f);

        // Move the target to a new spot
        Target.localPosition = new Vector3(Random.Range(-3f,3f),
                                           0f,
                                           10.24f);

        obstacle.localPosition = new Vector3(Random.Range(-3f,3f),
                                           0f,
                                           2f);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
    // Target and Agent positions
    sensor.AddObservation(Target.localPosition); //3
    // sensor.AddObservation(Obstacle.localPosition
    sensor.AddObservation(distanceToTarget);
    sensor.AddObservation(obstacle.localPosition);
    sensor.AddObservation(distanceToobstc);


    sensor.AddObservation(this.transform.localPosition);
    sensor.AddObservation(rBody.velocity);
    sensor.AddObservation(agl);
    // sensor.AddObservation(direct_A_T);
    // sensor.AddObservation(direct_A_O);
    // sensor.AddObservation(rBody.velocity.y);
    // sensor.AddObservation(rBody.velocity.z);
 
    

    
   
    }

    public float forceMultiplier = 0.1f;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    { step++;
        // Actions, size = 2
        controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(actionBuffers.ContinuousActions[0], -1, 1);
        controlSignal.y = Mathf.Clamp(actionBuffers.ContinuousActions[1], -1, 1);
        controlSignal.z = Mathf.Clamp(actionBuffers.ContinuousActions[2], -1, 1);
        rBody.AddForce(controlSignal * forceMultiplier);
        // print(controlSignal);

        // this.transform.localPosition += new Vector3(controlSignal.x*forceMultiplier,controlSignal.y*forceMultiplier,controlSignal.z*forceMultiplier);

      

         

        // Rewards
        distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        distanceToobstc = Vector3.Distance(this.transform.localPosition, obstacle.localPosition);

         direct_A_T = new Vector3(Target.transform.localPosition.x, Target.transform.localPosition.y, Target.transform.localPosition.z) - new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);
         direct_A_O = new Vector3(obstacle.transform.localPosition.x,obstacle.transform.localPosition.y,obstacle.transform.localPosition.z) - new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);   
         Debug.DrawLine(this.transform.position,Target.transform.position,Color.green);
         Debug.DrawLine(this.transform.position,obstacle.transform.position,Color.red);
         agl = Vector3.Angle(direct_A_T, direct_A_O)/180f;
        
        float maxdis= Vector3.Distance(limit1.localPosition,limit2.localPosition);
        if(distanceToTarget>distanceToobstc){

        O_reward = Mathf.Pow(agl , 2);
        // Mathf.Pow(1 - Mathf.Pow(1/(agl*distanceToobstc) , 2), 2);

        

        }

        reward = 1-distanceToTarget/maxdis;

        SetReward(reward/1f - 0.02f*distanceToobstc/maxdis  - step/3000);
        // AddReward();
      
        if (distanceToTarget < 1.7f)
        {
            AddReward(10.0f);
            print("jieshu1");
            EndEpisode();
        }

        // Fell off platform
        else if (distanceToTarget > 100)
        {
             AddReward(-10.0f);
            print("jieshu2");
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Collision的关键信息：
        //1.得到Collision的碰撞器：collision.collider
        //2.得到Collision依附的对象：collision.gameObject
        //3.得到Collision依附的对象的位置信息：collision.transform
        //4.得到我们俩有多少个接触点：collision.contactCount
        //  得到各个接触点的具体信息：ContactPoint[] pos = collision.contacts;
        if (collision.gameObject.name == "Cube"){
            print(this.name + "被" + "Cube" + "碰撞了");
            AddReward(-10f);
            EndEpisode();
        };
        
    }

   
   

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;
            continuousActionsOut[0] = Input.GetAxis("Horizontal");
            continuousActionsOut[1] = Input.GetAxis("Vertical");
            // continuousActionsOut[1] = Input.GetAxis("Vertical");
        }






}
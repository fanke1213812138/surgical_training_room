using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Security.Cryptography;
using Unity.Sentis.Layers;

public class rolleragent : Agent
{
    Rigidbody rBody;
    void Start () {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Target;

    public Transform Obstacle;

    public float angel_;

    Vector3 fromO_A;
    Vector3 fromT_A;
    public override void OnEpisodeBegin()
    {
       // If the Agent fell, zero its momentum
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3( 0, 0.5f, 0);
        }

        // Move the target to a new spot
        Target.localPosition = new Vector3(Random.value * 8 - 4,
                                           0.5f,
                                           Random.value * 8 - 4);

        // Obstacle.localPosition = new Vector3(Random.value * 8 - 4,
        //                                    0.5f,
        //                                    Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
    // Target and Agent positions
    sensor.AddObservation(Target.localPosition);
    sensor.AddObservation(Obstacle.localPosition);
    sensor.AddObservation(this.transform.localPosition);

    sensor.AddObservation(fromO_A);

    sensor.AddObservation(fromT_A);
    // sensor.AddObservation(angel_);

    // Agent velocity
    sensor.AddObservation(rBody.velocity.x);
    sensor.AddObservation(rBody.velocity.z);
    }

    public float forceMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        Vector3 fromO_A= Obstacle.transform.position - this.transform.position;
        Vector3 fromT_A = Target.transform.position -this.transform.position;
         angel_=Vector3.Angle(fromO_A,fromT_A);

        float a_reward= angel_/180;
        // print(a_reward);

         

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        float distanceToObstc = Vector3.Distance(this.transform.localPosition, Obstacle.localPosition);

        // if(distanceToTarget>distanceToObstc && angel_<90){
        //   AddReward((a_reward-1f)*0.1f);
        //   print(a_reward-1f);
        // }
        
        // Reached target
        if (distanceToTarget < 1.42f)
        {
            AddReward(1.0f);
            // print("jieshu1");
            EndEpisode();
        }

        // Fell off platform
        else if (this.transform.localPosition.y < 0)
        {
            // print("jieshu2");
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
        if (collision.gameObject.name == "Cube (1)"){
            print(this.name + "被" + "Cube (1)" + "碰撞了");
            AddReward(-1f);
        };
        
    }


        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;
            continuousActionsOut[0] = Input.GetAxis("Horizontal");
            continuousActionsOut[1] = Input.GetAxis("Vertical");
        }






}
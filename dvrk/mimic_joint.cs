using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using UnityEngine;

public class mimic_joint : MonoBehaviour
{
    public float mimic_cof;
    public JointStateReader mimic_parent;
    public JointStateWriter mimic_child;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // this.transform.localEulerAngles=new Vector3(0f,((270f-((mimic_j.transform.localEulerAngles.x+360f)%360f)+360f)%360f),0f);
        // transform.localRotation = Quaternion.Euler(Local_X, Local_Y, Local_Z);
        mimic_parent.Read(
                out string name,
                out float position,
                out float velocity,
                out float effort);
        // transform.localRotation = transform.localRotation*Quaternion.AngleAxis(270f-mimic_j.transform.localEulerAngles.x, Vector3.up);
        mimic_child.Write(position*mimic_cof);
        // print("this.transform.localEulerAngles"+position);
        // print("this"+(270f-((mimic_j.transform.localEulerAngles.x+360f)%360f)+360f)%360f);
        // print("2tthtit"+(270f-(mimic_j.transform.localEulerAngles.x+360f)%360f));
        // print("mimc_parent"+(mimic_j.transform.localEulerAngles.x+360f)%360f);
        // print("mimc_parent————"+mimic_j.transform.localEulerAngles);
        // print("mimc_child"+(this.transform.localEulerAngles.y+360f)%360f);
    }
}

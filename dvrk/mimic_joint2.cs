using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mimic_joint2 : MonoBehaviour
{
    public GameObject mimic_j;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localEulerAngles=new Vector3(0f,-(270f-(mimic_j.transform.localEulerAngles.x+360f)%360f),0f);
    }
}

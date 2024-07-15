using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class procedure_control : MonoBehaviour
{
    public path_agent agent;
    public ViewDataFusion kf;

    public GameObject ee;
    public GameObject ee_kf;
    // Start is called before the first frame update
    void Start()
    {
        agent= ee.GetComponent<path_agent>();
        kf = ee_kf.GetComponent<ViewDataFusion>();
        agent.enabled=false;
        kf.enabled=false;

        ee.transform.position = this.transform.position;
        ee_kf.transform.position = this.transform.position;



    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyUp(KeyCode.Q))
        {
           agent.start=true;
           agent.a=0;
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
           agent.start=true;
           agent.a=1;
        }
        if(Input.GetKeyUp(KeyCode.A))
        {
            ee.transform.position = this.transform.position;
            ee_kf.transform.position = this.transform.position;
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            agent.enabled = !agent.enabled;
            kf.enabled = !kf.enabled;
        }
        
    }
}

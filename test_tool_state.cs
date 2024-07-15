using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_tool_state : MonoBehaviour
{
    public ConfigurableJoint UnityJoint;
    public Vector3 state;
    
    // Start is called before the first frame update
    void Start()
    {
        UnityJoint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        state = UnityJoint.transform.localPosition - UnityJoint.connectedAnchor;
    }
}

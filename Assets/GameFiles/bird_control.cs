using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird_control : MonoBehaviour
{   
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        //var transform=this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position+=new Vector3(speed,0,0);
    }
}

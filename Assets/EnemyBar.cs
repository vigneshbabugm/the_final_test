using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBar : MonoBehaviour
{
    Vector3 local;
    float value;
    // Start is called before the first frame update
    void Start()
    {
        local = this.transform.localScale;
       

    }

    // Update is called once per frame
    void Update()
    {
        value = this.gameObject.GetComponentInParent<EnemyController>().health;
        local.x = ((value * 2) / 100);
        //Debug.Log(localscale.x);
        Debug.Log(local.x);
        this.transform.localScale = local;
    }
}

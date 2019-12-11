using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background_control : MonoBehaviour
{
    public GameObject bird; 
    
    WaitForSeconds waitForSeconds = new WaitForSeconds(5.0f);
 
    IEnumerator myStart()
    {
        while (true)
         {
             Instantiate(bird, new Vector3(0.74f, Random.Range(1.5f,2.5f), -0.1456481f), Quaternion.identity);
             yield return waitForSeconds;
         }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("myStart");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

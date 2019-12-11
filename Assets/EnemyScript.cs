using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private int enemyHealth;

    public float speed;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posx = new Vector3(target.position.x, target.position.y+0.03f, 0);
        transform.position= Vector3.MoveTowards(transform.position, posx, speed * Time.deltaTime); 
    }
    public void takeDamage(int damage)
    {
        if (enemyHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        enemyHealth = enemyHealth - damage;
        Debug.Log(enemyHealth);
    }
}

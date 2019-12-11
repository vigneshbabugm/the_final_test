using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemy;
    private float val;
    Vector2 spawnPoint;
    public Transform leftSpawn;
    public Transform rightSpawn;
    public float spawnRate = 3.4f;
    private float nextSpawn = 0.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            val = Random.Range(leftSpawn.position.x, rightSpawn.position.x);
            spawnPoint = new Vector2(val, leftSpawn.position.y);
            Instantiate(enemy, spawnPoint, Quaternion.identity);

        }
    }
}

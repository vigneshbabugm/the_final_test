using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    // Start is called before the first frame update
    public Text scoreIs;
    void Start()
    {
        int val = PlayerPrefs.GetInt("Score");
        scoreIs.text = "Score:  " + PlayerPrefs.GetInt("Score");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void buttonClick() {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}

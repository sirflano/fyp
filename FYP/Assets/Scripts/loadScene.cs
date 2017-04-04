using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour {

    public string scene;
    private GameObject gun;
    // Use this for initialization
    void Start()
    {
        gun = GameObject.FindGameObjectWithTag("gun");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            SceneManager.LoadScene(scene);
        }
    }
}

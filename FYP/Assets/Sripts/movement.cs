using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {

    public float moveSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
	}
}

using UnityEngine;
using System.Collections;

public class moveForward : MonoBehaviour {

    public float moveSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //move the object in the direction of its forward vector at the speed defined in the inspector
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
	}
}

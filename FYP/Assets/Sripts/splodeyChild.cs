using UnityEngine;
using System.Collections;

public class splodeyChild : MonoBehaviour {

    public GameObject parent;
    public float moveSpeed;
    public float deathCry;
    private bool splodey = false;
    private float splodeTime = 0;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(splodey)
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
            //splodeTime += 10 * Time.deltaTime;
            //if (splodeTime >= deathCry)
            //{
            //    Destroy(gameObject);
            //}
        }
	}

    void OnCollisionEnter(Collision col)
    {
        //transform.position -= new Vector3(0, -5000, 0);
        if (col.gameObject.layer == 9)
        {
            parent.GetComponent<splodeyParent>().splode();
        }
       // parent.GetComponent<splodeyParent>().splode();
    }

    public void splode()
    {
        splodey = true;
        transform.rotation = Random.rotation;
    }
}

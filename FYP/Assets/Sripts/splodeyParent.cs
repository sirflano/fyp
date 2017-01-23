using UnityEngine;
using System.Collections;

public class splodeyParent : MonoBehaviour {

    public GameObject[] children;
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
            splodeTime += 1 * Time.deltaTime;
            if(splodeTime >= deathCry)
            {
                Destroy(gameObject);
            }
        }
	}

    public void splode()
    {
        //transform.position -= new Vector3(0, 0, 5000);
        for(int i = 0; i < children.Length; i++)
        {
            children[i].GetComponent<splodeyChild>().splode();
        }
        splodey = true;
    }
}

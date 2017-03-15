using UnityEngine;
using System.Collections;

public class splodeyParent : MonoBehaviour {

    public GameObject[] children;
    public float deathCry;
    private bool splodey = false;
    private float splodeTime = 0;
    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(splodey)
        {
            anim.SetBool("dead", true);
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

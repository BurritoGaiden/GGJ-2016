using UnityEngine;
using System.Collections;

public class BubblePop : MonoBehaviour {

    float timer;
    float nextPop;

	// Use this for initialization
	void Start () {
        timer = 0;
        nextPop = Random.Range(0.5f, 1.4f);
        this.transform.localScale = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer >= nextPop)
        {
            if(this.transform.localScale.x < 1)
            {
                this.transform.localScale += new Vector3(0.1f,0.1f,0.1f);

            }
            else
            {
                timer = 0;
                nextPop = Random.Range(0.5f, 1.4f);
                this.transform.localScale = new Vector3(0, 0, 0);
            }

        }
	}
}

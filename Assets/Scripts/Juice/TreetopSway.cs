using UnityEngine;
using System.Collections;

public class TreetopSway : MonoBehaviour {

    private float circleTimer;
    private float randomSpeed;

    // Use this for initialization
    void Start () {
        circleTimer = Random.Range(0, 2 * Mathf.PI);
        randomSpeed = Random.Range(1.0f, 2.5f);
    }
	
	// Update is called once per frame
	void Update () {
        circleTimer += Time.deltaTime / randomSpeed;

        if (circleTimer > 2* Mathf.PI)
        {
            circleTimer -= 2*Mathf.PI;
        }
        this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(circleTimer) * 10);
	}
}

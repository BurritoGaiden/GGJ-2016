using UnityEngine;
using System.Collections;

public class FloatingIslands : MonoBehaviour {

    private Vector3 actualPosition;
    public float floatAmount;
    private float circleTimer;
    private float randomSpeed;

	// Use this for initialization
	void Start () {
        actualPosition = this.transform.position;
        randomSpeed = Random.Range(0.75f, 2.0f);
        circleTimer = Random.Range(0, 2 * Mathf.PI);
	}
	
	// Update is called once per frame
	void Update () {
        circleTimer += Time.deltaTime / randomSpeed;
        if(circleTimer > 2 *Mathf.PI)
        {
            circleTimer -= 2 * Mathf.PI;
        }
        float floatLocation = actualPosition.y + (Mathf.Sin(circleTimer) * floatAmount);
        this.transform.position = new Vector3(this.transform.position.x,
                                                floatLocation,
                                                this.transform.position.z);
	}
}

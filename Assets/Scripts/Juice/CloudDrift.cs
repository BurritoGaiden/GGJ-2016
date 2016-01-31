using UnityEngine;
using System.Collections;

public class CloudDrift : MonoBehaviour {

    public Vector2 cloudSpeed;
    private float speed;

	// Use this for initialization
	void Start () {
        speed = Random.Range(cloudSpeed.x, cloudSpeed.y);
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(this.transform.position.x - (Time.deltaTime * speed), this.transform.position.y, this.transform.position.z);
        if(this.transform.position.x < - 40)
        {
            this.transform.position = new Vector3(Random.Range(60, 45), this.transform.position.y, this.transform.position.z);
            speed = Random.Range(cloudSpeed.x, cloudSpeed.y);
        }
	}
}

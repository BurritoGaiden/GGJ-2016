using UnityEngine;
using System.Collections;

public class CloudDrift : MonoBehaviour {

	public Vector2 cloudSpeed;
	private float speed;
	private Vector3 startPos;
	public float screenshakeAmount = 0.0f;

	// Use this for initialization
	void Start() {
		Messenger.Instance.Listen(ListenerType.SCREENSHAKE, this);

		speed = Random.Range(cloudSpeed.x, cloudSpeed.y);
		startPos = transform.position;
	}

	// Update is called once per frame
	void Update() {
		transform.SetPositionXRelative(-(Time.deltaTime * speed));
		if (transform.position.x < -40) {
			transform.SetPositionX(Random.Range(60, 45));
			speed = Random.Range(cloudSpeed.x, cloudSpeed.y);
		}

		if (screenshakeAmount > 0.0f) {
			screenshakeAmount -= Time.deltaTime * 5;
			transform.SetPositionY(startPos.y + 0.05f * Random.Range(-screenshakeAmount, screenshakeAmount));
			transform.SetPositionZ(startPos.z + 0.05f * Random.Range(-screenshakeAmount, screenshakeAmount));
		} else {
			screenshakeAmount = 0.0f;
			this.transform.rotation = Quaternion.identity;
		}
	}

	public void _Screenshake(MessageScreenshake msg) {
		screenshakeAmount = Random.Range(msg.Amount * 0.9f, msg.Amount * 1.1f);
	}
}

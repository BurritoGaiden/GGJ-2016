using UnityEngine;
using System.Collections;

public class TreetopSway : MonoBehaviour {

	private float circleTimer;
	private float randomSpeed;
	public float screenshakeAmount = 0.0f;

	// Use this for initialization
	void Start() {
		Messenger.Instance.Listen(ListenerType.SCREENSHAKE, this);

		circleTimer = Random.Range(0, 2 * Mathf.PI);
		randomSpeed = Random.Range(1.0f, 2.5f);
	}

	// Update is called once per frame
	void Update() {
		circleTimer += Time.deltaTime / randomSpeed;

		if (screenshakeAmount > 0.0f) {
			screenshakeAmount -= Time.deltaTime * 1.5f;
			circleTimer += screenshakeAmount * Time.deltaTime;
		} else {
			screenshakeAmount = 0.0f;
		}

		if (circleTimer > 2 * Mathf.PI) {
			circleTimer -= 2 * Mathf.PI;
		}
		this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(circleTimer) * 10);
	}

	public void _Screenshake(MessageScreenshake msg) {
		screenshakeAmount = Random.Range(msg.Amount * 0.9f, msg.Amount * 1.1f);
	}

}

using UnityEngine;
using System.Collections;

public class FloatingIslands : MonoBehaviour {

	private Vector3 actualPosition;
	public float floatAmount;
	private float circleTimer;
	private float randomSpeed;

	private float offsetAmount = 0.0f;
	public float screenshakeAmount = 0.0f;
	private Vector3 startPosition;

	// Use this for initialization
	void Start() {
		Messenger.Instance.Listen(ListenerType.SCREENSHAKE, this);

		actualPosition = this.transform.position;
		randomSpeed = Random.Range(0.75f, 2.0f);
		circleTimer = Random.Range(0, 2 * Mathf.PI);

		startPosition = transform.position;
	}

	// Update is called once per frame
	void Update() {
		circleTimer += Time.deltaTime / randomSpeed;
		if (circleTimer > 2 * Mathf.PI) {
			circleTimer -= 2 * Mathf.PI;
		}
		float floatLocation = actualPosition.y + (Mathf.Sin(circleTimer) * floatAmount);
		this.transform.position = new Vector3(this.transform.position.x,
												floatLocation,
												this.transform.position.z);

		if (screenshakeAmount > 0.0f) {
			screenshakeAmount -= Time.deltaTime * 2;
			offsetAmount -= Time.deltaTime * 4;
			if (offsetAmount < 0) offsetAmount = 0.0f;
			circleTimer += screenshakeAmount * Time.deltaTime * 0.5f;
			this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(screenshakeAmount) * 10);
			transform.SetPositionX(startPosition.x + 0.05f * Random.Range(-offsetAmount, offsetAmount));
			transform.SetPositionZ(startPosition.z + 0.05f * Random.Range(-offsetAmount, offsetAmount));
		} else {
			screenshakeAmount = 0.0f;
			this.transform.rotation = Quaternion.identity;
		}
	}

	public void _Screenshake(MessageScreenshake msg) {
		offsetAmount = screenshakeAmount = Random.Range(msg.Amount * 0.9f, msg.Amount * 1.1f);
	}
}

using UnityEngine;
using System.Collections;

public class FireShakeRattleAndRoll : MonoBehaviour {

	private float timer = 0.0f;
	public float timeOut;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		timer -= Time.deltaTime;

		if (timer < 0.0f) {
			if (Random.Range(1, 10) < 6)
				this.transform.localScale = new Vector3(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));
			timer = timeOut;
		}
	}
}

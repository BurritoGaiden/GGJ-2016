using UnityEngine;
using System.Collections;

public class ExplorerBehavior : MonoBehaviour {

	private bool dead = false;
	private float yspeed = 0.0f;
	private GameObject parent;

	// Use this for initialization
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {
		if (dead)
			MoveTowardsPot();
	}

	private void MoveTowardsPot() {
		parent.transform.SetPositionYRelative(yspeed * Time.deltaTime);

		yspeed -= 5.5f * Time.deltaTime;

		int axesAligned = 0;

		if (Mathf.Abs(parent.transform.position.x) > 0.1f)
			parent.transform.SetPositionXRelative(-4.0f * Time.deltaTime * Mathf.Sign(parent.transform.position.x));
		else {
			parent.transform.SetPositionX(0.0f);
			++axesAligned;
		}

		if (Mathf.Abs(parent.transform.position.z) > 0.1f)
			parent.transform.SetPositionZRelative(-4.0f * Time.deltaTime * Mathf.Sign(parent.transform.position.z));
		else
			parent.transform.SetPositionZ(0.0f);

		float angleX = parent.transform.GetEulerAngleX();
		if ((angleX < 90) || (angleX < 10))
			parent.transform.SetEulerAngleX(angleX + 65.0f * Time.deltaTime);
		else {
			parent.transform.SetEulerAngleX(90.0f);
			++axesAligned;
		}

		if (axesAligned == 2) {
			parent.transform.SetEulerAngleY(parent.transform.GetEulerAngleY() + Time.deltaTime * 180.0f);
			float scale = parent.transform.localScale.x - 0.2f * Time.deltaTime;
			parent.transform.localScale = new Vector3(scale, scale, scale);
		}

		if (parent.transform.localScale.x < 0.65f) {
			new MessageAddedToPot() {
				//
			}.Send();
			Destroy(parent);
		}
	}

	public void Die() {
		parent = new GameObject("Explorer Rotation Parent");
		parent.transform.position = transform.position + Vector3.up * 1.5f;
		// This doesn't look confusing
		transform.parent = parent.transform;
		dead = true;
		yspeed = 10.0f;
	}

}

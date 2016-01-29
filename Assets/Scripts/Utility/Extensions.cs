using UnityEngine;
using System.Collections;

public static class Extensions {

	public static void SetPositionX(this Transform t, float newX) {
		t.position = new Vector3(newX, t.position.y, t.position.z);
	}

	public static void SetPositionY(this Transform t, float newY) {
		t.position = new Vector3(t.position.x, newY, t.position.z);
	}

	public static void SetPositionZ(this Transform t, float newZ) {
		t.position = new Vector3(t.position.x, t.position.y, newZ);
	}

	public static void SetPositionXRelative(this Transform t, float newX) {
		t.SetPositionX(newX + t.GetPositionX());
	}

	public static void SetPositionYRelative(this Transform t, float newY) {
		t.SetPositionY(newY + t.GetPositionY());
	}

	public static void SetPositionZRelative(this Transform t, float newZ) {
		t.SetPositionZ(newZ + t.GetPositionZ());
	}

	public static void SetEulerAngleX(this Transform t, float newX) {
		t.eulerAngles = new Vector3(newX, t.eulerAngles.y, t.eulerAngles.z);
	}

	public static void SetEulerAngleY(this Transform t, float newY) {
		t.eulerAngles = new Vector3(t.eulerAngles.x, newY, t.eulerAngles.z);
	}

	public static void SetEulerAngleZ(this Transform t, float newZ) {
		t.eulerAngles = new Vector3(t.eulerAngles.x, t.eulerAngles.y, newZ);
	}

	public static float GetPositionX(this Transform t) {
		return t.position.x;
	}

	public static float GetPositionY(this Transform t) {
		return t.position.y;
	}

	public static float GetPositionZ(this Transform t) {
		return t.position.z;
	}

	public static float GetEulerAngleX(this Transform t) {
		return t.eulerAngles.x;
	}

	public static float GetEulerAngleY(this Transform t) {
		return t.eulerAngles.y;
	}

	public static float GetEulerAngleZ(this Transform t) {
		return t.eulerAngles.z;
	}
}
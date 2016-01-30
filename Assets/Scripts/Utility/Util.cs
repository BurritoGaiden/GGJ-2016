using UnityEngine;
using System.Collections;

public class Util {

	public const float FRAME = 1.0f / 60.0f;
	public static float Frames(int n) {
		return FRAME * (float)n;
	}

	public static float Wrap(float value, float min, float max) {
		if (value < min) return max;
		if (value > max) return min;
		return value;
	}

	public static float Clamp(float value, float min, float max) {
		if (value < min) return min;
		if (value > max) return max;
		return value;
	}

	public static Vector2 ClampInRect(Vector2 value, Rect rect) {
		return new Vector2(
			Clamp(value.x, rect.xMin, rect.xMax),
			Clamp(value.y, rect.yMin, rect.yMax)
		);
	}

	public static bool IsInt(float value) {
		return (float)((int)value) == value;
	}

	public static float GetDecimal(float value) {
		value = Mathf.Abs(value);
		return value - Mathf.Floor(value);
	}

	public static float Sign(float value) {
		return (value < 0.0f) ? -1.0f : (value > 0.0f ? 1.0f : 0.0f);
	}

	public static int Sign(int value) {
		return (value < 0) ? -1 : (value > 0 ? 1 : 0);
	}

	public static Vector2 GetRandomInputDir() {
		return GetInputDirByIndex(Random.Range(0, 4));
	}

	public static Vector2 GetInputDirByIndex(int n) {
		switch (n) {
			case 0: return InputDir.Left;
			case 1: return InputDir.Right;
			case 2: return InputDir.Up;
			case 3: return InputDir.Down;
		}

		return Vector2.zero;
	}



	// Approach functions totally stolen from Kyle Pulver
	public static float Approach(float val, float target, float maxMove) {
		return val > target ? Mathf.Max(val - maxMove, target) : Mathf.Min(val + maxMove, target);
	}

	public static Vector2 Approach(Vector2 val, Vector2 target, Vector2 maxMove) {
		return new Vector2(
			Approach(val.x, target.x, maxMove.x),
			Approach(val.y, target.y, maxMove.y)
			);
	}

	public static Vector2 Approach(Vector2 val, Vector2 target, float maxMove) {
		Vector2 maxMoveVec2 = target - val;
		Vector2 signs = new Vector2(Mathf.Sign(maxMoveVec2.x), Mathf.Sign(maxMoveVec2.y));
		maxMoveVec2.Normalize();
		maxMoveVec2 *= maxMove;
		maxMoveVec2.Scale(signs);
		return Approach(val, target, maxMoveVec2);
	}

	public static bool InRange(float value, float min, float max) {
		return (value > min) && (value < max);
	}

	public static bool InRange(Vector2 value, Vector2 min, Vector2 max) {
		return (InRange(value.x, min.x, max.x)) && (InRange(value.y, min.y, max.y));
	}

	public static bool WithinRange(float value, float min, float max) {
		return (value >= min) && (value <= max);
	}

	public static bool InRect(Vector2 point, Rect rect) {
		return Util.InRange(point.x, rect.xMin, rect.xMax) && Util.InRange(point.y, rect.yMin, rect.yMax);
	}

	public static bool WithinRect(Vector2 point, Rect rect) {
		return Util.WithinRange(point.x, rect.xMin, rect.xMax) && Util.WithinRange(point.y, rect.yMin, rect.yMax);
	}



	public static float GetPercentBetween(float value, float min, float max) {
		if (value < min) return 0.0f;
		if (value > max) return 1.0f;
		return (value - min) / (max - min);
	}

	public static void DrawDebugLine(float x1, float y1, float w, float h, Color c) {
		Vector2 topLeft = new Vector2(x1, y1);
		Vector2 topRight = new Vector2(x1 + w, y1);
		Vector2 bottomLeft = new Vector2(x1, y1 + h);
		Vector2 bottomRight = new Vector2(x1 + w, y1 + h);
		Debug.DrawLine(topLeft, topRight, c);
		Debug.DrawLine(topRight, bottomRight, c);
		Debug.DrawLine(bottomRight, bottomLeft, c);
		Debug.DrawLine(bottomLeft, topLeft, c);
	}

}

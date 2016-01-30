﻿using UnityEngine;
using System.Collections;

public class Util {
	
	#region Pixel constants for quick reference
#if UNITY_EDITOR
	public const float _2PIXELS = 0.125f;
	public const float _3PIXELS = 0.1875f;
	public const float _4PIXELS = 0.25f;
	public const float _5PIXELS = 0.3125f;
	public const float _6PIXELS = 0.375f;
	public const float _7PIXELS = 0.4375f;
	public const float _8PIXELS = 0.5f;
	public const float _9PIXELS = 0.5625f;
	public const float _10PIXELS = 0.625f;
	public const float _11PIXELS = 0.6875f;
	public const float _12PIXELS = 0.75f;
	public const float _13PIXELS = 0.8125f;
	public const float _14PIXELS = 0.875f;
	public const float _15PIXELS = 0.9375f;
#endif
	#endregion

	public const float PIXEL = 0.0625f;
	public static float Pixels(int n) {
		return PIXEL * (float)n;
	}

	public static float NearestPixel(float value) {
		return Pixels(Mathf.RoundToInt(value * 16.0f));
	}

	public const float FRAME = 1.0f / 60.0f;
	public static float Frames(int n) {
		return FRAME * (float)n;
	}

	public static IEnumerator WaitForSeconds(float seconds) {
		for (float t = 0.0f; t < seconds; t += Time.deltaTime) {
			yield return 0;
		}
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
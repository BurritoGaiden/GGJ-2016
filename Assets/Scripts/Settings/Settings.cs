using UnityEngine;
using System.Collections;

public class Settings {

	private static bool initialized = false;
	public static void Init() {
		if (!initialized) {
			initialized = true;
		}
	}

}

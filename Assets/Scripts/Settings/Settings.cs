using UnityEngine;
using System.Collections;

public class Settings {

	private static bool initialized = false;
	public static void Init() {
		if (!initialized) {
			initialized = true;
		}
	}

	private static float volume = 1.0f;
	public static float Volume {
		set {
			var mcb = GameObject.FindGameObjectWithTag(Tags.MUSICCONTROLLER).GetComponent<MusicControllerBehavior>();
			for (int i = 0; i < mcb.AudioSource.Length; ++i) {
				if (volume != 0.0f) {
					mcb.AudioSource[i].volume /= volume;
					mcb.AudioSource[i].volume *= value;
				} else
					mcb.AudioSource[i].volume = value;
			}
			volume = value;
		}
		get {
			return volume;
		}
	}

}

using UnityEngine;
using System.Collections;

public enum MusicFiles {
	TRIBAL, EXPLORER, WIN, LOSE,
	NUM, NONE,
}

public static class Music {

	public static float GetTrackLength(MusicFiles n) {
		var mcb = GameObject.FindGameObjectWithTag(Tags.MUSICCONTROLLER).GetComponent<MusicControllerBehavior>();
		return mcb.Music[(int)n].length;
	}

	public static void PlayTracks() {
		var mcb = GameObject.FindGameObjectWithTag(Tags.MUSICCONTROLLER).GetComponent<MusicControllerBehavior>();
		mcb.PlayTracks();
	}

}

public class MusicControllerBehavior : MonoBehaviour {

	public AudioClip[] Music;
	public AudioSource[] AudioSource;

	// Use this for initialization
	void Start() {
		AudioSource = new AudioSource[Music.Length];
		for (int i = 0; i < Music.Length; ++i) {
			AudioSource[i] = gameObject.AddComponent<AudioSource>();
			AudioSource[i].clip = Music[i];
		}

		//Track = MusicFiles.MENUMUSIC;
		Object.DontDestroyOnLoad(gameObject);
	}

	// Update is called once per frame
	void Update() {

	}

	public void PlayTracks() {
		float delay = 0.0f;
		for (int i = 0; i < AudioSource.Length; ++i) {
			if (i < 2) // TODO: Remove this!!!
				AudioSource[i].PlayDelayed(delay);
			delay += AudioSource[i].clip.length;
		}
	}

}

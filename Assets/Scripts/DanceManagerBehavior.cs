using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Dance {
	public GameObject Icon, IconBroken;
	public string AnimationName;
	// TODO(bret): Animation that should play
}

public class DanceManagerBehavior : MonoBehaviour {

	// A list of Dances
	public List<Dance> Dances = new List<Dance>();
	// The Dances claimed by Pillars
	public static List<int> DancesClaimed = new List<int>();

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public static void Reset() {
		DancesClaimed.Clear();
	}

	// note(bret): NOT REALLY THE SINGLETON PATTERN BUT KIND OF DON'T HATE ME!!!
	public static DanceManagerBehavior GetInstance() {
		return GameObject.FindGameObjectWithTag(Tags.DANCEMANAGER).GetComponent<DanceManagerBehavior>();
	}

	public Dance ChooseRandomDance() {
		if (DancesClaimed.Count >= Dances.Count) {
			Debug.LogWarning("We don't have enough dances!");
			return null;
		}

		int n;
		while (true) {
			n = Random.Range(0, Dances.Count);
			if (!DancesClaimed.Contains(n)) {
				DancesClaimed.Add(n);
				return Dances[n];
			}
		}
	}

}

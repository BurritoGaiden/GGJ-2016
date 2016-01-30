using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PillarBehavior : MonoBehaviour {

	[System.Serializable]
	public class IconInfo {
		public List<Vector2> InputString = new List<Vector2>();
		public Dance Dance;
	}

	// The input strings
	public IconInfo Tribal;
	public IconInfo Explorer;

	// If false, tribal on top, if true, explorer on top
	private bool swapped = false;

	// The inputs that have been claimed by other pillars, too prevent from duplicates
	private static List<List<Vector2>> inputsClaimed = new List<List<Vector2>>();

	// Use this for initialization
	void Start() {
		
	}

	// Update is called once per frame
	void Update() {

	}

	public static void Reset() {
		inputsClaimed.Clear();
	}

	private void GetInput() {
		
	}

	public void Create() {
		// Set some variables
		var dmb = DanceManagerBehavior.GetInstance();
		swapped = (Random.Range(0, 2) > 0);

		// Set up Tribal dance information
		Tribal.InputString = CreateRandomInputString();
		Tribal.Dance = dmb.ChooseRandomDance();

		if (Tribal.Dance != null) {
			PlaceIcon(Tribal.Dance.Icon, swapped);
			PlaceArrows(Tribal.InputString, swapped);
		}

		// Set up Explorer dance information
		Explorer.InputString = CreateRandomInputString();
		Explorer.Dance = dmb.ChooseRandomDance();

		if (Explorer.Dance != null) {
			PlaceIcon(Explorer.Dance.Icon, !swapped);
			PlaceArrows(Explorer.InputString, !swapped);
		}
	}

	private void PlaceIcon(GameObject icon, bool top) {
		var icongo = Instantiate(icon);
		icongo.transform.parent = gameObject.transform;
		icongo.transform.position = gameObject.transform.position + new Vector3(0.0f, (top) ? 0.75f : -0.25f, -0.5f);
	}

	// TODO: implement
	private void PlaceArrows(List<Vector2> inputString, bool top) {
		
	}

	private List<Vector2> CreateRandomInputString() {
		var inputRequired = new List<Vector2>();

		while (true) {
			// Create a random pattern
			inputRequired.Clear();
			for (int i = 0; i < 4; ++i) {
				inputRequired.Add(Util.GetRandomInputDir());
			}

			// Check for matches
			bool noMatch = true;
			for (int n = 0; n < inputsClaimed.Count; ++n) {
				bool matches = true;
				for (int index = 0; index < inputsClaimed[n].Count; ++index) {
					if (inputRequired[index] != inputsClaimed[n][index]) {
						matches = false;
					}
				}

				if (matches) {
					noMatch = false;
					break;
				}
			}

			// If it doesn't already exist, good! Break outta the loop
			if (noMatch) {
				inputsClaimed.Add(inputRequired);
				break;
			}
		}

		return inputRequired;
	}

	public bool CheckInput(List<Vector2> inputQueue) {
		// If the input count doesn't match up, it definitely isn't correct
		if (inputQueue.Count != Explorer.InputString.Count)
			return false;

		// Check input one at a time to see if they match
		bool success = true;
		for (int i = 0; i < Explorer.InputString.Count; ++i) {
			if (inputQueue[i] != Explorer.InputString[i]) {
				success = false;
			}
		}

		return success;
	}

#if UNITY_EDITOR
	public string GetInputString() {
		string inputStr = "";
		for (int i = 0; i < Explorer.InputString.Count; ++i) {
			if (Explorer.InputString[i] == InputDir.Left) inputStr += "Left, ";
			if (Explorer.InputString[i] == InputDir.Right) inputStr += "Right, ";
			if (Explorer.InputString[i] == InputDir.Up) inputStr += "Up, ";
			if (Explorer.InputString[i] == InputDir.Down) inputStr += "Down, ";
		}
		return inputStr;
	}
#endif
}

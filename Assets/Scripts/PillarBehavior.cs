using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dance {
	//
}

public class PillarBehavior : MonoBehaviour {

	public Dance Dance;

	// The input strings
	[SerializeField]
	public List<Vector2> tribalInputString = new List<Vector2>();
	[SerializeField]
	public List<Vector2> explorerInputString = new List<Vector2>();

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
		tribalInputString = CreateRandomInputString();
		explorerInputString = CreateRandomInputString();

		swapped = (Random.Range(0, 2) > 0);
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
		if (inputQueue.Count != explorerInputString.Count)
			return false;

		// Check input one at a time to see if they match
		bool success = true;
		for (int i = 0; i < explorerInputString.Count; ++i) {
			if (inputQueue[i] != explorerInputString[i]) {
				success = false;
			}
		}

		return success;
	}

#if UNITY_EDITOR
	public string GetInputString() {
		string inputStr = "";
		for (int i = 0; i < explorerInputString.Count; ++i) {
			if (explorerInputString[i] == InputDir.Left) inputStr += "Left, ";
			if (explorerInputString[i] == InputDir.Right) inputStr += "Right, ";
			if (explorerInputString[i] == InputDir.Up) inputStr += "Up, ";
			if (explorerInputString[i] == InputDir.Down) inputStr += "Down, ";
		}
		return inputStr;
	}
#endif
}

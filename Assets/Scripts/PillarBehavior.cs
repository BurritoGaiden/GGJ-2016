using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dance {
	//
}

public class PillarBehavior : MonoBehaviour {

	public Dance Dance;
	[SerializeField]
	public List<Vector2> inputRequired = new List<Vector2>();

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
		while (true) {
			// Create a random pattern
			inputRequired.Clear();
			for (int i = 0; i < 4; ++i) {
				inputRequired.Add(Util.GetRandomInputDir());
			}

			// Check for matches
			bool noMatch = true;
			for (int i = 0; i < inputsClaimed.Count; ++i) {
				bool matches = true;
				for (int j = 0; j < inputsClaimed[i].Count; ++j) {
					if (inputRequired[i] != inputsClaimed[i][j]) {
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
	}

	public bool CheckInput(List<Vector2> inputQueue) {
		// If the input count doesn't match up, it definitely isn't correct
		if (inputQueue.Count != inputRequired.Count)
			return false;

		// Check input one at a time to see if they match
		bool success = true;
		for (int i = 0; i < inputRequired.Count; ++i) {
			if (inputQueue[i] != inputRequired[i]) {
				success = false;
			}
		}

		return success;
	}

#if UNITY_EDITOR
	public string GetInputString() {
		string inputStr = "";
		for (int i = 0; i < inputRequired.Count; ++i) {
			if (inputRequired[i] == InputDir.Left)	inputStr += "Left, ";
			if (inputRequired[i] == InputDir.Right)	inputStr += "Right, ";
			if (inputRequired[i] == InputDir.Up)	inputStr += "Up, ";
			if (inputRequired[i] == InputDir.Down)	inputStr += "Down, ";
		}
		return inputStr;
	}
#endif
}

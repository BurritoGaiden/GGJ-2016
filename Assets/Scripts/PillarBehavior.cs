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

	private static Vector3[] faceOffsets;
	private static bool faceOffsetsCreated = false;
	private static GameObject pillarParent;

	// Positioning
	private Vector3 startPos;
	private Vector3 sinkOffset = new Vector3(0.0f, -8.5f, 0.0f);
	private Vector3 shakeOffset = new Vector3();
	private float sinkDelay = 0.0f;
	private float sinkDelayMax = 0.8f;
	private float sinkSpeed = 1.0f / 5.0f; // The second number is the number of seconds
	private float sinkLerpValue = 1.0f;
	public float sinkDirection = -1.0f;

	public GameObject DanceIcon;
	public GameObject[] ArrowPrefabs;
	private GameObject[] Arrows;
	private GameObject[] ArrowsGlowing;

	public int MountainForce;

	public int PillarID = -1;

	// If false, tribal on top, if true, explorer on top
	private bool swapped = false;

	// The inputs that have been claimed by other pillars, too prevent from duplicates
	private static List<List<Vector2>> inputsClaimed = new List<List<Vector2>>();

	// Use this for initialization
	void Start() {
		Messenger.Instance.Listen(ListenerType.CHANGESTATE, this);
	}

	// Update is called once per frame
	void Update() {
		if (sinkDelay > 0.0f)
			sinkDelay -= Time.deltaTime;

		if (sinkDelay <= 0.0f) {
			sinkLerpValue += sinkSpeed * sinkDirection * Time.deltaTime;
			sinkLerpValue = Mathf.Clamp01(sinkLerpValue);
			transform.position = startPos + Vector3.Lerp(Vector3.zero, sinkOffset, sinkLerpValue);
			if ((sinkLerpValue != 0.0f) && (sinkLerpValue != 1.0f)) {
				shakeOffset.x = Random.Range(-100.0f, 100.0f) / (float)MountainForce;
				transform.position += shakeOffset;
			}
		}
	}

	public void _ChangeState(MessageChangeState msg) {
		if (msg.GameState == GameStates.ENDROUND) {
			sinkDirection *= -1.0f;
		}
	}

	public static void Reset() {
		inputsClaimed.Clear();
	}

	private void GetInput() {
		
	}

	public void Create() {
		startPos = transform.position;
		transform.position += sinkOffset;

		sinkDelay = 0.1f + Random.Range(0.0f, sinkDelayMax);

		// Create the faceOffsets if not created
		if (!faceOffsetsCreated) {
			pillarParent = new GameObject("Pillars");
			pillarParent.transform.position = Vector3.zero;

			faceOffsets = new Vector3[10];

			// Top/bottom on Pillar 1
			faceOffsets[0] = new Vector3(-0.1f, 0.6f, 0.95f);
			faceOffsets[1] = new Vector3(0.0f, -2.2f, 0.95f);

			// Top/bottom on Pillar 2
			faceOffsets[2] = new Vector3(-0.1f, 0.55f, 0.95f);
			faceOffsets[3] = new Vector3(-0.1f, -2.25f, 0.95f);

			// Top/bottom on Pillar 3
			faceOffsets[4] = new Vector3(0.0f, 0.35f, 0.95f);
			faceOffsets[5] = new Vector3(0.0f, -2.45f, 0.95f);

			// Top/bottom on Pillar 4
			faceOffsets[6] = new Vector3(0.0f, 0.55f, 0.95f);
			faceOffsets[7] = new Vector3(0.0f, -2.25f, 0.95f);

			// Top/bottom on Pillar 5
			faceOffsets[8] = new Vector3(0.0f, 0.6f, 0.95f);
			faceOffsets[9] = new Vector3(-0.1f, -2.2f, 0.95f);

			faceOffsetsCreated = true;
		}

		transform.parent = pillarParent.transform;

		// Set some variables
		var dmb = DanceManagerBehavior.GetInstance();
		swapped = (Random.Range(0, 2) > 0);
		Arrows = new GameObject[8];
		ArrowsGlowing = new GameObject[8];

		// Set up Tribal dance information
		Tribal.InputString = CreateRandomInputString();
		Tribal.Dance = dmb.ChooseRandomDance();

		if (Tribal.Dance != null) {
			// TODO: Add chance for broken tile based on GameLoopBehavior.level
			PlaceIcon(Tribal.Dance.Icon, swapped);
		}
		PlaceArrows(Tribal.InputString, swapped, 0);

		// Set up Explorer dance information
		Explorer.InputString = CreateRandomInputString();
		Explorer.Dance = dmb.ChooseRandomDance();

		if (Explorer.Dance != null) {
			// TODO: Add chance for broken tile based on GameLoopBehavior.level
			PlaceIcon(Explorer.Dance.Icon, !swapped);
		}
		PlaceArrows(Explorer.InputString, !swapped, 4);
	}

	private void PlaceIcon(GameObject icon, bool top) {
		var icongo = Instantiate(icon);
		icongo.name = ((top) ? "0 Top" : "1 Bottom") + " Icon";
		icongo.transform.parent = gameObject.transform;
		int index = (top ? 0 : 1) + PillarID * 2;
		icongo.transform.localPosition = faceOffsets[index] + new Vector3(0.05f, 1.1f);
	}

	// TODO(anyone): implement
	private void PlaceArrows(List<Vector2> inputString, bool top, int offset) {
		GameObject arrows = new GameObject(((top) ? "0 Top" : "1 Bottom") + " Arrows");
		arrows.transform.parent = gameObject.transform;
		int index = (top ? 0 : 1) + PillarID * 2;
		arrows.transform.localPosition = faceOffsets[index];

		float x = 1.25f;
		float increment = -0.8f;
		for (int i = 0; i < inputString.Count; ++i) {
			// Default arrow
			GameObject prefab = null;
			if (inputString[i] == InputDir.Up) prefab = ArrowPrefabs[0];
			if (inputString[i] == InputDir.Right) prefab = ArrowPrefabs[1];
			if (inputString[i] == InputDir.Down) prefab = ArrowPrefabs[2];
			if (inputString[i] == InputDir.Left) prefab = ArrowPrefabs[3];

			var newArrow = (GameObject)Instantiate(prefab);
			newArrow.name = "Arrow " + (i + offset + 1).ToString();
			newArrow.transform.parent = arrows.transform;
			newArrow.transform.localPosition = new Vector3(x, 0.0f);
			Arrows[i + offset] = newArrow;

			// Glowing arrow
			if (inputString[i] == InputDir.Up) prefab = ArrowPrefabs[4];
			if (inputString[i] == InputDir.Right) prefab = ArrowPrefabs[5];
			if (inputString[i] == InputDir.Down) prefab = ArrowPrefabs[6];
			if (inputString[i] == InputDir.Left) prefab = ArrowPrefabs[7];

			var newArrowGlow = (GameObject)Instantiate(prefab);
			newArrowGlow.name = "Arrow " + (i + offset + 1).ToString() + " Glowing";
			newArrowGlow.transform.parent = arrows.transform;
			newArrowGlow.transform.localPosition = new Vector3(x, 0.0f);
			newArrowGlow.SetActive(false);
			ArrowsGlowing[i + offset] = newArrowGlow;

			// Increment x position
			x += increment;
		}
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
		bool success = false;
		int n = 0;

		// Tribal input
		for (int i = 0; i < Tribal.InputString.Count && i < inputQueue.Count; ++i, ++n) {
			if (inputQueue[i] != Tribal.InputString[i]) {
				break;
			}
		}
		if (n < inputQueue.Count) n = 0;

		if (n == 4) success = true;
		HighlightArrows(n, 0);

		// Explorer input
		n = 0;
		for (int i = 0; i < Explorer.InputString.Count && i < inputQueue.Count; ++i, ++n) {
			if (inputQueue[i] != Explorer.InputString[i]) {
				break;
			}
		}
		if (n < inputQueue.Count) n = 0;

		if (n == 4) success = true;
		HighlightArrows(n, 4);

		return success;
	}

	// TODO(anyone): implement
	private void HighlightArrows(int n, int offset) {
		int i;
		int start = offset;
		int end = start + 4;
		n += offset;

		for (i = start; i < end; ++i) {
			Arrows[i].SetActive(true);
			ArrowsGlowing[i].SetActive(false);
		}

		for (i = start; i < n; ++i) {
			Arrows[i].SetActive(false);
			ArrowsGlowing[i].SetActive(true);
		}

		for (i = n; i < end; ++i) {
			Arrows[i].SetActive(true);
			ArrowsGlowing[i].SetActive(false);
		}
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

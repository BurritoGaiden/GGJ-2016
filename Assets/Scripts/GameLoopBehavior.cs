using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: Before shipping, Window -> Lighting -> Lightmap Tab -> Enable Continuous Baking
public class GameLoopBehavior : MonoBehaviour {

	// carroters

	// ##### GAME LOOP #####
	// 1) The Rule Blocks are created
	// 2) The tribesmen pick one of the rules and dance
	// 3) They wait for your response (for a certain amount of time)
	// 4) You respond
	// 5) They either accept the response and then good music plays and the cycle starts again, or you fail and they take one of your people away from you.

	public enum GameStates {
		SETUP,			// Before any rounds
		STARTROUND,		// Picks dance, sets up variables for that round of dancing
		TRIBEDANCE,		// Tribe is dancing
		PLAYERDANCE,	// Player is trying to match dance
		ENDROUND,		// Decides what to do with the player's response
		GAMEOVER,
		NUM,
	}
	public GameStates CurrentState;

	// Prefabs
	public GameObject PillarPrefab;
	public GameObject TribeMemberPrefab;
	public GameObject ExplorerPrefab;

	// Input handling
	private List<Vector2> inputQueue = new List<Vector2>();
	private float inputTimer = 0.0f;
	private float inputTimeout = 7.0f;
	private int inputLength = 4;
	[SerializeField]
	private PillarBehavior pillarChosen = null;

	// Lists of GameObjects
	private List<PillarBehavior> pillars = new List<PillarBehavior>();
	private List<GameObject> explorers = new List<GameObject>();

	// Song information
	private int level = 0;

	// Use this for initialization
	void Start() {
		ChangeState(GameStates.SETUP);
	}

	// Update is called once per frame
	void Update() {
		
	}

	// Called 60 times a second
	void FixedUpdate() {
		//
	}

#if UNITY_EDITOR
	private UnityEngine.Rect rect = new UnityEngine.Rect(10.0f, 100.0f, 300.0f, 50.0f);
	void OnGUI() {
		// Show the input string if C is held
		if ((pillarChosen != null) && (Input.GetKey(KeyCode.C))) {
			GUI.Label(rect, pillarChosen.GetInputString() + " " + inputTimer.ToString());
		}
	}
#endif

	// This changes the state! Wow!
	private void ChangeState(GameStates state) {
		// Debug.Log(state.ToString() + " started");

		CurrentState = state;
		switch (CurrentState) {
			case GameStates.SETUP: StartCoroutine(Setup()); break;
			case GameStates.STARTROUND: StartCoroutine(StartRound()); break;
			case GameStates.TRIBEDANCE: StartCoroutine(TribeDance()); break;
			case GameStates.PLAYERDANCE: StartCoroutine(PlayerDance()); break;
			case GameStates.ENDROUND: StartCoroutine(EndRound()); break;
			case GameStates.GAMEOVER: StartCoroutine(GameOver()); break;
		}
	}

	private IEnumerator Setup() {
		// Instantiate dancers and pillars
		float angle = 30.0f;
		float angleIncrease = 30.0f; // 30 deg, 1/6pi rad
		float circleRadius = 8.0f;
		float x = 0.0f;
		float z = 0.0f;
		for (int i = 0; i < 5; ++i) {
			x = Mathf.Cos(angle * Mathf.Deg2Rad) * circleRadius;
			z = Mathf.Sin(angle * Mathf.Deg2Rad) * circleRadius;

			GameObject explorer = Instantiate(ExplorerPrefab);
			explorer.transform.SetPositionX(x);
			explorer.transform.SetPositionZ(z);
			explorers.Add(explorer);

			GameObject tribeMember = Instantiate(TribeMemberPrefab);
			tribeMember.transform.SetPositionX(-x);
			tribeMember.transform.SetPositionZ(-z);

			x = Mathf.Cos(angle * Mathf.Deg2Rad) * 10.0f;
			z = Mathf.Sin(angle * Mathf.Deg2Rad) * 13.0f;

			angle += angleIncrease;
		}

		ChangeState(GameStates.STARTROUND);
		yield return 0;
	}

	private IEnumerator StartRound() {
		yield return 0;

		// NOTE(bret): Perhaps this should only happen every X rounds?

		// Destroy last set of pillars
		for (int i = 0; i < pillars.Count; ++i) {
			Destroy(pillars[i].gameObject);
		}
		pillars.Clear();

		// Create the pillars
		DanceManagerBehavior.Reset();
		PillarBehavior.Reset();
		float angle = 30.0f;
		float angleIncrease = 30.0f; // 30 deg, 1/6pi rad
		float x = 0.0f;
		float z = 0.0f;
		for (int i = 0; i < 5; ++i) {
			x = Mathf.Cos(angle * Mathf.Deg2Rad) * 10.0f;
			z = Mathf.Sin(angle * Mathf.Deg2Rad) * 13.0f;

			GameObject pillar = Instantiate(PillarPrefab);
			pillar.transform.SetPositionX(x);
			pillar.transform.SetPositionZ(z);
			var pillarBehavior = pillar.GetComponent<PillarBehavior>();
			pillarBehavior.Create();
			pillars.Add(pillarBehavior);

			angle += angleIncrease;
		}

		// Choose a dance
		pillarChosen = pillars[Random.Range(0, pillars.Count)];

		// Create dance input based off correct pillar
		inputQueue.Clear();

		ChangeState(GameStates.TRIBEDANCE);
	}

	private IEnumerator TribeDance() {
		yield return 0;

		ChangeState(GameStates.PLAYERDANCE);

		/*while (true) {
			yield return 0;
		}*/
	}

	private IEnumerator PlayerDance() {
		yield return 0;

		// Reset the timer
		inputTimer = inputTimeout;

		while (true) {
			// Process input
			AddInputToQueue();

			// Count down time
			inputTimer -= Time.deltaTime;

			// Check to see if time is up, the correct input has been given, or if the input is the full length
			if ((inputTimer <= 0.0f) || (pillarChosen.CheckInput(inputQueue)) || (inputQueue.Count >= inputLength)) {
				ChangeState(GameStates.ENDROUND);
				break;
			}

			yield return 0;
		}
	}

	private IEnumerator EndRound() {
		yield return 0;

		// If the input was correct
		if (pillarChosen.CheckInput(inputQueue)) {
			yield return StartCoroutine(AddToScore());
		} else {
			yield return StartCoroutine(KillExplorer()); // :(
		}

		// If there's still explorers, start round or end game
		if (explorers.Count > 0)
			ChangeState(GameStates.STARTROUND);
		else
			ChangeState(GameStates.GAMEOVER);
	}

	private IEnumerator GameOver() {
		yield return 0;

		Debug.Log("GAME FREAKIN' OVER");
	}

	// TODO(anyone): implement
	private IEnumerator AddToScore() {
		yield return 0;
	}

	// TODO(anyone): implement
	private IEnumerator KillExplorer() {
		// Temp code
		var explorer = explorers[explorers.Count - 1];
		Destroy(explorer);
		explorers.Remove(explorer);

		yield return 0;
	}

	private void AddInputToQueue() {
		// Get the proper key
		if (Input.GetKeyDown(KeyCode.LeftArrow)) inputQueue.Add(InputDir.Left);
		if (Input.GetKeyDown(KeyCode.RightArrow)) inputQueue.Add(InputDir.Right);
		if (Input.GetKeyDown(KeyCode.UpArrow)) inputQueue.Add(InputDir.Up);
		if (Input.GetKeyDown(KeyCode.DownArrow)) inputQueue.Add(InputDir.Down);
	}

}

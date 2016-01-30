using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: class Pillar

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
	private List<Vector2> inputRequired = new List<Vector2>();

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

	// This changes the state! Wow!
	private void ChangeState(GameStates state) {
		Debug.Log(state.ToString() + " started");

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
		// Instantiate dancers
		float angle = 30.0f;
		float angleIncrease = 30.0f; // 30 deg, 1/6pi rad
		float circleRadius = 8.0f;
		float pillarRadius = 13.0f;
		float x = 0.0f;
		float z = 0.0f;
		for (int i = 0; i < 5; ++i) {
			x = Mathf.Cos(angle * Mathf.Deg2Rad) * circleRadius;
			z = Mathf.Sin(angle * Mathf.Deg2Rad) * circleRadius;

			GameObject explorer = Instantiate(ExplorerPrefab);
			explorer.transform.SetPositionX(x);
			explorer.transform.SetPositionZ(z);

			GameObject tribeMember = Instantiate(TribeMemberPrefab);
			tribeMember.transform.SetPositionX(-x);
			tribeMember.transform.SetPositionZ(-z);

			x = Mathf.Cos(angle * Mathf.Deg2Rad) * pillarRadius;
			z = Mathf.Sin(angle * Mathf.Deg2Rad) * pillarRadius;

			GameObject pillar = Instantiate(PillarPrefab);
			pillar.transform.SetPositionX(x);
			pillar.transform.SetPositionZ(z);

			angle += angleIncrease;
		}

		ChangeState(GameStates.STARTROUND);
		yield return 0;
	}

	private IEnumerator StartRound() {
		yield return 0;

		// TODO: Create the pillars

		// TODO: Choose a dance

		// Create dance input based off correct pillar
		inputQueue.Clear();
		inputRequired.Clear();
		inputRequired.Add(InputDir.Left);
		inputRequired.Add(InputDir.Left);
		inputRequired.Add(InputDir.Left);
		inputRequired.Add(InputDir.Right);

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

		while (true) {
			AddInputToQueue();
			if (CheckInput()) {
				ChangeState(GameStates.ENDROUND);
				break;
			}
			yield return 0;
		}
	}

	private IEnumerator EndRound() {
		yield return 0;

		// If the input was correct
		if (CheckInput()) {
			yield return AddToScore();
		} else {
			yield return KillExplorer(); // :(
		}

		// if (players.count > 0)
		ChangeState(GameStates.STARTROUND);
		// else
		// ChangeState(GameStates.GAMEOVER);
	}

	private IEnumerator GameOver() {
		yield return 0;
	}

	// TODO: implement
	private IEnumerator AddToScore() {
		yield return 0;
	}

	// TODO: implement
	private IEnumerator KillExplorer() {
		yield return 0;
	}

	private void AddInputToQueue() {
		if (Input.GetKeyDown(KeyCode.LeftArrow)) inputQueue.Add(InputDir.Left);
		if (Input.GetKeyDown(KeyCode.RightArrow)) inputQueue.Add(InputDir.Right);
		if (Input.GetKeyDown(KeyCode.UpArrow)) inputQueue.Add(InputDir.Up);
		if (Input.GetKeyDown(KeyCode.DownArrow)) inputQueue.Add(InputDir.Down);

		for (int i = 0; i < Temp.cubes.Count; ++i) {
			Temp.cubes[i].transform.SetPositionZRelative(1.0f);
		}
	}

	private bool CheckInput() {
		if (inputQueue.Count != inputRequired.Count)
			return false;

		bool success = true;
		for (int i = 0; i < inputRequired.Count; ++i) {
			if (inputQueue[i] != inputRequired[i]) {
				success = false;
				Debug.Log("WRGON!");
			}
		}

		return success;
	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

[System.Serializable]
public class Something {
	public GameObject Prefab;
	public Vector3 Position;
}

// TODO(bret): Before shipping, Window -> Lighting -> Lightmap Tab -> Enable Continuous Baking
public class GameLoopBehavior : MonoBehaviour {

	public GameStates CurrentState;

	// Prefabs
	public GameObject SceneObject;
	public GameObject GoldPile;
	public Something[] TribeMemberPrefabs;
	public Something[] ExplorerPrefabs;

	// Input handling
	private List<Vector2> inputQueue = new List<Vector2>();
	private float inputTimer = 0.0f;
	private int inputLength = 4;
	[SerializeField]
	private PillarBehavior pillarChosen = null;

	// Lists of GameObjects
	private List<PillarBehavior> pillars = new List<PillarBehavior>();
	private List<GameObject> explorers = new List<GameObject>();
	private List<GameObject> tribalMembers = new List<GameObject>();

	// Song information
	private int level = 0;

	private bool firstRun = true;

	private LineRenderer laserLineRenderer;

	public AudioClip LaserSound;
	private AudioSource laserAudioSource;

	public AudioClip[] Screams;
	private AudioSource screamAudioSource;

	public AudioClip RumbleSound;
	private AudioSource rumbleAudioSource;

	public AudioClip CoinsSound;
	private AudioSource coinsAudioSource;

#if UNITY_ANDROID
	public float minSwipeDistX;
	public float minSwipeDistY;
	private Vector2 swipeStartPos;
#endif

#if UNITY_EDITOR
	private int progress = 0;
	private float progressElapsed = 0.0f;
	private float progressTotal = 1.0f;
#endif

	// Use this for initialization
	void Start() {
		Messenger.Instance.Listen(ListenerType.ADDEDTOPOT, this);

		ChangeState(GameStates.SETUP);

		laserLineRenderer = GetComponent<LineRenderer>();

		laserAudioSource = gameObject.AddComponent<AudioSource>();
		laserAudioSource.clip = LaserSound;

		screamAudioSource = gameObject.AddComponent<AudioSource>();

		rumbleAudioSource = gameObject.AddComponent<AudioSource>();
		rumbleAudioSource.clip = RumbleSound;

		coinsAudioSource = gameObject.AddComponent<AudioSource>();
		coinsAudioSource.clip = CoinsSound;

#if UNITY_ANDROID
		float smaller = Mathf.Min(Screen.width, Screen.height);
		minSwipeDistX = minSwipeDistY = smaller * 0.3f;
#endif
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			new MessageScreenshake() {
				Amount = 5.0f
			}.Send();
		}

		if (Input.GetKeyDown(KeyCode.S))
			ChangeState(((GameStates)(int)CurrentState + 1));
	}

	// Called 60 times a second
	void FixedUpdate() {
		//
	}

	public void _AddedToPot(MessageAddedToPot msg) {
		screamAudioSource.clip = Screams[Random.Range(0, Screams.Length)];
		screamAudioSource.Play();
	}

#if UNITY_EDITOR
	private UnityEngine.Rect rect = new Rect(10.0f, 100.0f, 300.0f, 50.0f);
	private UnityEngine.Rect progressRect = new Rect(10.0f, 10.0f, 0.0f, 25.0f);
	private float progressWidth = 300.0f;
	void OnGUI() {
		// Show the input string if C is held
		if ((pillarChosen != null) && (Input.GetKey(KeyCode.C))) {
			GUI.Label(rect, pillarChosen.GetInputString() + " " + inputTimer.ToString());
		}

		float y = 10.0f;
		float increment = 35.0f;

		for (int i = 0; i <= progress; ++i) {
			progressRect.y = y;
			progressRect.width = progressWidth;
			GUI.Box(progressRect, GetTitle(i));
			y += increment;
		}

		progressRect.y = y;
		progressRect.width = progressWidth * Util.GetPercentBetween(progressElapsed, 0.0f, progressTotal);
		GUI.Box(progressRect, GetTitle(progress + 1));
	}

	private string GetTitle(int n) {
		switch (n) {
			case 0: return "";
			case 1: return "Tribal dance";
			case 2: return "Explorer dance";
			case 3: return "Results";
			default: return "FUCK";
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

		new MessageChangeState() {
			GameState = CurrentState
		}.Send();
	}

	private IEnumerator Setup() {
		yield return new WaitForSeconds(2.0f);

		// Instantiate dancers
		GameObject characters = new GameObject("Characters");
		GameObject explorersParent = new GameObject("Explorers");
		explorersParent.transform.parent = characters.transform;
		GameObject tribeMembersParent = new GameObject("Tribe Members");
		tribeMembersParent.transform.parent = characters.transform;

		for (int i = 0; i < ExplorerPrefabs.Length; ++i) {
			GameObject explorer = Instantiate(ExplorerPrefabs[i].Prefab);
			explorer.transform.position = ExplorerPrefabs[i].Position;
			explorer.transform.SetEulerAngleY(180.0f);
			explorers.Add(explorer);
			explorer.transform.parent = explorersParent.transform;
		}

		for (int i = 0; i < TribeMemberPrefabs.Length; ++i) {
			GameObject tribeMember = Instantiate(TribeMemberPrefabs[i].Prefab);
			tribeMember.transform.position = TribeMemberPrefabs[i].Position;
			tribeMember.transform.parent = tribeMembersParent.transform;
			tribalMembers.Add(tribeMember);
		}

		ChangeState(GameStates.STARTROUND);
		yield return 0;

		// Play Music
		if (firstRun) {
			Settings.Volume = 0.5f;
			firstRun = false;
		}
		Music.PlayTracks();
	}

	private IEnumerator StartRound() {
		// NOTE(bret): Perhaps this should only happen every X rounds?

		// Destroy last set of pillars
		for (int i = 0; i < pillars.Count; ++i) {
			Destroy(pillars[i].gameObject);
		}
		pillars.Clear();

		// Create the pillars
		DanceManagerBehavior.Reset();
		PillarBehavior.Reset();

		// Create the Pillars from the Pillars in the scene
		int id = 0;
		foreach (Transform child in SceneObject.transform) {
			if (child.name.StartsWith("Pillar")) {
				GameObject pillarPrefab = child.gameObject;
				if (pillarPrefab.GetComponent<PillarBehavior>() == null)
					continue;

				GameObject pillar = Instantiate(pillarPrefab);
				pillar.SetActive(true);
				var pillarBehavior = pillar.GetComponent<PillarBehavior>();
				if (pillarBehavior == null)
					Debug.Log("WHAT THE FUCK");
				pillarBehavior.PillarID = id;
				pillarBehavior.Create();
				pillars.Add(pillarBehavior);
				++id;
			}
		}

		// Choose a dance
		pillarChosen = pillars[Random.Range(0, pillars.Count)];

		// Create dance input based off correct pillar
		inputQueue.Clear();

		ChangeState(GameStates.TRIBEDANCE);

		yield return 0;
	}

	private IEnumerator TribeDance() {
		yield return 0;

		rumbleAudioSource.Play();

		foreach (GameObject explorer in explorers) {
			explorer.GetComponent<ExplorerBehavior>().PlayAnimation("Idle");
		}

		foreach (GameObject tribeMember in tribalMembers) {
			tribeMember.GetComponent<ExplorerBehavior>().PlayAnimation(pillarChosen.Tribal.Dance.AnimationName);
		}

#if UNITY_EDITOR
		float total = Music.GetTrackLength(MusicFiles.TRIBAL);
		for (float elapsed = 0.0f; elapsed < total; elapsed += Time.deltaTime) {
			progress = 0;
			progressElapsed = elapsed;
			progressTotal = total;
			yield return 0;
		}
#else
		yield return new WaitForSeconds(Music.GetTrackLength(MusicFiles.TRIBAL));
#endif

		ChangeState(GameStates.PLAYERDANCE);
	}

	private IEnumerator PlayerDance() {
		// Reset the timer
		inputTimer = Music.GetTrackLength(MusicFiles.EXPLORER);

		foreach (GameObject tribeMember in tribalMembers) {
			tribeMember.GetComponent<ExplorerBehavior>().PlayAnimation("Idle");
		}

		while (true) {
			// Process input
			AddInputToQueue();

			// Count down time
			inputTimer -= Time.deltaTime;

#if UNITY_EDITOR
			progress = 1;
			progressTotal = Music.GetTrackLength(MusicFiles.EXPLORER);
			progressElapsed = progressTotal - inputTimer;
#endif

			// "Check input" to highlight the things
			for (int i = 0; i < pillars.Count; ++i) {
				pillars[i].CheckInput(inputQueue);
			}

			// Check to see if time is up, the correct input has been given, or if the input is the full length
			if ((inputTimer <= 0.0f) || (pillarChosen.CheckInput(inputQueue)) || (inputQueue.Count >= inputLength)) {
				break;
			} else
				yield return 0;
		}

		while (true) {
			// Once time has run out, change the state!
			if (inputTimer <= 0.0f) {
				ChangeState(GameStates.ENDROUND);
				break;
			}

			yield return 0;

			// Count down time
			inputTimer -= Time.deltaTime;

#if UNITY_EDITOR
			progress = 1;
			progressTotal = Music.GetTrackLength(MusicFiles.EXPLORER);
			progressElapsed = progressTotal - inputTimer;
#endif
		}
	}

	private IEnumerator EndRound() {
		foreach (GameObject explorer in explorers) {
			explorer.GetComponent<ExplorerBehavior>().PlayAnimation(pillarChosen.Explorer.Dance.AnimationName);
		}

		rumbleAudioSource.PlayDelayed(2.5f);

		bool inputCorrect = pillarChosen.CheckInput(inputQueue);
		Music.InputCorrect(inputCorrect);

#if UNITY_EDITOR
		/*float total = Music.GetTrackLength(MusicFiles.WIN);
		for (float elapsed = 0.0f; elapsed < total; elapsed += Time.deltaTime) {
			progress = 2;
			progressElapsed = elapsed;
			progressTotal = total;
			yield return 0;
		}*/
#endif

		// If the input was correct
		if (inputCorrect) {
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
		yield return new WaitForSeconds(3.0f);

		Application.Quit();
	}

	// TODO(anyone): implement
	private IEnumerator AddToScore() {
		foreach (GameObject tribeMember in tribalMembers) {
			tribeMember.GetComponent<ExplorerBehavior>().PlayAnimation(pillarChosen.Tribal.Dance.AnimationName);
		}

		// Remove the explorer from the List and give them GOLD!
		var explorer = explorers[explorers.Count - 1];
		explorers.Remove(explorer);
		Instantiate(GoldPile, explorer.transform.position, Quaternion.identity);
		coinsAudioSource.Play();

		yield return new WaitForSeconds(Music.GetTrackLength(MusicFiles.WIN));

		yield return 0;
	}

	float beamChargeTimer = 0.0f;
	float beamChargeLength = 2.0f;
	float beamShootLength = 0.2f;

	// TODO(anyone): implement
	private IEnumerator KillExplorer() {
		float timeLeft = Music.GetTrackLength(MusicFiles.LOSE);

		// Play laser sound
		laserAudioSource.Play();

		// Glow
		for (beamChargeTimer = 0.0f; beamChargeTimer < beamChargeLength; beamChargeTimer += Time.deltaTime) {
			timeLeft -= Time.deltaTime;

			// TODO(bret): Beam charge

			yield return 0;
		}
		
		// Shoot
		var explorer = explorers[explorers.Count - 1];
		explorers.Remove(explorer);
		explorer.GetComponent<ExplorerBehavior>().Die();

		new MessageScreenshake() {
			Amount = 5.0f
		}.Send();

		laserLineRenderer.SetPosition(1, explorer.transform.position);
		laserLineRenderer.enabled = true;
		for (float t = 0.0f; t < beamShootLength; t += Time.deltaTime) {
			timeLeft -= Time.deltaTime;
			yield return 0;
		}
		laserLineRenderer.enabled = false;

		yield return 0;
		

		// Wait
		for (; timeLeft > 0; timeLeft -= Time.deltaTime) {
			yield return 0;
		}

		yield return 0;
	}

	private void AddInputToQueue() {
		// Get input
		if (Input.GetKeyDown(KeyCode.LeftArrow)) inputQueue.Add(InputDir.Left);
		if (Input.GetKeyDown(KeyCode.RightArrow)) inputQueue.Add(InputDir.Right);
		if (Input.GetKeyDown(KeyCode.UpArrow)) inputQueue.Add(InputDir.Up);
		if (Input.GetKeyDown(KeyCode.DownArrow)) inputQueue.Add(InputDir.Down);

#if UNITY_ANDROID
		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];

			float swipeValue;
			switch (touch.phase) {
				case TouchPhase.Began:
					swipeStartPos = touch.position;
					break;
				case TouchPhase.Ended:
					float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, swipeStartPos.y, 0)).magnitude;

					if (swipeDistVertical > minSwipeDistY) {
						swipeValue = Mathf.Sign(touch.position.y - swipeStartPos.y);

						if (swipeValue > 0.0f)//up swipe
							inputQueue.Add(InputDir.Up);
						else if (swipeValue < 0.0f)//down swipe
							inputQueue.Add(InputDir.Down);
					}

					float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(swipeStartPos.x, 0, 0)).magnitude;

					if (swipeDistHorizontal > minSwipeDistX) {
						swipeValue = Mathf.Sign(touch.position.x - swipeStartPos.x);

						if (swipeValue > 0.0f)//right swipe
							inputQueue.Add(InputDir.Right);
						else if (swipeValue < 0.0f)//left swipe
							inputQueue.Add(InputDir.Left);
					}
					break;
			}
		}
#endif
	}

}

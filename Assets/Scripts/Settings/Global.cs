using UnityEngine;
using System.Collections;

// TODO(anyone): Temp using, remove these
using System.Collections.Generic;

// A place to store nasty, temporary global variables
public static class Temp {
	
}

public static class Global {

	

}

public static class InputDir {

	public static readonly Vector2 Left = -1.0f * Vector2.right;
	public static readonly Vector2 Right = Vector2.right;
	public static readonly Vector2 Up = Vector2.up;
	public static readonly Vector2 Down = -1.0f * Vector2.up;

}

public enum ListenerType {

	NUM,
}

public struct Scenes {
	public const string GAMELOOPSCENE = "GameLoopScene";
	public const string MAINMENUSCENE = "MainMenuScene";
}

public struct Tags {
	public const string RESPAWN = "Respawn";
	public const string FINISH = "Finish";
	public const string EDITORONLY = "EditorOnly";
	public const string MAINCAMERA = "MainCamera";
	public const string PLAYER = "Player";
	public const string GAMECONTROLLER = "GameController";
	public const string MUSICCONTROLLER = "MusicController";
	public const string DANCEMANAGER = "DanceManager";
}

public struct Layers {
	public const int DEFAULT = 0;
	public const int UI = 5;
}
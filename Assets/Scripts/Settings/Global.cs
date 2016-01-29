using UnityEngine;
using System.Collections;

// A place to store nasty, temporary global variables
public static class Temp {

}

public static class Global {

	

}

public enum ListenerType {

	NUM,
}

public struct Scenes {
	
}

public struct Tags {
	public const string RESPAWN = "Respawn";
	public const string FINISH = "Finish";
	public const string EDITORONLY = "EditorOnly";
	public const string MAINCAMERA = "MainCamera";
	public const string PLAYER = "Player";
	public const string GAMECONTROLLER = "GameController";
	public const string MUSICCONTROLLER = "Music Controller";
}

public struct Layers {
	public const int DEFAULT = 0;
	public const int UI = 5;
}
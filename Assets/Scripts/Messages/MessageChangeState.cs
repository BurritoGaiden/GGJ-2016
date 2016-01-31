using UnityEngine;
using System.Collections;

public class MessageChangeState : Message {

	public GameStates GameState;

	public MessageChangeState()
		: base(ListenerType.CHANGESTATE) {
		
	}

}

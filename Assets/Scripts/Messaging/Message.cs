using UnityEngine;
using System.Collections;

public class Message : System.Object {

	public ListenerType ListenerType;
	public string FunctionName;

	public Message(ListenerType type) {
		ListenerType = type;

		FunctionName = "_" + GetType().Name.Substring(7);
	}

	public void Send() {
		Messenger.Instance.Send(this);
	}

}

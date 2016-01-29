using UnityEngine;
using System.Collections;

public class Messenger {

	Hashtable Listeners = new Hashtable();

	// TODO: Implement this
	//private Hashtable MessageTypeCount = new Hashtable();

	private static Messenger instance;
	public static Messenger Instance {
		get {
			if (instance == null)
				instance = new Messenger();
			return instance;
		}
	}
	// TODO: What the hell is Use?
	static Messenger Use;

	public void Listen(ListenerType listenerType, GameObject gameObject) {
		if (Listeners[listenerType] == null) {
			Listeners[listenerType] = new ArrayList();
		}

		ArrayList list = Listeners[listenerType] as ArrayList;

		if (!list.Contains(gameObject)) {
			list.Add(gameObject);
		}
	}

	public void Listen(ListenerType listenerType, Component component) {
		Listen(listenerType, component.gameObject);
	}

	public void StopListen(ListenerType listenerType, GameObject gameObject) {
		ArrayList list = Listeners[listenerType] as ArrayList;

		if (list != null) {
			list.Remove(gameObject);
		}
	}

	public void Send(Message msg) {
		ArrayList list = Listeners[msg.ListenerType] as ArrayList;

		if (list != null) {
			for (int i = 0; i < list.Count; ++i) {
				GameObject listener = list[i] as GameObject;

				if (listener != null) {
					listener.SendMessage(msg.FunctionName, msg, SendMessageOptions.DontRequireReceiver);
				} else {
					list.RemoveAt(i--);
				}
			}
		}
	}
}

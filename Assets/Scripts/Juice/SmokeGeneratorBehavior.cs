using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmokeGeneratorBehavior : MonoBehaviour {

	public class ParticleBehavior : MonoBehaviour {

		private float scale {
			set {
				transform.localScale = new Vector3(value, value, value);
			}
			get {
				return transform.localScale.x;
			}
		}

		private float color {
			set {
				float ivalue = value + 0.55f;
				rend.material.color = new Color(ivalue, ivalue, ivalue, value);
			}
			get {
				return rend.material.color.a;
			}
		}

		private float xspeed = 0;
		private float yspeed = 0;
		private float zspeed = 0;
		private float maxYSpeed = 0.0f;
		private Renderer rend;

		public bool Dead {
			get { return transform.GetPositionY() > 10.0f; }
		}

		public void UpdateValues(float delta) {
			var decrement = 0.3f;
			xspeed -= decrement * delta * Mathf.Sign(xspeed);
			yspeed += Random.Range(0.4f, 0.6f) * delta;
			yspeed = Util.Clamp(yspeed, 0.0f, maxYSpeed);
			zspeed -= decrement * delta * Mathf.Sign(zspeed);
			transform.SetPositionXRelative(xspeed * delta);
			transform.SetPositionYRelative(yspeed * delta);
			transform.SetPositionZRelative(zspeed * delta);
			color -= 1.2f * delta;
		}

		public void Reset() {
			transform.position = Vector3.zero;
			scale = Random.Range(0.1f, 0.25f);
			var speed = Random.Range(0.2f, 0.22f) * 5.0f;
			var angle = Random.Range(0.0f, Mathf.PI * 2);
			xspeed = Mathf.Cos(angle) * speed;
			zspeed = Mathf.Sin(angle) * speed;

			maxYSpeed = Random.Range(1.0f, 1.25f);

			transform.SetPositionX(xspeed);
			transform.SetPositionZ(zspeed);

			rend = GetComponent<Renderer>();
			color = 1.0f;
		}

	}

	public int NumToCreateOnStart = 100;
	public int NumCreated = 0;
	public int NumToCreatePerSecond;
	private List<GameObject> particles = new List<GameObject>();
	private List<GameObject> particlesRecycled = new List<GameObject>();

	// Use this for initialization
	void Start() {
		int perLoop = 3;
		for (int i = 0; i < NumToCreateOnStart; i += perLoop) {
			for (int j = 0; j < perLoop; ++j)
				AddParticle();
			UpdateParticles(0.01f);
		}
	}

	// Update is called once per frame
	void Update() {
		UpdateParticles(Time.deltaTime);

		int amountToCreate = Mathf.FloorToInt(NumToCreatePerSecond * Time.deltaTime);
		for (int i = 0; i < amountToCreate; ++i) {
			AddParticle();
		}

		while (particles.Count > NumToCreateOnStart) {
			RecycleParticle(particles[0]);
		}
	}

	private void UpdateParticles(float delta) {
		for (int i = 0; i < particles.Count; ++i) {
			var pb = particles[i].GetComponent<ParticleBehavior>();
			
			pb.UpdateValues(delta);

			if (pb.Dead) {
				RecycleParticle(particles[i--]);
			}
		}
	}

	private void RecycleParticle(GameObject particle) {
		particles.Remove(particle);
		particle.SetActive(false);
		particlesRecycled.Add(particle);
	}

	private GameObject AddParticle() {
		GameObject particle;

		if (particlesRecycled.Count > 0) {
			particle = particlesRecycled[0];
			particlesRecycled.RemoveAt(0);
			particle.SetActive(true);
			particles.Add(particle);
		} else {
			particle = GameObject.CreatePrimitive(PrimitiveType.Cube);
			particle.AddComponent<ParticleBehavior>();
			particle.transform.parent = gameObject.transform;
			particles.Add(particle);
		}

		// Set up some variables

		particle.GetComponent<ParticleBehavior>().Reset();
		NumCreated = particles.Count;

		return particle;
	}

}

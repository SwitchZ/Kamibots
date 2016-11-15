using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {

	public delegate bool ItemCollected(Item item);
	public static event ItemCollected OnItemCollected;

	private Rigidbody2D body;
	private Animator animator;

	[Header ("Hero Attributes")]
	public float speed;

	[Header ("References")]
	public GameManager gameManager;
	
	void Start () {
		this.body = this.GetComponent<Rigidbody2D> ();
		this.animator = this.GetComponent<Animator> ();
	}

	void Update () {
	}

	public void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("Item") && OnItemCollected != null) {
			Item item = other.GetComponent<Item>();
			OnItemCollected(item);
			other.gameObject.SetActive(false);
		}
	}

}

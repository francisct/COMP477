using UnityEngine;
using System.Collections;

public class RagdollMovement : MonoBehaviour {

    Rigidbody torso;
    public float speed;

	// Use this for initialization
	void Start () {
        torso = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Animate(h, v);
	}

    void Move(Vector3 direction)
    {
        torso.AddForce(speed * direction, ForceMode.Impulse);
    }

    void Animate(float h, float v)
    {
        Vector3 direction = new Vector3(h, 0.0f, v);
        Move(direction);
    }
}

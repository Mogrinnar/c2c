using UnityEngine;
using System.Collections;

public class ThirdPersonController : MonoBehaviour 
{
	public AudioClip shoutingClip;		// Audio clip of the player shouting.
	public float turnSmoothing = 15f;	// A smoothing value for turning the player.
	public float speedDampTime = 0.1f;	// The damping for the speed parameter

	public float movingSpeed = 1.0f;
	public Camera followingCamera;

	private Rigidbody _rigidbody;
	private Transform _transfrom;
	private Vector3 _initialLocalPosition = Vector3.zero;
	
	void Awake ()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_transfrom = GetComponent<Transform>();
	}

	void Start()
	{
		_initialLocalPosition = _transfrom.localPosition;
	}
	
	void FixedUpdate ()
	{
		// Cache the inputs.
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		bool sneak = Input.GetButton("Sneak");
		
		MovementManagement(h, v, sneak);
	}
	
	
	void Update ()
	{

	}

	void MovementManagement (float horizontal, float vertical, bool sneaking)
	{		
		// If there is some axis input...
		if(horizontal != 0f || vertical != 0f)
		{
			Debug.Log("horizontal : " + horizontal.ToString() + ". vertical : "  + vertical.ToString());

			Moving(horizontal,vertical, sneaking);
			Rotating(horizontal, vertical);
		}
	}

	private void Moving(float horizontal, float vertical, bool sneaking)
	{
		Vector3 newDirection = new Vector3(horizontal, 0.0f, vertical);
		//newDirection.Normalize();
		newDirection = newDirection * movingSpeed;

		Vector3 newPosition = _transfrom.localPosition + newDirection;
		_rigidbody.MovePosition(newPosition);
		MovingCamera(newPosition);
	}

	private void MovingCamera(Vector3 newPlayerPosition)
	{
		Vector3 previousCamPos = followingCamera.GetComponent<Transform>().localPosition;
		followingCamera.GetComponent<Transform>().localPosition = new Vector3(newPlayerPosition.x, previousCamPos.y, newPlayerPosition.z);
	}
	
	void Rotating (float horizontal, float vertical)
	{
		// Create a new vector of the horizontal and vertical inputs.
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		
		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		
		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
		Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
		
		// Change the players rotation to this new rotation.
		_rigidbody.MoveRotation(newRotation);
	}
}

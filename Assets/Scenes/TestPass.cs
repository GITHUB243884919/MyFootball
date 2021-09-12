using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPass : MonoBehaviour
{
	[SerializeField]
	float _friction = 3f;

	[SerializeField]
	float finalVelocity = 13;

	public Transform target;

	public Rigidbody Rigidbody;



	public Vector3 NormalizedPosition {
		get {
			return new Vector3(transform.position.x, 0f, transform.position.z);
		}

		set {
			transform.position = new Vector3(value.x, 0f, value.z);
		}
	}


	private void Awake()
	{
		_friction = _friction < 0 ? _friction : -1 * _friction;
	}
	// Start is called before the first frame update
	void Start()
    {
		//float power = FindPower(NormalizedPosition, target.position , finalVelocity);
		float power = FindPower(transform.position, target.position, finalVelocity);

		float time = TimeToTarget(transform.position,
		   target.position,
		   power,
		   _friction);
		Debug.LogError(time);

		KickToPoint(target.position, power);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
		ApplyFriction();
	}

	public float FindPower(Vector3 from, Vector3 to, float finalVelocity)
	{
		// v^2 = u^2 + 2as => u^2 = v^2 - 2as => u = root(v^2 - 2as)
		return Mathf.Sqrt(Mathf.Pow(finalVelocity, 2f) - (2 * _friction * Vector3.Distance(from, to)));
	}

	public void KickToPoint(Vector3 to, float power)
	{
		//Vector3 direction = to - NormalizedPosition;
		Vector3 direction = to - transform.position;
		direction.Normalize();

		//change the velocity
		//direction.y = 0.015f;
		Rigidbody.velocity = direction * power;

		////invoke the ball launched event
		//BallLaunched temp = OnBallLaunched;
		//if (temp != null)
		//	temp.Invoke(0f, power, NormalizedPosition, to);
	}

	public void ApplyFriction()
	{
		//get the direction the ball is travelling
		Vector3 _frictionVector = Rigidbody.velocity.normalized;
		_frictionVector.y = 0f;

		//calculate the actual friction
		_frictionVector *= _friction;

		////calculate the raycast start positiotn
		//_rayCastStartPosition = transform.position + SphereCollider.radius * Vector3.up;

		////check if the ball is touching with the pitch
		////if yes apply the ground friction force
		////else apply the air friction
		//_isGrounded = Physics.Raycast(_rayCastStartPosition,
		//	Vector3.down,
		//	out _hit,
		//	_rayCastDistance,
		//	_groundMask);

		////apply friction if grounded
		//if (_isGrounded)
			Rigidbody.AddForce(_frictionVector);

//#if UNITY_EDITOR
//		Debug.DrawRay(_rayCastStartPosition,
//			Vector3.down * _rayCastDistance,
//			Color.red);
//#endif

	}

	/// <summary>
	/// Calculates the time it will take to reach the target
	/// </summary>
	/// <param name="inital">start position</param>
	/// <param name="target">final position</param>
	/// <param name="initialVelocity">initial velocity</param>
	/// <param name="acceleration">force acting aginst motion</param>
	/// <returns></returns>
	public float TimeToTarget(Vector3 initial, Vector3 target, float velocityInitial, float acceleration)
	{
		//using  v^2 = u^2 + 2as 
		float distance = Vector3.Distance(initial, target);
		float uSquared = Mathf.Pow(velocityInitial, 2f);
		float v_squared = uSquared + (2 * acceleration * distance);

		//if v_squared is less thaSn or equal to zero it means we can't reach the target
		if (v_squared <= 0)
			return -1.0f;

		//find the final velocity
		float v = Mathf.Sqrt(v_squared);

		//find time to travel 
		return TimeToTravel(velocityInitial, v, acceleration);
	}

	public float TimeToTravel(float initialVelocity, float finalVelocity, float acceleration)
	{
		// t = v-u
		//     ---
		//      a
		float time = (finalVelocity - initialVelocity) / acceleration;

		//return result
		return time;
	}
}

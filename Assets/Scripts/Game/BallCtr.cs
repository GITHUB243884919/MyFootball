using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Soccer
{
	public class BallCtr : UFrame.Common.SingletonMono<BallCtr>
	{
		public GameObject owner;

		public float velocity = 1f;

		Rigidbody rigidbody;
		// Start is called before the first frame update
		void Start()
		{
			InitWithOwner();

			rigidbody = gameObject.GetComponent<Rigidbody>();
		}

		// Update is called once per frame
		void Update()
		{
			float fVertical = Input.GetAxis("Vertical");
			float fHorizontal = Input.GetAxis("Horizontal");

			if (fVertical != 0)
			{
				Debug.LogFormat("fVertical {0}", fVertical);
			}


			if (fHorizontal != 0)
			{
				Debug.LogFormat("fHorizontal {0}", fHorizontal);
			}

			if (Input.GetKey(KeyCode.S))
			{
				Debug.LogFormat("key {0}", KeyCode.S);
			}

			if (Input.GetKey(KeyCode.D))
			{
				Debug.LogFormat("key {0}", KeyCode.D);
				//Shoot();
			}

			//KeepRotate();
		}

		void InitWithOwner()
		{
			transform.position = owner.transform.position + owner.transform.forward / 1.5f + owner.transform.up / 5.0f;
		}

		public void KeepRotate()
		{
			transform.Rotate(owner.transform.right, velocity * 200.0f);
		}

		public void Shoot()
		{
			ShowTrail(true);
			rigidbody.velocity = new Vector3(owner.transform.forward.x * 30.0f, 10.0f,
				owner.transform.forward.z * 30.0f);
		}

		public void Pass(SoccerPlayerCtr curr, SoccerPlayerCtr next)
		{
			ShowTrail(true);
			Vector3 velocity = Vector3.zero;

			velocity = (next.transform.position - curr.transform.position);
			gameObject.GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, 5f, velocity.z);

		}

		public void ShowTrail(bool isShow)
		{
			GetComponent<TrailRenderer>().enabled = isShow;
		}

		public void StopBall()
		{
			rigidbody.velocity = Vector3.zero;
		}
	}

}

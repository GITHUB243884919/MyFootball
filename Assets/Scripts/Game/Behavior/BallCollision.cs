using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UFrame;
using UnityEngine;
using UFrame.Common;

namespace Game.Soccer
{
	public class BallCollision : SingletonMono<BallCollision>
	{
		public Transform ownBallPlayer { get; set; }

		/// <summary>
		/// 检查球场边界的层
		/// </summary>
		int layerCheckField;

		private void Awake()
		{
			layerCheckField = LayerMask.NameToLayer("CheckField");
		}

		void OnCollisionEnter(Collision collision)
		{
			ownBallPlayer = collision.gameObject.transform;
			//Debug.LogError(collision.gameObject.name);
		}

		//private void OnTriggerEnter(Collider other)
		//{

		//	if (other.gameObject.layer == layerCheckField)
		//	{
		//		MessageManager.GetInstance().Send((int)GameMessageDefine.BallOutOfField);
		//	}

		//}
	}

}

using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UFrame;
using UnityEngine;

namespace Game.Soccer
{
	public class CheckFieldCollision : MonoBehaviour
	{
		/// <summary>
		/// 检查球场边界的层
		/// </summary>
		int layerBall;

		private void Awake()
		{
			layerBall = LayerMask.NameToLayer("Ball");
		}


		private void OnTriggerEnter(Collider other)
		{

			if (other.gameObject.layer == layerBall)
			{
				MessageManager.GetInstance().Send((int)GameMessageDefine.BallOutOfField);
			}

		}
	}

}

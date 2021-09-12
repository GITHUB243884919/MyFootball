using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UFrame;
using UFrame.MessageCenter;
using UnityEngine;

namespace Game.Soccer.Behavior
{
	/// <summary>
	/// 往前跑
	/// </summary>
	[TaskCategory("MySoccer/Match")]
	public class MatchRuning : Action
	{
		/// <summary>
		/// 死球
		/// </summary>
		bool isOutOfPlay = false;

		public override void OnAwake()
		{
			base.OnAwake();

			isOutOfPlay = false;
		}

		public override void OnStart()
		{
			base.OnStart();

			MessageManager.GetInstance().Regist((int)GameMessageDefine.BallOutOfField, OnBallOutOfField);
		}

		public override void OnEnd()
		{
			base.OnEnd();
			
			MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BallOutOfField, OnBallOutOfField);
		}

		public override TaskStatus OnUpdate()
		{
			//是否死球，判定球是否出界
			if (isOutOfPlay)
			{
				return TaskStatus.Failure;
			}


			if (MatchDataManager.GetInstance().OwnerBall() == null)
			{
				//BallCtr.GetInstance().StopBall();
				return TaskStatus.Running;

			}
			//BallCtr.GetInstance().StopBall();

			//var player = MatchDataManager.GetInstance().GetAttackTeamCouldShootOrPass();
			//if (player != null)
			//{
			//	return TaskStatus.Failure;
			//}

			return TaskStatus.Running;
		}

		void OnBallOutOfField(Message msg)
		{
			isOutOfPlay = true;
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UFrame;
using UFrame.MessageCenter;
using UnityEngine;
using DG.Tweening;

namespace Game.Soccer.Behavior
{

	/// <summary>
	/// 游戏暂停
	/// 确定操作的球员
	/// 确定操作的方式
	/// 确定根据以上定UI
	/// 
	/// MatchPause和MatchRuning的转换
	/// MatchPause=>MatchRuning
	///		手指完成划线操作，鼠标抬起
	/// 
	/// MatchRuning=>MatchPause
	///		死球
	///		有球员拥有球，且只能传球和射门
	/// 
	/// </summary>
	[TaskCategory("MySoccer/Match")]
	public class MatchPause : Action
	{

		public SharedBool isOnePlayerOwnBall;

		public SharedTransform ownBallPlayer;


		public GameObject ball;

		Rigidbody ballRigidbody;

		SoccerPlayerCtr player = null;

		bool isFinishManualOperation = false;

		public override void OnAwake()
		{
			base.OnAwake();

			ballRigidbody = ball.GetComponent<Rigidbody>();
		}

		public override void OnStart()
		{
			base.OnStart();

			MessageManager.GetInstance().Regist((int)GameMessageDefine.FinishManualOperation, OnFinishManualOperation);

			isFinishManualOperation = false;

			BallCtr.GetInstance().ShowTrail(false);
		}

		public override void OnEnd()
		{
			base.OnEnd();

			MessageManager.GetInstance().UnRegist((int)GameMessageDefine.FinishManualOperation, OnFinishManualOperation);
		}

		public override TaskStatus OnUpdate()
		{
			if (CouldInThisAction())
			{
				if (player.currActionType == PlayerActionType.Pass)
				{
					MatchDataManager.GetInstance().operCheckDis =
						(player.passPlayers[0].transform.position - player.transform.position).magnitude;

					MatchDataManager.GetInstance().operCheckdir =
						(player.passPlayers[0].transform.position - player.transform.position).normalized;

					Vector3 edgeDown = MathTools.RotateByAxisAndAngle(player.transform.position,
						player.passPlayers[0].transform.position, player.transform.up, 45);
					Vector3 edgeUp = MathTools.RotateByAxisAndAngle(player.transform.position,
						player.passPlayers[0].transform.position, player.transform.up, -45);
					LineManager.Instance.DrawLine(player.transform.position, edgeDown, Color.red, 1);
					LineManager.Instance.DrawLine(player.transform.position, edgeUp, Color.black, 1);
					LineManager.Instance.DrawLine(player.transform.position, player.passPlayers[0].transform.position, Color.green, 1);

					
					player.transform.DOLookAt(player.passPlayers[0].transform.position, 1f).OnComplete(() => { RunOperUI(); });
				}

				return TaskStatus.Running;
			}

			StopOperUI();
			return TaskStatus.Success;
			
		}


		bool CouldInThisAction()
		{
			//1.关卡启动
			//2.进攻方一球员拥有球,  并且不会进入控球(只能选择传球和射门)
			//3.死球球碰到设置的好的碰撞体（球门，边线处设置的碰撞体）
			//4.手动操作结束

			if (isFinishManualOperation)
			{
				return false;
			}

			player = MatchDataManager.GetInstance().GetAttackTeamCouldShootOrPass();
			if (player != null)
			{
				return true;
			}
			return false;
		}

		///// <summary>
		///// 选择操作的球员
		///// </summary>
		//void SelectOperPlayer()
		//{

		//}

		///// <summary>
		///// 选择操作的类型
		///// 
		///// </summary>
		//void SelectOperType()
		//{
		//	//进攻方球员触球
		//	//射门/传球：
		//	//    距离球门的位置 太远只能传球
		//	//    防守球员的位置 射门正前方没有球员
		//}

		void RunOperUI()
		{
			FingerCtr.GetInstance().Run();
		}

		void StopOperUI()
		{
			FingerCtr.GetInstance().Stop();
		}


		void OnFinishManualOperation(Message msg)
		{
			isFinishManualOperation = true;
		}


	}

}

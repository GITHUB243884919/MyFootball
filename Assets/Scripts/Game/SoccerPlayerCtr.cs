using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Soccer
{
	public class SoccerPlayerCtr : MonoBehaviour
	{
		/// <summary>
		/// 队伍
		/// </summary>
		public TeamTag teamTag;

		/// <summary>
		/// 逻辑编号
		/// </summary>
		public int playerID;

		/// <summary>
		/// 球衣号码
		/// </summary>
		public int showNum;

		/// <summary>
		/// 射门最远距离
		/// </summary>
		public double maxShootDis;

		public double maxPassDis;

		public GameObject ball;

		Animation anim;

		/// <summary>
		/// 检查球的碰撞体
		/// </summary>
		CapsuleCollider colliderBall;

		/// <summary>
		/// 检查球的碰撞体
		/// </summary>
		CapsuleCollider colliderDetact;

		public PlayerActionType currActionType;

		public List<SoccerPlayerCtr> passPlayers;

		bool pause = false;
		
		void Start()
		{
			anim = gameObject.GetComponentInChildren<Animation>();
			colliderBall = gameObject.GetComponent<CapsuleCollider>();

			colliderDetact = transform.Find("Detact").GetComponent<CapsuleCollider>();
		}

		// Update is called once per frame
		void Update()
		{
			CurrAttackAction();
		}


		public void Action()
		{
			switch (currActionType)
			{
				case PlayerActionType.Shoot:
					Debug.LogError("Shoot");
					ShootAnim();
					BallCtr.GetInstance().Shoot();
					break;
				case PlayerActionType.Pass:
					PassAnim();
					BallCtr.GetInstance().Pass(this, passPlayers[0]);
					break;
			}
		}

		public void ShootAnim()
		{
			anim.Play("shoot");
			
		}

		public void PassAnim()
		{
			anim.Play("pass");
		}

		/// <summary>
		/// 是否拥有球
		/// </summary>
		/// <returns></returns>
		public bool IsOwnBall()
		{
			//球是否在碰撞体内

			//https://blog.csdn.net/qq_37724011/article/details/80244892
			int intLayer = LayerMask.NameToLayer("Ball");
			LayerMask lm = 1 << intLayer;
			var colliders = Physics.OverlapCapsule(transform.position - colliderBall.height * 0.5f * Vector3.up,
				transform.position + colliderBall.height * 0.5f * Vector3.up,
				colliderBall.radius, lm);

			if (colliders.Length <= 0)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// 有没有防守队员
		/// 临时这么写，应该是有个扇形范围
		/// </summary>
		/// <returns></returns>
		public bool HaveDefencePlayer()
		{
			int intLayer = LayerMask.NameToLayer("Team2");
			LayerMask lm = 1 << intLayer;
			var colliders = Physics.OverlapCapsule(transform.position - colliderDetact.height * 0.5f * Vector3.up,
				transform.position + colliderDetact.height * 0.5f * Vector3.up,
				colliderDetact.radius, lm);

			if (colliders.Length <= 0)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// 是否可以传球
		/// </summary>
		/// <returns></returns>
		public bool CouldPassBall()
		{
			if (!IsOwnBall())
			{
				return false;
			}

			//距离球门的距离大于球员的射程
			float disGoal = MatchDataManager.GetInstance().GetDisGoal(transform.position);
			if (disGoal > maxShootDis)
			{
				//射门距离太远，传球还是控球？
				//传球
				//	有位置更好的队友，且在传球距离
				//		位置更好的队友
				//			距离球门更近，其防守队员较远
				//
				var players = MatchDataManager.GetInstance().GetCouldPassPlayer(this);
				if (players.Count <= 0)
				{
					return false;
				}

				return true;
			}
			else
			{
				//在射门距离内，传球，控球，射门
				//有位置更好的队友
				//前面没有防守球员或者前面有防守球员但在防守范围之外

				var players = MatchDataManager.GetInstance().GetCouldPassPlayer(this);
				if (players.Count <= 0)
				{
					return false;
				}

			}


			return true;
		}

		public bool CouldShootBall()
		{
			if (!IsOwnBall())
			{
				return false;
			}

			//距离球门的距离小于球员的射程
			//前面没有防守球员或者前面有防守球员但在防守范围之外

			return true;
		}


		/// <summary>
		/// 是否可以控球
		/// </summary>
		/// <returns></returns>
		public bool CouldControllBall()
		{
			if (!IsOwnBall())
			{
				return false;
			}

			//距离球门的距离大于球员的射程

			return true;
		}


		/// <summary>
		/// 
		/// </summary>
		public void CurrAttackAction()
		{
			if (IsOwnBall())
			{
				CurrAttackWithBall();
				return;
			}

			CurrAttackWithoutBall();
		}

		/// <summary>
		/// 有球的情况
		/// </summary>
		public void CurrAttackWithBall()
		{
			//距离球门的距离大于球员的射程
			float disGoal = MatchDataManager.GetInstance().GetDisGoal(transform.position);
			if (disGoal > maxShootDis)
			{
				//射门距离太远，传球还是控球？
				//传球
				//	有位置更好的队友，且在传球距离
				//		位置更好的队友
				//			距离球门更近，其防守队员较远
				//
				var players = MatchDataManager.GetInstance().GetCouldPassPlayer(this);
				if (players.Count > 0)
				{
					currActionType = PlayerActionType.Pass;
					passPlayers = players;
					return;
				}

				currActionType = PlayerActionType.Controll;
			}
			else
			{
				//在射门距离内，传球，控球，射门
				//有位置更好的队友
				//前面没有防守球员或者前面有防守球员但在防守范围之外

				var players = MatchDataManager.GetInstance().GetCouldPassPlayer(this);
				if (players.Count > 0)
				{
					currActionType = PlayerActionType.Pass;
					return;
				}
				
				if (HaveDefencePlayer())
				{
					currActionType = PlayerActionType.Controll;
					return;
				}

				currActionType = PlayerActionType.Shoot;
			}
		}

		/// <summary>
		/// 没有的球的情况
		/// 
		/// </summary>
		public void CurrAttackWithoutBall()
		{
			//暂时只有idle
			currActionType = PlayerActionType.Idle;
		}

		public void Pause(bool pause)
		{
			this.pause = pause;
		}
	}

}

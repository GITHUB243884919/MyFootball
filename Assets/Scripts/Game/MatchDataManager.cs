using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1.根据配置确定第1脚的发起
/// 2.确定下一个接球的人是谁
/// 3.确定是传球还是射门
/// </summary>
namespace Game.Soccer
{
	public class MatchDataManager : UFrame.Common.SingletonMono<MatchDataManager>
	{
		public BallCtr ball;

		public GameObject AttackTeamNode;
		public GameObject DefenceTeamNode;
		public GameObject goalNode;

		public int maxSelect = 3;
		
		public bool isInit = false;
		List<SoccerPlayerCtr> playerAttackTeam = new List<SoccerPlayerCtr>();
		List<SoccerPlayerCtr> playerDefenceTeam = new List<SoccerPlayerCtr>();

		SoccerPlayerCtr firstPlayer;

		public SoccerPlayerCtr currPlayer;

		List<SoccerPlayerCtr> nearPlayers = new List<SoccerPlayerCtr>();

		Vector3 goalPos;

		public float operCheckDis = 0;
		public Vector3 operCheckdir = Vector3.zero;
		void Start()
		{
			InitTeam(AttackTeamNode, playerAttackTeam);
			InitTeam(DefenceTeamNode, playerDefenceTeam);
			goalPos = goalNode.transform.position;

			SetFirstPlayer();

			CameraController.GetInstance().target = firstPlayer.transform.Find("camera");

			isInit = true;
		}

		// Update is called once per frame
		void Update()
		{

		}

		void InitTeam(GameObject teamNode, List<SoccerPlayerCtr> playerTeam)
		{
			for (int i = 0; i < teamNode.transform.childCount; i++)
			{
				var go = teamNode.transform.GetChild(i);
				var ctr = go.GetComponent<SoccerPlayerCtr>();
				if (ctr != null)
				{
					playerTeam.Add(ctr);
				}
			}
		}

		void SetFirstPlayer()
		{
			int minPlayerId = int.MaxValue;
			for (int i = 0; i < playerAttackTeam.Count; i++)
			{
				var player = playerAttackTeam[i];
				if (player.playerID < minPlayerId)
				{
					minPlayerId = player.playerID;
					firstPlayer = player;
				}
			}

			currPlayer = firstPlayer;

			Debug.LogErrorFormat("SetFirstPlayer {0}", firstPlayer.name);
		}

		//void SetOper()
		//{
		//	var player = GetNearestGate();
		//	if (player == currPlayer)
		//	{
		//		//最近的就是自己射门
		//		player.Shoot();
		//		ball.Shoot();
		//	}
		//	else
		//	{
		//		//传球
		//		var near = GetNearPlayers();
		//		ball.Pass(currPlayer, near[0]);
		//		player.Pass();
		//	}

		//}

		/// <summary>
		/// 离球门最近的
		/// </summary>
		/// <returns></returns>
		public SoccerPlayerCtr GetNearestGate()
		{
			float minDis = float.MaxValue;
			SoccerPlayerCtr nearestPlayer = null;
			for (int i = 0; i < playerAttackTeam.Count; i++)
			{
				var player = playerAttackTeam[i];
				float dis = (goalPos - player.transform.position).sqrMagnitude;
				if (dis <= minDis)
				{
					nearestPlayer = player;
				}
			}

			return nearestPlayer;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<SoccerPlayerCtr> GetNearPlayers()
		{

			nearPlayers.Clear();

			for (int i = 0; i < playerAttackTeam.Count && i < maxSelect; i++)
			{
				var player = playerAttackTeam[i];
				if (player != currPlayer && nearPlayers.Count < maxSelect)
				{
					nearPlayers.Add(player);
				}
				else if (player != currPlayer && nearPlayers.Count == maxSelect)
				{
					for (int j = 0; j < nearPlayers.Count; j++)
					{
						float dis1 = (goalPos - player.transform.position).sqrMagnitude;
						float dis2 = (goalPos - nearPlayers[j].transform.position).sqrMagnitude;
						if (dis1 < dis2)
						{
							nearPlayers[j] = player;
						}
					}
				}
			}

			return nearPlayers;

		}

		public float GetSqrDisGoal(Vector3 p)
		{
			return (goalPos - p).sqrMagnitude;
		}

		public float GetDisGoal(Vector3 p)
		{
			return (goalPos - p).magnitude;
		}

		/// <summary>
		/// 获得比这个距离更近的球员
		/// </summary>
		/// <param name="disGoal"></param>
		/// <returns></returns>
		public List<SoccerPlayerCtr> GetNearerGoalPlayer(SoccerPlayerCtr player)
		{
			nearPlayers.Clear();
			float disSqr = (goalPos - player.transform.position).sqrMagnitude;
			for (int i = 0; i < playerAttackTeam.Count; i++)
			{
				if (playerAttackTeam[i] != player)
				{
					if ((goalPos - playerAttackTeam[i].transform.position).sqrMagnitude < disSqr)
					{
						nearPlayers.Add(playerAttackTeam[i]);
					}
				}
			}

			return nearPlayers;

		}

		/// <summary>
		/// 离球门更近的队友
		/// 传球距离合适
		/// 队友没有防守队员
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public List<SoccerPlayerCtr> GetCouldPassPlayer(SoccerPlayerCtr player)
		{
			var players = GetNearerGoalPlayer(player);
			if (players.Count <= 0)
			{
				return players;
			}
			double sqrPassDis = System.Math.Pow(player.maxPassDis, 2d);
			for (int i = players.Count - 1; i >=0; i--)
			{
				double sqrDis = (players[i].transform.position - player.transform.position).sqrMagnitude;
				//超过传球距离
				if (sqrDis > sqrPassDis)
				{
					players.Remove(players[i]);
					continue;
				}
				
				//有防守球员
				if (players[i].HaveDefencePlayer())
				{
					players.Remove(players[i]);
				}
			}


			return players;
		}

		/// <summary>
		/// 进攻队的能传球或者射门的队员
		/// </summary>
		/// <returns></returns>
		public SoccerPlayerCtr GetAttackTeamCouldShootOrPass()
		{
			for (int i = 0; i < playerAttackTeam.Count && i < maxSelect; i++)
			{
				var player = playerAttackTeam[i];
				if (player.currActionType == PlayerActionType.Shoot || player.currActionType == PlayerActionType.Pass)
				{
					currPlayer = player;
					return player;
				}
			}

			return null;
		}

		public SoccerPlayerCtr OwnerBall()
		{
			for (int i = 0; i < playerAttackTeam.Count; i++)
			{
				var player = playerAttackTeam[i];
				if (player.IsOwnBall())
				{
					return player;
				}
			}

			return null;
		}

		/// <summary>
		/// 是否是死球
		/// </summary>
		/// <returns></returns>
		public bool IsOutOfPlay()
		{
			return false;
		}
	}

}

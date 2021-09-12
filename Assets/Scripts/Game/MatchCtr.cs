//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 1.根据配置确定第1脚的发起
///// 2.确定下一个接球的人是谁
///// 3.确定是传球还是射门
///// </summary>
//namespace Game.Soccer
//{
//	public class MatchCtr : MonoBehaviour
//	{
//		public BallCtr ball;

//		public GameObject team1Node;
//		public GameObject team2Node;
//		public GameObject gateNode;

//		public int maxSelect = 3;

//		public bool isInit = false;
//		List<SoccerPlayerCtr> playerTeam1 = new List<SoccerPlayerCtr>();
//		List<SoccerPlayerCtr> playerTeam2 = new List<SoccerPlayerCtr>();

//		SoccerPlayerCtr firstPlayer;

//		SoccerPlayerCtr currPlayer;

//		List<SoccerPlayerCtr> nearPlayers = new List<SoccerPlayerCtr>();

//		Vector3 gatePos;
		

//		void Start()
//		{
//			InitTeam(team1Node, playerTeam1);
//			InitTeam(team2Node, playerTeam2);
//			gatePos = gateNode.transform.position;

//			SetFirstPlayer();

//			isInit = true;
//		}

//		// Update is called once per frame
//		void Update()
//		{

//		}

//		void InitTeam(GameObject teamNode, List<SoccerPlayerCtr> playerTeam)
//		{
//			for (int i = 0; i < teamNode.transform.childCount; i++)
//			{
//				var go = teamNode.transform.GetChild(i);
//				var ctr = go.GetComponent<SoccerPlayerCtr>();
//				if (ctr != null)
//				{
//					playerTeam.Add(ctr);
//				}
//			}
//		}

//		void SetFirstPlayer()
//		{
//			int minPlayerId = int.MaxValue;
//			for (int i = 0; i < playerTeam1.Count; i++)
//			{
//				var player = playerTeam1[i];
//				if (player.playerID < minPlayerId)
//				{
//					firstPlayer = player;
//				}
//			}

//			currPlayer = firstPlayer;
//		}

//		//void SetOper()
//		//{
//		//	var player = GetNearestGate();
//		//	if (player == currPlayer)
//		//	{
//		//		//最近的就是自己射门
//		//		player.Shoot();
//		//		ball.Shoot();
//		//	}
//		//	else
//		//	{
//		//		//传球
//		//		var near = GetNearPlayers();
//		//		ball.Pass(currPlayer, near[0]);
//		//		player.Pass();
//		//	}
			
//		//}

//		/// <summary>
//		/// 离球门最近的
//		/// </summary>
//		/// <returns></returns>
//		public SoccerPlayerCtr GetNearestGate()
//		{
//			float minDis = float.MaxValue;
//			SoccerPlayerCtr nearestPlayer = null;
//			for (int i = 0; i < playerTeam1.Count; i++)
//			{
//				var player = playerTeam1[i];
//				float dis = (gatePos - player.transform.position).sqrMagnitude;
//				if (dis <= minDis)
//				{
//					nearestPlayer = player;
//				}
//			}

//			return nearestPlayer;
//		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <returns></returns>
//		public List<SoccerPlayerCtr> GetNearPlayers()
//		{

//			nearPlayers.Clear();

//			for (int i = 0; i < playerTeam1.Count && i < maxSelect; i++)
//			{
//				var player = playerTeam1[i];
//				if (player != currPlayer && nearPlayers.Count < maxSelect)
//				{
//					nearPlayers.Add(player);
//				}
//				else if (player != currPlayer && nearPlayers.Count == maxSelect)
//				{
//					for (int j = 0; j < nearPlayers.Count; j++)
//					{
//						float dis1 = (gatePos - player.transform.position).sqrMagnitude;
//						float dis2 = (gatePos - nearPlayers[j].transform.position).sqrMagnitude;
//						if (dis1 < dis2)
//						{
//							nearPlayers[j] = player;
//						}
//					}
//				}
//			}

//			return nearPlayers;

//		}
//	}

//}

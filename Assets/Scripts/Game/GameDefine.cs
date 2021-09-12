using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Soccer
{

	/// <summary>
	/// 球队
	/// </summary>
	public enum TeamTag
	{
		/// <summary>
		/// 进攻方
		/// </summary>
		Attack = 1,

		/// <summary>
		/// 防守方
		/// </summary>
		Defence = 2,
	}

	/// <summary>
	/// 球员一级状态
	/// </summary>
	public enum PlayerActionType
	{
		/// <summary>
		/// 无求待机
		/// </summary>
		Idle = 1,

		/// <summary>
		/// 无球跑
		/// </summary>
		Run = 2,


		Pass = 3,
		Shoot = 4,

		/// <summary>
		/// 传球
		/// </summary>
		Controll = 5,
		Defence = 6,
	}



}

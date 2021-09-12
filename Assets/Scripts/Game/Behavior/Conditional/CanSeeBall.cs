using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Game.Soccer.Behavior
{
	/// <summary>
	/// 是否往前move
	/// 判定球离开我的距离
	/// 如果很远
	/// 判定我的站位和阵型
	/// 如果在阵型范围内
	/// 随机决定是否向前
	/// </summary>
	[TaskCategory("MySoccer")]
	public class CanSeeBall : Conditional
	{

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Failure;
		}
	}

}

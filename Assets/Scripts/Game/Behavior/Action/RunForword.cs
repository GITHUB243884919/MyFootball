using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Game.Soccer.Behavior
{
	/// <summary>
	/// ��ǰ��
	/// </summary>
	[TaskCategory("MySoccer")]
	public class RunForword : Action
	{

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Failure;
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Game.Soccer.Behavior
{
	/// <summary>
	/// ÊÇ·ñÄÜ´«Çò
	/// </summary>
	[TaskCategory("MySoccer")]
	public class CouldPass : Conditional
	{

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Failure;
		}
	}

}

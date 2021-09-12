using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Game.Soccer.Behavior
{
	/// <summary>
	/// ¿ØÇò
	/// </summary>
	[TaskCategory("MySoccer")]
	public class Controll : Action
	{

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Failure;
		}
	}

}

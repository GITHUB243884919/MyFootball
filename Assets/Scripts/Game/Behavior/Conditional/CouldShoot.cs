using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Game.Soccer.Behavior
{
	/// <summary>
	///  «∑Òƒ‹…‰√≈
	/// </summary>
	[TaskCategory("MySoccer")]
	public class CouldShoot : Conditional
	{

		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Failure;
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Game.Soccer.Behavior
{
	/// <summary>
	/// �Ƿ���ǰmove
	/// �ж����뿪�ҵľ���
	/// �����Զ
	/// �ж��ҵ�վλ������
	/// ��������ͷ�Χ��
	/// ��������Ƿ���ǰ
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Soccer
{
	public class MathTools
	{
		/// <summary>
		/// 绕点旋转
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="axis"></param>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static Vector3 RotateByAxisAndAngle(Vector3 start, Vector3 end, Vector3 axis, float angle)
		{
			//https://blog.csdn.net/wuming2016/article/details/120114275
			Vector3 dir = (end - start);
			Quaternion q = Quaternion.identity;

			q = Quaternion.AngleAxis(angle, axis);
			return q * dir + start;
		}


		/// <summary>
		/// 是否在扇形内
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="forward"></param>
		/// <param name="fieldOfViewAngle"></param>
		/// <param name="viewDistance"></param>
		/// <returns></returns>
		public static bool IsInSector(Vector3 start, Vector3 end, Vector3 forward, float fieldOfViewAngle, float viewDistance)
		{
			var dir = end - start;
			float angle = Vector3.Angle(dir, forward);
			return (dir.magnitude < viewDistance && angle < fieldOfViewAngle * 0.5f);
		}
	}

}

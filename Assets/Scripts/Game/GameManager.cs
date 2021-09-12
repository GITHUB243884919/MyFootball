using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFrame.Common;
using UFrame;

namespace Game.Soccer
{
	public class GameManager : SingletonMono<GameManager>
	{
		public void Update()
		{
			MessageManager.GetInstance().Tick();
		}
	}

}

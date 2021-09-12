using System.Collections;
using System.Collections.Generic;
using UFrame;
using UFrame.Common;
using UnityEngine;

namespace Game.Soccer
{
	public class FingerCtr : SingletonMono<FingerCtr>
	{
		public Material matLine;

		bool beginDraw;

		List<Vector3> posList = new List<Vector3>();

		Vector3 curPos = Vector3.zero;

		float interval = 0.01f;

		bool runing = false;

		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			//TraceFinger();
		}



		void OnGUI()
		{
			if (runing)
			{
				TraceFinger();
			}
			
		}

		void OnPostRender()
		{

			DrawLine();
		}


		void DrawLine()
		{
			if (!matLine)
			{
				Debug.LogError("Please Assign a material on the inspector");
				return;
			}

			if (!beginDraw)
				return;
			GL.PushMatrix();
			GL.LoadOrtho();

			matLine.SetPass(0);
			GL.Begin(GL.LINES);
			for (int i = 0; i < posList.Count - 1; i++)
			{
				Vector3 pos = posList[i];

				GL.Vertex3(pos.x, pos.y, pos.z);
				GL.Vertex3(posList[i + 1].x, posList[i + 1].y, posList[i + 1].z);
			}

			GL.End();
			GL.PopMatrix();
		}

		void TraceFinger()
		{
			Event e = Event.current;

			if (e != null)
			{
				if (e.type == EventType.MouseDown)
				{
					Vector3 dp = new Vector3(Input.mousePosition.x / Screen.width,
						Input.mousePosition.y / Screen.height, 0);

					//Vector3 wp = Camera.main.ScreenToWorldPoint(dp);
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  //摄像机需要设置MainCamera的Tag这里才能找到
					RaycastHit hitInfo;
					if (Physics.Raycast(ray, out hitInfo, float.MaxValue, LayerMask.GetMask("Gound")))
					{
						GameObject gameObj = hitInfo.collider.gameObject;
						Vector3 hitPoint = hitInfo.point;
						Debug.Log("click object name is " + gameObj.name + " , hit point " + hitPoint.ToString());
						var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
						go.transform.position = hitPoint;

						//检查手指在屏幕的点是否在球员前扇形范围内
						var player = MatchDataManager.GetInstance().currPlayer;
						beginDraw = MathTools.IsInSector(player.transform.position, hitPoint, 
							MatchDataManager.GetInstance().operCheckdir, 90f, 
							MatchDataManager.GetInstance().operCheckDis);

						//如果可以操作停止相机旋转操作
						if (beginDraw)
						{
							CameraController.GetInstance().isStop = true;
						}
					}

					//beginDraw = true;
				}
				if (e.type == EventType.MouseDrag)
				{
					if (!beginDraw)
					{
						return;
					}

					if (Vector3.Distance(curPos, Input.mousePosition) > interval)
					{
						curPos = Input.mousePosition;

						posList.Add(new Vector3(curPos.x / Screen.width, curPos.y / Screen.height, 0));
						//posList.Add(curPos);
					}

					//DrawLine();
				}
				if (e.type == EventType.MouseUp)
				{
					if (!beginDraw)
					{
						return;
					}

					beginDraw = false;
					ClearLines();
					MatchDataManager.GetInstance().currPlayer.Action();
					MessageManager.GetInstance().Send((int)GameMessageDefine.FinishManualOperation);
				}
			}
		}

		void ClearLines()
		{
			beginDraw = false;
			posList.Clear();
			curPos = Vector3.zero;
		}

		public void Run()
		{
			runing = true;
		}

		public void Stop()
		{
			runing = false;
			ClearLines();
		}
	}

}

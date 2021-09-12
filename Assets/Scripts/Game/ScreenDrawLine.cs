using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Soccer
{
	public class ScreenDrawLine : MonoBehaviour
	{
		public Material lineMaterial;

		bool beginDraw;

		List<Vector3> posList = new List<Vector3>();

		Vector3 curPos = Vector3.zero;

		float interval = 0.01f;

		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		void OnPostRender()
		{
			//if (!lineMaterial) {
			//	Debug.LogError("Please Assign a material on the inspector");
			//	return;
			//}
			//GL.PushMatrix(); //保存当前Matirx  
			//lineMaterial.SetPass(0); //刷新当前材质  
			//GL.LoadPixelMatrix();//设置pixelMatrix  
			//GL.Color(Color.yellow);
			//GL.Begin(GL.LINES);
			//GL.Vertex3(0, 0, 0);
			//GL.Vertex3(Screen.width, Screen.height, 0);
			//GL.End();
			//GL.PopMatrix();//读取之前的Matrix  
			DrawLine();
		}


		void DrawLine()
		{
			Debug.LogError("DrawLine");

			if (!beginDraw)
				return;
			GL.PushMatrix();
			GL.LoadOrtho();

			lineMaterial.SetPass(0);
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

		void OnGUI()
		{
			Event e = Event.current;

			if (e != null && e.type != null)
			{
				if (e.type == EventType.MouseDown)
				{
					beginDraw = true;
				}
				if (e.type == EventType.MouseDrag)
				{

					if (Vector3.Distance(curPos, Input.mousePosition) > interval)
					{
						curPos = Input.mousePosition;

						posList.Add(new Vector3(curPos.x / Screen.width, curPos.y / Screen.height, 0));
					}

					//DrawLine();
				}
				if (e.type == EventType.MouseUp)
				{
					beginDraw = false;
					ClearLines();
				}
			}
		}

		void ClearLines()
		{
			beginDraw = false;
			posList.Clear();
			curPos = Vector3.zero;
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMesh : MonoBehaviour
{
    // Start is called before the first frame update
	public MeshFilter mf;
	public MeshRenderer mr;
	public float size = 1f;
	void Start()
    {
		//MeshFilter mf = gameObject.AddComponent<MeshFilter>();
		//MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
		//float size = 1f;

		//顶点数组
		Vector3[] vertes = new Vector3[]
		{
			new Vector3(-size, -size, 0),//第一个点
			new Vector3(-size, size, 0), //第二个
			new Vector3(size, size, 0), //第三个
			new Vector3(size, -size, 0), //第四个
		};
		mf.mesh.vertices = vertes;

		//顶点组成的三角形
		mf.mesh.triangles = new[]
		{
			0, 1, 2,
			0, 2, 3
		};

		mf.mesh.uv = new Vector2[]
		{
			new Vector2(0, 0),//第一个点
			new Vector2(0, 1),//2
			new Vector2(1, 1),//3
			new Vector2(1, 0), //4
		};
	}

    // Update is called once per frame
    void Update()
    {

	}
}

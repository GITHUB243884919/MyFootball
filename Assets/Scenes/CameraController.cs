using UnityEngine;
using System;
using System.Collections;
using UFrame.Common;


public class CameraController : SingletonMono<CameraController>
{
	// Text m_debugTip;
	public bool canRotation_X = true;
	public bool canRotation_Y = true;
	public bool canScale = true;
	#region Field and Property
	/// <summary>
	/// Around center.
	/// </summary>
	public Transform target;

	/// <summary>
	/// Settings of mouse button, pointer and scrollwheel.
	/// </summary>
	public MouseSettings mouseSettings = new MouseSettings(0, 10, 10);

	/// <summary>
	/// Range limit of angle.
	/// </summary>
	public Range angleRange = new Range(-90, 90);

	/// <summary>
	/// Range limit of distance.
	/// </summary>
	public Range distanceRange = new Range(1, 10);

	/// <summary>
	/// Damper for move and rotate.
	/// </summary>
	[Range(0, 10)]
	public float damper = 5;

	/// <summary>
	/// Camera current angls.
	/// </summary>
	public Vector2 CurrentAngles { protected set; get; }

	/// <summary>
	/// Current distance from camera to target.
	/// </summary>
	public float CurrentDistance { protected set; get; }

	/// <summary>
	/// Camera target angls.
	/// </summary>
	protected Vector2 targetAngles;


	/// <summary>
	/// Target distance from camera to target.
	/// </summary>
	protected float targetDistance;
	#endregion

	#region Protected Method

	bool isInit = false;

	public bool isStop = false;

	protected virtual void Start()
	{
		//  m_debugTip = GameObject.Find("Text").GetComponent<Text>();
		//Vector3 => Vector2 ���Զ�ȥ����z
		CurrentAngles = targetAngles = transform.eulerAngles;
		Debug.LogErrorFormat("{0}, {1}", transform.eulerAngles, CurrentAngles);

		//CurrentDistance = targetDistance = Vector3.Distance(transform.position, target.position);

		RunCoroutine.Run(CoWaitTarget());
	}

	IEnumerator CoWaitTarget()
	{
		while (target == null)
		{
			yield return RunCoroutine.WaitForEndOfFrame;
		}

		CurrentDistance = targetDistance = Vector3.Distance(transform.position, target.position);

		isInit = true;
	}

	protected virtual void LateUpdate()
	{
		if (!isInit || isStop)
		{
			return;
		}

#if UNITY_EDITOR
		AroundByMouseInput();
#elif UNITY_ANDROID || UNITY_IPHONE
 
        AroundByMobileInput();
#endif

	}

	//��¼��һ���ֻ�����λ���ж��û�������Ŵ�����С����  
	private Vector2 oldPosition1;
	private Vector2 oldPosition2;

	private bool m_IsSingleFinger;
	/*
    private void ScaleCamera()
    {
        //�������ǰ���㴥�����λ��  
        var tempPosition1 = Input.GetTouch(0).position;
        var tempPosition2 = Input.GetTouch(1).position;
        float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
        float lastTouchDistance = Vector3.Distance(oldPosition1, oldPosition2);
        //�����ϴκ����˫ָ����֮��ľ�����  
        //Ȼ��ȥ����������ľ���  
        distance -= ( currentTouchDistance - lastTouchDistance ) * scaleFactor * Time.deltaTime;
        //�Ѿ�������ס��min��max֮��  
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        //������һ�δ������λ�ã����ڶԱ�  
        oldPosition1 = tempPosition1;
        oldPosition2 = tempPosition2;
    }
    */

	protected void AroundByMobileInput()
	{
		if (Input.touchCount == 1)
		{

			if (Input.touches[0].phase == TouchPhase.Moved)
			{
				targetAngles.y += Input.GetAxis("Mouse X") * mouseSettings.pointerSensitivity;
				targetAngles.x -= Input.GetAxis("Mouse Y") * mouseSettings.pointerSensitivity;

				//Range.
				targetAngles.x = Mathf.Clamp(targetAngles.x, angleRange.min, angleRange.max);
			}
			//Mouse pointer.
			m_IsSingleFinger = true;
		}

		//Mouse scrollwheel.
		if (canScale)
		{
			if (Input.touchCount > 1)
			{


				//�������ǰ���㴥�����λ��  
				if (m_IsSingleFinger)
				{
					oldPosition1 = Input.GetTouch(0).position;
					oldPosition2 = Input.GetTouch(1).position;
				}


				if (Input.touches[0].phase == TouchPhase.Moved && Input.touches[1].phase == TouchPhase.Moved)
				{
					var tempPosition1 = Input.GetTouch(0).position;
					var tempPosition2 = Input.GetTouch(1).position;


					float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
					float lastTouchDistance = Vector3.Distance(oldPosition1, oldPosition2);

					//�����ϴκ����˫ָ����֮��ľ�����  
					//Ȼ��ȥ����������ľ���  
					targetDistance -= (currentTouchDistance - lastTouchDistance) * Time.deltaTime * mouseSettings.wheelSensitivity;
					//  m_debugTip.text = ( currentTouchDistance - lastTouchDistance ).ToString() + " + " + targetDistance.ToString();


					//�Ѿ�������ס��min��max֮��  



					//������һ�δ������λ�ã����ڶԱ�  
					oldPosition1 = tempPosition1;
					oldPosition2 = tempPosition2;
					m_IsSingleFinger = false;
				}
			}

		}




		targetDistance = Mathf.Clamp(targetDistance, distanceRange.min, distanceRange.max);

		//Lerp.
		CurrentAngles = Vector2.Lerp(CurrentAngles, targetAngles, damper * Time.deltaTime);
		CurrentDistance = Mathf.Lerp(CurrentDistance, targetDistance, damper * Time.deltaTime);




		if (!canRotation_X) targetAngles.y = 0;
		if (!canRotation_Y) targetAngles.x = 0;


		//Update transform position and rotation.
		transform.rotation = Quaternion.Euler(CurrentAngles);

		transform.position = target.position - transform.forward * CurrentDistance;
		// transform.position = target.position - Vector3.forward * CurrentDistance;

	}

	/// <summary>
	/// Camera around target by mouse input.
	/// </summary>
	protected void AroundByMouseInput()
	{
		if (Input.GetMouseButton(mouseSettings.mouseButtonID))
		{
			//Mouse pointer.
			//���������Ļx�ᣬy�ᴥ��
			//���X�ᣨ���ң�����y��ת��y�ᣨ���£�����x��ת
			//http://blog.sina.com.cn/s/blog_bfa00a970102viu2.html
			targetAngles.y += Input.GetAxis("Mouse X") * mouseSettings.pointerSensitivity;
			targetAngles.x -= Input.GetAxis("Mouse Y") * mouseSettings.pointerSensitivity;

			//Range.
			targetAngles.x = Mathf.Clamp(targetAngles.x, angleRange.min, angleRange.max);
		}

		//Mouse scrollwheel.
		if (canScale)
		{
			targetDistance -= Input.GetAxis("Mouse ScrollWheel") * mouseSettings.wheelSensitivity;

		}
		// m_debugTip.text = Input.GetAxis("Mouse ScrollWheel").ToString() + " + " + targetDistance.ToString();


		targetDistance = Mathf.Clamp(targetDistance, distanceRange.min, distanceRange.max);

		//Lerp.
		CurrentAngles = Vector2.Lerp(CurrentAngles, targetAngles, damper * Time.deltaTime);
		CurrentDistance = Mathf.Lerp(CurrentDistance, targetDistance, damper * Time.deltaTime);




		if (!canRotation_X) targetAngles.y = 0;
		if (!canRotation_Y) targetAngles.x = 0;


		//Update transform position and rotation.
		transform.rotation = Quaternion.Euler(CurrentAngles);

		transform.position = target.position - transform.forward * CurrentDistance;
		// transform.position = target.position - Vector3.forward * CurrentDistance;



	}
	#endregion
}

[Serializable]
public struct MouseSettings
{
	/// <summary>
	/// ID of mouse button.
	/// </summary>
	public int mouseButtonID;

	/// <summary>
	/// Sensitivity of mouse pointer.
	/// </summary>
	public float pointerSensitivity;

	/// <summary>
	/// Sensitivity of mouse ScrollWheel.
	/// </summary>
	public float wheelSensitivity;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="mouseButtonID">ID of mouse button.</param>
	/// <param name="pointerSensitivity">Sensitivity of mouse pointer.</param>
	/// <param name="wheelSensitivity">Sensitivity of mouse ScrollWheel.</param>
	public MouseSettings(int mouseButtonID, float pointerSensitivity, float wheelSensitivity)
	{
		this.mouseButtonID = mouseButtonID;
		this.pointerSensitivity = pointerSensitivity;
		this.wheelSensitivity = wheelSensitivity;
	}
}

/// <summary>
/// Range form min to max.
/// </summary>
[Serializable]
public struct Range
{
	/// <summary>
	/// Min value of range.
	/// </summary>
	public float min;

	/// <summary>
	/// Max value of range.
	/// </summary>
	public float max;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="min">Min value of range.</param>
	/// <param name="max">Max value of range.</param>
	public Range(float min, float max)
	{
		this.min = min;
		this.max = max;
	}
}

/// <summary>
/// Rectangle area on plane.
/// </summary>
[Serializable]
public struct PlaneArea
{
	/// <summary>
	/// Center of area.
	/// </summary>
	public Transform center;

	/// <summary>
	/// Width of area.
	/// </summary>
	public float width;

	/// <summary>
	/// Length of area.
	/// </summary>
	public float length;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="center">Center of area.</param>
	/// <param name="width">Width of area.</param>
	/// <param name="length">Length of area.</param>
	public PlaneArea(Transform center, float width, float length)
	{
		this.center = center;
		this.width = width;
		this.length = length;
	}
}

/// <summary>
/// Target of camera align.
/// </summary>
[Serializable]
public struct AlignTarget
{
	/// <summary>
	/// Center of align target.
	/// </summary>
	public Transform center;

	/// <summary>
	/// Angles of align.
	/// </summary>
	public Vector2 angles;

	/// <summary>
	/// Distance from camera to target center.
	/// </summary>
	public float distance;

	/// <summary>
	/// Range limit of angle.
	/// </summary>
	public Range angleRange;

	/// <summary>
	/// Range limit of distance.
	/// </summary>
	public Range distanceRange;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="center">Center of align target.</param>
	/// <param name="angles">Angles of align.</param>
	/// <param name="distance">Distance from camera to target center.</param>
	/// <param name="angleRange">Range limit of angle.</param>
	/// <param name="distanceRange">Range limit of distance.</param>
	public AlignTarget(Transform center, Vector2 angles, float distance, Range angleRange, Range distanceRange)
	{
		this.center = center;
		this.angles = angles;
		this.distance = distance;
		this.angleRange = angleRange;
		this.distanceRange = distanceRange;
	}
}
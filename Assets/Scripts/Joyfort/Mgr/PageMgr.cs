/*******************************************************************
* FileName:     PageMgr.cs
* Author:       Fan Zheng Yong
* Date:         2019-10-14
* Description:  以车项目代码为基础, 增加通用按钮点击音效,增加通用点击缩放动效
* other:    
********************************************************************/


using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using UFrame.MiniGame;
using UFrame.Logger;

public class PageMgr{

	//all pages with the union type
	private static Dictionary<string, UIPage> m_allPages;
	public static Dictionary<string, UIPage> allPages
	{ 
		get { 
			return m_allPages; 
		} 
	}

	//control 1>2>3>4>5 each page close will back show the previus page.
	private static List<UIPage> m_currentPageNodes;
	public static List<UIPage> currentPageNodes
	{ 
		get { 
			return m_currentPageNodes; 
		} 
	}

    public static T GetPage<T>() where T : UIPage
    {
        foreach (var val in allPages.Values)
        {
            T t = val as T;
            if (t != null)
            {
                return t;
            }
        }
        return null;
    }

    public static UIPage GetPage(string pageName)
    {
        if (m_allPages != null && m_allPages.ContainsKey(pageName))
        {
            return m_allPages[pageName];
        }
        return null;
    }

    //delegate load ui function.
    public static Func<string, Object> delegateSyncLoadUI = null;
	public static Action<string, Action<Object>> delegateAsyncLoadUI = null;

    public static string btnSoundPath = "";

    public static void SetButtonSound(string path)
    {
        btnSoundPath = path;
    }

    public static void PlayButtonSound()
    {
        if (string.IsNullOrEmpty(btnSoundPath))
        {
            return;
        }

        SoundManager.GetInstance().PlaySound(btnSoundPath);
    }

	private static bool CheckIfNeedBack(UIPage page)
	{
		return page != null && page.CheckIfNeedBack();
	}

	/// <summary>
	/// make the target node to the top.
	/// </summary>
	public static void PopNode(UIPage page)
	{
		if (m_currentPageNodes == null)
		{
			m_currentPageNodes = new List<UIPage>();
		}

		if (page == null)
		{
            Debug.LogError("[UI] page popup is null.");
			return;
		}

		//sub pages should not need back.
		if (CheckIfNeedBack(page) == false)
		{
			return;
		}

		bool _isFound = false;
		for (int i = 0; i < m_currentPageNodes.Count; i++)
		{
			if (m_currentPageNodes[i].Equals(page))
			{
				m_currentPageNodes.RemoveAt(i);
				m_currentPageNodes.Add(page);
				_isFound = true;
				break;
			}
		}

		//if dont found in old nodes
		//should add in nodelist.
		if (!_isFound)
		{
			m_currentPageNodes.Add(page);
		}

		//after pop should hide the old node if need.
		//HideOldNodes();
	}

	private static void HideOldNodes()
	{
		if (m_currentPageNodes.Count < 0)
			return;
		UIPage topPage = m_currentPageNodes [m_currentPageNodes.Count - 1];
//		if (topPage.mode == UIMode.HideOther) {
//			//form bottm to top.
//			for (int i = m_currentPageNodes.Count - 2; i >= 0; i--) {
//				if (m_currentPageNodes [i].isActive ())
//					m_currentPageNodes [i].Hide ();
//			}
//		}
	}

	public static void ClearNodes()
	{
		m_currentPageNodes.Clear();
	}

	private static void ShowPage<T>(Action callback, object pageData, bool isAsync) where T : UIPage, new()
	{
		Type t = typeof(T);
		string pageName = t.ToString();

		if (m_allPages != null && m_allPages.ContainsKey(pageName))
		{
			ShowPage(pageName, m_allPages[pageName], callback, pageData, isAsync);
		}
		else
		{
			T instance = new T();
			ShowPage(pageName, instance, callback, pageData, isAsync);
		}
	}

	private static void ShowPage(string pageName, UIPage pageInstance, Action callback, object pageData, bool isAsync)
	{
		if (string.IsNullOrEmpty(pageName) || pageInstance == null)
		{
            Debug.LogError("[UI] show page error with :" + pageName + " maybe null instance.");
			return;
		}

		if (m_allPages == null)
		{
			m_allPages = new Dictionary<string, UIPage>();
		}

		UIPage page = null;
		if (m_allPages.ContainsKey(pageName))
		{
			page = m_allPages[pageName];
		}
		else
		{
			m_allPages.Add(pageName, pageInstance);
			page = pageInstance;
		}

		//if active before,wont active again.
		if (page.isActive () == false) {
			//before show should set this data if need. maybe.!!
			page.m_data = pageData;

			if (isAsync)
				page.Show (callback);
			else
				page.Show ();
		} else if (pageData != null) {
			page.m_data = pageData;
			page.Refresh ();
		}

        //RefreshUIEffectOrder(pageName,true);

    }

    /// <summary>
    /// 初始化某个UI的awake，
    /// 目的是为了打开UI时候减少awake的时间，提高速度
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void AwakePage<T>() where T : UIPage, new()
    {
        ShowPage<T>();  //初始化
        ClosePage<T>(); //隐藏
    }
    public static void AwakePage<T>(object pageData) where T : UIPage, new()
    {
        ShowPage<T>(pageData);  //初始化
        ClosePage<T>(); //隐藏
    }  

    /// <summary>
    /// Sync Show Page
    /// </summary>
    public static void ShowPage<T>() where T : UIPage, new()
	{
		ShowPage<T>(null, null, false);
	}

	/// <summary>
	/// Sync Show Page With Page Data Input.
	/// </summary>
	public static void ShowPage<T>(object pageData) where T : UIPage, new()
	{
		ShowPage<T>(null, pageData, false);
	}

	public static void ShowPage(string pageName, UIPage pageInstance)
	{
		ShowPage(pageName, pageInstance, null, null, false);
	}

	public static void ShowPage(string pageName, UIPage pageInstance, object pageData)
	{
		ShowPage(pageName, pageInstance, null, pageData, false);
	}

	/// <summary>
	/// Async Show Page with Async loader bind in 'TTUIBind.Bind()'
	/// </summary>
	public static void ShowPage<T>(Action callback) where T : UIPage, new()
	{
		ShowPage<T>(callback, null, true);
	}

	public static void ShowPage<T>(Action callback, object pageData) where T : UIPage, new()
	{
		ShowPage<T>(callback, pageData, true);
	}

	/// <summary>
	/// Async Show Page with Async loader bind in 'TTUIBind.Bind()'
	/// </summary>
	public static void ShowPage(string pageName, UIPage pageInstance, Action callback)
	{
		ShowPage(pageName, pageInstance, callback, null, true);
	}

	public static void ShowPage(string pageName, UIPage pageInstance, Action callback, object pageData)
	{
		ShowPage(pageName, pageInstance, callback, pageData, true);
	}

	/// <summary>
	/// close current page in the "top" node.
	/// </summary>
	public static void ClosePage()
	{
		//Debug.Log("Back&Close PageNodes Count:" + m_currentPageNodes.Count);

		if (m_currentPageNodes == null || m_currentPageNodes.Count <= 1) return;

		UIPage closePage = m_currentPageNodes[m_currentPageNodes.Count - 1];
		m_currentPageNodes.RemoveAt(m_currentPageNodes.Count - 1);

		//show older page.
		//TODO:Sub pages.belong to root node.
		if (m_currentPageNodes.Count > 0)
		{
			UIPage page = m_currentPageNodes[m_currentPageNodes.Count - 1];
			if (page.isAsyncUI)
				ShowPage(page.name, page, () =>
					{
						closePage.Hide();
					});
			else
			{
				ShowPage(page.name, page);
				//after show to hide().
				closePage.Hide();
			}
		}
	}

    /// <summary>
	/// Close else target page
	/// </summary>
    /// <param name="pageName"></param>
    public static void CloseElsePage(string pageName)
    {
        var allPage = PageMgr.allPages;
        foreach (var item in allPage)
        {
            if (item.Value.name != pageName && item.Value.name != "UINewIdleUIMainPage")
            {
                PageMgr.ClosePage(item.Value);
            }
        }
    }


	/// <summary>
	/// Close target page
	/// </summary>
	public static void ClosePage(UIPage target)
	{
        //UIInteractive.GetInstance().iPage = null;
        if (target == null) return;
		if (target.isActive() == false)
		{
			if (m_currentPageNodes != null)
			{
				for (int i = 0; i < m_currentPageNodes.Count; i++)
				{
					if (m_currentPageNodes[i] == target)
					{
						m_currentPageNodes.RemoveAt(i);
						break;
					}
				}
				return;
			}
		}

		if (m_currentPageNodes != null && m_currentPageNodes.Count >= 1 && m_currentPageNodes[m_currentPageNodes.Count - 1] == target)
		{
			m_currentPageNodes.RemoveAt(m_currentPageNodes.Count - 1);

			//show older page.
			//TODO:Sub pages.belong to root node.
			if (m_currentPageNodes.Count > 0)
			{
				UIPage page = m_currentPageNodes[m_currentPageNodes.Count - 1];
				if (page.isAsyncUI)
					ShowPage(page.name, page, () =>
						{
							target.Hide();
						});
				else
				{
					ShowPage(page.name, page);
					target.Hide();
				}

				return;
			}
		}
		else if (target.CheckIfNeedBack())
		{
			for (int i = 0; i < m_currentPageNodes.Count; i++)
			{
				if (m_currentPageNodes[i] == target)
				{
					m_currentPageNodes.RemoveAt(i);
					target.Hide();
					break;
				}
			}
		}

		target.Hide();
    }

	public static void ClosePage<T>() where T : UIPage
	{
		Type t = typeof(T);
		string pageName = t.ToString();

		if (m_allPages != null && m_allPages.ContainsKey(pageName))
		{
			ClosePage(m_allPages[pageName]);
		}
		else
		{
			//Debug.LogError(pageName + "havnt show yet!");
		}
        //RefreshUIEffectOrder(pageName,false);

    }

	public static void ClosePage(string pageName)
	{
		if (m_allPages != null && m_allPages.ContainsKey(pageName))
		{
			ClosePage(m_allPages[pageName]);
		}
		else
		{
			//Debug.LogError(pageName + " havnt show yet!");
		}
	}

    //public static void CloseOtherAllPage(string pageName)
    //{
    //    if (m_allPages != null && m_allPages.ContainsKey(pageName))
    //    {




    //        ClosePage(m_allPages[pageName]);
    //    }
    //    else
    //    {
    //        //Debug.LogError(pageName + " havnt show yet!");
    //    }
    //}


    public static List<UIPage> activePages = new List<UIPage>();
    public static List<UIPage> GetActivePages()
    {
        activePages.Clear();
        foreach(var val in allPages.Values)
        {
            if (val.isActive())
            {
                activePages.Add(val);
            }
        }

        return activePages;
    }

    public static void CloseAllPage(bool isAll, string exceptPage)
    {
        var allPages = PageMgr.allPages;
        foreach (var item in allPages)
        {
            if (!isAll)
            {
                if (item.Value.name != exceptPage)
                {
                    PageMgr.ClosePage(item.Value);
                }
            }
            else
            {
                PageMgr.ClosePage(item.Value);
            }

        }
    }

    /// <summary>
    /// 刷新UI粒子特效
    /// </summary>
    /// <param name="pageName"></param>
    /// <param name="isOpen"></param>
    public static void RefreshUIEffectOrder(string pageName,bool isOpen)
    {
        if (pageName == "UIMoneyEffect")
        {
            return;
        }
        UIPage page = PageMgr.GetPage("UIMain_Bath");
        if (page == null)
        {
            return;
        }
        var activePages = PageMgr.GetActivePages();
        for (int i = 0; i < activePages.Count; i++)
        {

            if (isOpen)
            {
                if (pageName == string.Empty && activePages[i].name == "UIMain_Bath")
                {
                    activePages[i].RefreshParticalSystemsOrder(pageName == "UIMain_Bath");
                }
                else if(pageName != string.Empty)
                {
                    activePages[i].RefreshParticalSystemsOrder(pageName == activePages[i].name);
                }
            }
            else
            {
                //隐藏
                if (PageMgr.OnlyHaveOneActivePage("UIMain_Bath"))
                {
                    activePages[i].RefreshParticalSystemsOrder(pageName != "UIMain_Bath");
                }
                else
                {
                    activePages[i].RefreshParticalSystemsOrder(activePages[i].name!= "UIMain_Bath");
                }
            }
        }
        //if (isOpen)
        //{
        //    page.RefreshParticalSystemsOrder(pageName == "UIMain_Bath");
        //}
        //else
        //{
        //    //隐藏
        //    if (PageMgr.OnlyHaveOneActivePage("UIMain_Bath"))
        //    {
        //        page.RefreshParticalSystemsOrder(pageName != "UIMain_Bath");
        //    }
        //}
    }

    /// <summary>
    /// 只有一个激活的UI
    /// </summary>
    /// <param name="pageName"></param>
    /// <returns></returns>
    public static bool OnlyHaveOneActivePage(string pageName)
    {
        var activePages = PageMgr.GetActivePages();
        if (activePages.Count == 1 && activePages[0].name == pageName)
        {
            return true;
        }
        else if (activePages.Count == 2)
        {
            //UIMoneyEffect
            for (int i = 0; i < activePages.Count; i++)
            {
                if (activePages[i].name != "UIMoneyEffect" && activePages[i].name != pageName)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}

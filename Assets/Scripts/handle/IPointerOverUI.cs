using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// 判断手指触摸
/// </summary>
public class IPointerOverUI
{
    public delegate void CancelEvent(GameObject go);
    public CancelEvent cancelEvent;
    /// <summary>
    /// 忽略所有事件
    /// </summary>
    bool _ignoreEvent;
    public bool ignoreEvent
    {
        get { return _ignoreEvent; }
        set { this._ignoreEvent = value; }
    }
    private static IPointerOverUI instance = new IPointerOverUI();
    public static IPointerOverUI Instance
    {
        get
        {
            return instance;
        }
    }
    /// <summary>
    /// 当前是否点击在UI上
    /// </summary>
    bool _isOnUI = false;
    public bool isOnUI
    {
        get { return _isOnUI; }
        set { _isOnUI = value; }
    }
    /// <summary>
    /// 当前选中的对象
    /// </summary>
    RaycastResult _currentSelected;
    public RaycastResult currentSelected
    {
        get { return _currentSelected; }
        set { this._currentSelected = value; }
    }
    /// <summary>
    /// 当前结果
    /// </summary>
    List<RaycastResult> results;
    /// <summary>
    /// 当前屏幕坐标点是否在UI上
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    public bool IsPointerOverUIObject(Canvas canvas, Vector2 screenPosition)
    {
        if (ignoreEvent) return false;
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = screenPosition;
        GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    /// <summary>
    /// 返回点击到的UI物体
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool IsPointerOverUIObject(out RaycastResult result, bool setSelected = false)
    {
        bool ignore = setCurrentPointerObjects();
        if (ignore)
        {
            result = new RaycastResult();
            return false;
        }
        if (results.Count > 0)
        {
            result = results[0];
            isOnUI = true;
        }
        else
        {
            result = new RaycastResult();
            isOnUI = false;
        }
        if (setSelected)
            currentSelected = result;
        return results.Count > 0;
    }
    /// <summary>
    /// 是否点击在UI上
    /// </summary>
    /// <returns></returns>
    public bool IsPointerOverUIObject(bool setSelected = false)
    {
        bool ignore = setCurrentPointerObjects();
        if (ignore)
        {
            return false;
        }
        if (results.Count > 0)
        {
            isOnUI = true;
            if (setSelected)
                currentSelected = results[0];
        }
        else
            isOnUI = false;
        return isOnUI;
    }
    /// <summary>
    /// 是否同一个物体
    /// </summary>
    /// <returns></returns>
    public bool IsOneObject()
    {
        bool ignore = setCurrentPointerObjects();
        if (ignore)
        {
            return false;
        }
        RaycastResult r;
        if (results.Count > 0)
            r = results[0];
        else
            r = new RaycastResult();
        if (cancelEvent != null)
            cancelEvent(r.gameObject);
        return currentSelected.gameObject == r.gameObject;
    }

    public bool setCurrentPointerObjects()
    {
        if (ignoreEvent) return true;
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return false;
    }

}

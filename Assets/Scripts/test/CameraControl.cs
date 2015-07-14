using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using th.nx;

public class CameraControl : MonoBehaviour
{
    /// <summary>
    /// 最大值
    /// </summary>
    public Vector3 maxValue;
    /// <summary>
    /// 最小值
    /// </summary>
    public Vector3 minValue;
    /// <summary>
    /// 第一点
    /// </summary>
    public Vector3 first;
    /// <summary>
    /// 第二点
    /// </summary>
    public Vector3 second;
    /// <summary>
    /// 滑动速度
    /// </summary>
    public float speed = 1f;
    /// <summary>
    /// 距离间隔值
    /// </summary>
    float mThreshold = 0f;
    /// <summary>
    /// z轴的偏移量
    /// </summary>
    [HideInInspector]
    public float offsetZ;
    /// <summary>
    /// 缩放时触摸的第一点(屏幕坐标)
    /// </summary>
    Vector3 firstScalePosition;
    /// <summary>
    /// 缩放时触摸的第二点(屏幕坐标)
    /// </summary>
    Vector3 secondScalePosition;
    /// <summary>
    /// 记录上一次缩放的距离
    /// </summary>
    float lastDistance = 0f;
    /// <summary>
    /// 摄像机距离最小值
    /// </summary>
    public float minScale;
    /// <summary>
    /// 摄像机距离最大值
    /// </summary>
    public float maxScale;
    /// <summary>
    /// 是否开始移动摄像机（外部调用）
    /// </summary>
    bool startMoveCameraStatus = false;
    /// <summary>
    /// 移动摄像机到指定位置（外部调用）
    /// </summary>
    Vector3 startMoveCameraDestination;
    public void setStartMoveCamera(bool start, Vector3 dest)
    {
        this.startMoveCameraStatus = start;
        this.startMoveCameraDestination = new Vector3(dest.x, transform.position.y, dest.z+transform.position.z/2);
        this.startMoveCameraDestination = new Vector3(Mathf.Clamp(startMoveCameraDestination.x, minValue.x, maxValue.x),
            Mathf.Clamp(startMoveCameraDestination.y, minValue.y, maxValue.y),
            Mathf.Clamp(startMoveCameraDestination.z, minValue.z, maxValue.z));
    }
    // Use this for initialization
    void Start()
    {
        offsetZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        startMoveCamera();
        srollScale();
        if (Input.GetMouseButtonDown(0))
        {
            if (IPointerOverUI.Instance.IsPointerOverUIObject(true))
            {
                return;
            }
            Vector3 v = Input.mousePosition;
            v.z = offsetZ;
            first = Camera.main.ScreenToWorldPoint(v);
        }
        if (Input.GetMouseButton(0))
        {
            if (IPointerOverUI.Instance.currentSelected.gameObject!=null)
            {
                return;
            }
            Vector3 v = Input.mousePosition;
            v.z = offsetZ;
            second = Camera.main.ScreenToWorldPoint(v);
            move();
            first = second;
        }
        if (Input.touchCount == 2)
        {
            if (IPointerOverUI.Instance.currentSelected.gameObject != null)
            {
                return;
            }
            scale();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (IPointerOverUI.Instance.IsPointerOverUIObject())
            {
                return;
            }
            startMoveCameraStatus = false;
            lastDistance = 0;
        }
    }

    float SpringLerp(float strength, float deltaTime)
    {
        if (deltaTime > 1f) deltaTime = 1f;
        int ms = Mathf.RoundToInt(deltaTime * 1000f);
        deltaTime = 0.001f * strength;
        float cumulative = 0f;
        for (int i = 0; i < ms; ++i) cumulative = Mathf.Lerp(cumulative, 1f, deltaTime);
        return cumulative;
    }
    /// <summary>
    /// 移动
    /// </summary>
    void move()
    {
        float offsetx = second.x - first.x;
        float offsety = second.y - first.y;
        Vector3 offset = new Vector3(offsetx * speed, 0, offsety * speed);
        Vector3 target = transform.position + offset;
        mThreshold = offset.magnitude * 0.01f;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime*10);
        if (mThreshold >= (target - transform.position).magnitude)
        {
            transform.position = target;
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minValue.x, maxValue.x), Mathf.Clamp(transform.position.y, minValue.y, maxValue.y), Mathf.Clamp(transform.position.z, minValue.z, maxValue.z));
    }
    /// <summary>
    /// 缩放
    /// </summary>
    void scale()
    {
        Touch[] touches = Input.touches;
        if (touches.Length < 2) return;
        firstScalePosition = touches[0].position;
        secondScalePosition = touches[1].position;
        float distance = Vector3.Distance(firstScalePosition, secondScalePosition);
        if (lastDistance == 0f)
            lastDistance = distance;
        float field = Camera.main.fieldOfView;
        field = field - (distance - lastDistance) * 0.1f;
        field = Mathf.Clamp(field, minScale, maxScale);
        Camera.main.fieldOfView = field;
        lastDistance = distance;
    }
    /// <summary>
    /// 滚轮缩放
    /// </summary>
    void srollScale()
    {
        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta != 0)
        {
            float field = Camera.main.fieldOfView;
            field = field + scrollDelta;
            field = Mathf.Clamp(field, minScale, maxScale);
            Camera.main.fieldOfView = field;
        }
    }
    /// <summary>
    /// 外部调用，移动摄像机
    /// </summary>
    /// <param name="destination"></param>
    public void startMoveCamera()
    {
        if (startMoveCameraStatus)
        {
            if (transform.position == startMoveCameraDestination)
            {
                startMoveCameraStatus = false;
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, startMoveCameraDestination, Time.deltaTime * 150);
        }
    }
}

using UnityEngine;
using System.Collections;
using th.nx;

public class ShowFPS : MonoBehaviour {

    public float f_UpdateInterval = 1F;

    private float f_LastInterval;

    private int i_Frames = 0;

    private float f_Fps;

    GUIStyle bb;
    void OnBecameVisible()
    {
        Debug.Log("OnBecameVisible ");
    }
    void OnEnable()
    {
        Debug.Log("OnEnable");
    }
    void Awake()
    {
        Debug.Log("Awake");
    }
    void Start() 
    {
        Debug.Log("Start");
        Application.targetFrameRate=60;

        f_LastInterval = Time.realtimeSinceStartup;

        i_Frames = 0;

        bb = new GUIStyle();
        bb.normal.textColor = Color.white;
        bb.fontSize = 40;
    }

    void OnGUI() 
    {
        GUI.Label(new Rect(0, 0, 200, 200), "FPS:" + f_Fps.ToString("f2"),bb);
    }

    void Update() 
    {
        ++i_Frames;

        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval) 
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
        }
    }
    void FixedUpdate()
    {

    }
    void LateUpdate()
    {

    }
}

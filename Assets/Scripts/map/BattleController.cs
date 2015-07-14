using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using com.tianhe.map.sprite;
using UnityEngine.SocialPlatforms;
using System;
using th.nx;
/// <summary>
/// 战斗控制器
/// </summary>
public class BattleController : MonoBehaviour
{
    public Sprite[] buttonSprites;
    public Button[] buttons;
    Map map;
    CameraControl cameraControl;
    /// <summary>
    /// 记录长按时间戳
    /// </summary>
    float lastPointTime;
    /// <summary>
    /// 是否开始长按操作
    /// </summary>
    bool startFocusCamera;
    /// <summary>
    /// 是否触发了长按操作，如果是，忽略鼠标抬起操作
    /// </summary>
    bool isFocusCamera;
    // Use this for initialization
    void Start()
    {
        map = GameObject.FindObjectOfType<Map>();
        cameraControl = GameObject.FindObjectOfType<CameraControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastResult result;
            if (IPointerOverUI.Instance.IsPointerOverUIObject(out result, true))
            {
                if (result.gameObject == null)
                {
                    return;
                }
                IPointerOverUI.Instance.ignoreEvent = false;
                startFocusCamera = true;
                isFocusCamera = false;
                lastPointTime = Time.time;
                Button b = result.gameObject.GetComponent<Button>();
                if (b != null)
                {
                    playScaleAnimation(b, "Pressed");
                }
            }
        }
        if (!isFocusCamera && Input.GetMouseButtonUp(0))
        {
            if (IPointerOverUI.Instance.ignoreEvent)
            {
                IPointerOverUI.Instance.ignoreEvent = false;
                return;
            }
            RaycastResult result = IPointerOverUI.Instance.currentSelected;
            if (result.gameObject != null)
            {
                IPointerOverUI.Instance.ignoreEvent = false;
                Button b = result.gameObject.GetComponent<Button>();
                if (b != null)
                {
                    playScaleAnimation(b, "Normal");
                    setSelected(b);
                }
            }
        }
        if (startFocusCamera && Input.GetMouseButton(0))
        {
            RaycastResult result;
            if (IPointerOverUI.Instance.IsPointerOverUIObject(out result))
            {
                float curr = Time.time;
                if (curr - lastPointTime > 0.7)
                {
                    if (startFocusCamera)
                    {
                        startFocusCamera = false;
                        isFocusCamera = true;
                        moveCamera(result.gameObject.name);
                        Button b = result.gameObject.GetComponent<Button>();
                        if (b != null)
                        {
                            allDeselected(b);
                            playScaleAnimation(b, "Normal");
                            setSelectedEnable(b);
                        }
                    }
                }
            }
            else
            {
                if (IPointerOverUI.Instance.ignoreEvent)
                {
                    return;
                }
                IPointerOverUI.Instance.ignoreEvent = true;
                if (IPointerOverUI.Instance.currentSelected.gameObject != null)
                {
                    Button b = IPointerOverUI.Instance.currentSelected.gameObject.GetComponent<Button>();
                    if (b != null)
                    {
                        playScaleAnimation(b, "Normal");
                    }
                }
            }
        }
    }

    public void moveCamera(string name)
    {
        SpriteSoldier temp = null;
        if (name.Equals("All"))
        {
            temp = map.getFocusSoldierOfAll();
        }
        else
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (name.Equals(buttons[i].name))
                {
                    temp = map.getFocusSoldierByType(9 + i);
                }
            }
        }
        if (temp != null)
        {
            cameraControl.setStartMoveCamera(true, temp.transform.position);
        }
    }

    public void setSelected(Button button)
    {
        allDeselected(button);
        if (button.name == "All")
        {
            if (button.image.sprite == buttonSprites[12])
            {
                playerButtonSound(button);
                button.image.sprite = buttonSprites[13];
                map.setAllSoldiersType(true);
            }
            else
            {
                button.image.sprite = buttonSprites[12];
                map.setAllSoldiersType(false);
            }
        }
        else
            for (int i = 0; i < buttons.Length; i++)
            {
                if (button == buttons[i])
                {
                    if (button.image.sprite == buttonSprites[i * 2])
                    {
                        playerButtonSound(button);
                        button.image.sprite = buttonSprites[i * 2 + 1];
                        map.setSoldiersByType(9 + i, true);
                    }
                    else
                    {
                        button.image.sprite = buttonSprites[i * 2];
                        map.setSoldiersByType(9 + i, false);
                    }
                }
            }
    }
    public void playScaleAnimation(Button button,string type)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (button == buttons[i])
            {
                if (button.image.sprite == buttonSprites[i * 2])
                {
                    Animator anim = button.GetComponent<Animator>();
                    anim.SetBool(type, true);
                }
            }
        }
    }
    /// <summary>
    /// 强制选中
    /// </summary>
    /// <param name="button"></param>
    public void setSelectedEnable(Button button)
    {
        if (button.name == "All")
        {
            playerButtonSound(button);
            button.image.sprite = buttonSprites[13];
            map.setAllSoldiersType(true);
        }
        else
            for (int i = 0; i < buttons.Length; i++)
            {
                if (button == buttons[i])
                {
                    playerButtonSound(button);
                    button.image.sprite = buttonSprites[i * 2 + 1];
                    map.setSoldiersByType(9 + i, true);
                }
            }
    }
    private void allDeselected(Button button)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == button) continue;
            buttons[i].image.sprite = buttonSprites[i * 2];
        }
    }

    private void playerButtonSound(Button button)
    {
        int random = UnityEngine.Random.Range(0, 2);
        string name = button.name;
        switch (random)
        {
            case 0:
                name += "_Yes_00";
                break;
            case 1:
                name += "_Yes_01";
                break;
        }
        AudioManagerMiniBattle.audioManager.playSound(button.gameObject, name);
    }
}

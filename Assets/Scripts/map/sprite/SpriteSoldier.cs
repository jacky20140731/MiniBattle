using UnityEngine;
using System.Collections;
using com.tianhe.handle;
using System.Collections.Generic;
using th.nx;
using com.tianhe.map.logic;
using System;
namespace com.tianhe.map.sprite
{
    /// <summary>
    /// 单兵显示
    /// </summary>
    public class SpriteSoldier : MonoBehaviour
    {
        /// <summary>
        /// 士兵数据
        /// </summary>
        public Soldier soldier;
        /// <summary>
        /// map
        /// </summary>
        Map map;
        /// <summary>
        /// 开始行走
        /// </summary>
        [HideInInspector]
        private bool _isMoving = false;
        /// <summary>
        /// 开始旋转
        /// </summary>
        [HideInInspector]
        private bool canRotate = false;
        /// <summary>
        /// 旋转速度
        /// </summary>
        public float rotateSpeed = 30f;
        /// <summary>
        /// 目标点
        /// </summary>
        private Vector3 _nextPos = Vector3.zero;
        private Vector3 _direct;
        /// <summary>
        /// 动画控制器
        /// </summary>
        Animator animator;
        /// <summary>
        /// 选中框
        /// </summary>
        Transform selected;

        private AStarNode _nextNode;

        int runningHash;
        private AsyncDispatchBehaviour _dispatcherMono;
        // Use this for initialization
        void Start()
        {
            //Log.debug("ENTER SpriteSoldier.Start");

            _dispatcherMono = GameObject.FindObjectOfType<AsyncDispatchBehaviour>();
            map = transform.parent.parent.GetComponent<Map>();
            animator = transform.FindChild("Player").GetComponent<Animator>();
            runningHash = Animator.StringToHash("running");
            selected = transform.FindChild("Quad");
            setSelected(false);

            //Log.debug("LEAVE SpriteSoldier.Start");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Log.debug("ENTER SpriteSoldier.FixedUpdate");

            if (_isMoving)
            {
                float delta = _nextNode.speed /* * Time.deltaTime*/;  // 为了能让服务器演算，每一帧移动的距离必须是一个固定值
                Vector3 pos;
                if (soldier.paths.Count > 0)
                {
                    pos = Vector3.MoveTowards(transform.position, _direct, delta);

                    Vector3 pos1 = Vector3.MoveTowards(pos, _direct, delta);
                    float d1 = Vector3.Distance(_nextPos, pos);
                    float d2 = Vector3.Distance(pos1, _nextPos);
                    if (d1 < d2)
                        moveToNextGrid();
                }
                else
                {
                    pos = Vector3.MoveTowards(transform.position, _nextPos, delta);
                    if (pos == _nextPos)
                        moveToNextGrid();
                }

                transform.position = pos;
            }

            if (canRotate)
                rotate();

            //Log.debug("LEAVE SpriteSoldier.FixedUpdate");
        }

        public void move()
        {
            if (soldier.paths != null && soldier.paths.Count > 0)
            {
                soldier.paths.RemoveAt(0);

                _isMoving = true;
                canRotate = true;
                animator.SetBool(runningHash, true);
                moveToNextGrid();
                playAttackSound();
            }
        }
        public void stop()
        {
            _isMoving = false;
            canRotate = false;
            //animator.SetBool(runningHash, false);
        }
        void moveToNextGrid()
        {
            //if (soldier.index < soldier.paths.Count)
            if (soldier.paths.Count > 0)
            {
                _nextNode = soldier.paths[0];
                soldier.paths.RemoveAt(0);

                soldier.x = _nextNode.pos.x;
                soldier.y = _nextNode.pos.y;

                if (soldier.paths.Count > 0)
                {
                    _nextPos.x = _nextNode.pos.x * map.nodeW + map.nodeW / 2.0F;
                    _nextPos.z = _nextNode.pos.y * map.nodeH + map.nodeH / 2.0F;
                    //transform.FindChild("Player").LookAt(_nextGridPos);

                    _direct = _nextPos - transform.position;
                    _direct = _direct * 10000.0F + _nextPos;
                }
                else
                {
                    _nextPos.x = soldier._target.x;
                    _nextPos.z = soldier._target.y;
                }
            }
            else
            {
                _nextNode = null;

                //Debug.Log("soldier.paths.Count == 0");
                canRotate = false;
                _isMoving = false;
                animator.SetBool(runningHash, false);
                soldier.paths = null;
                _nextPos = Vector3.zero;
                playAttackSound();
            }
        }
        void rotate()
        {
            Vector3 targetDir = _nextPos - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.FindChild("Player").forward, targetDir, Time.deltaTime * rotateSpeed, 0.0f);
            transform.FindChild("Player").rotation = Quaternion.LookRotation(newDir);
        }
        /// <summary>
        /// 设置选中
        /// </summary>
        public void setSelected(bool show)
        {
            soldier.isSelected = show;
            selected.gameObject.SetActive(show);
        }
        void playAttackSound()
        {
            if (_isMoving)
                AudioManagerMiniBattle.audioManager.stopSound(gameObject, gameObject.name + "_Attack_00");
            else
                AudioManagerMiniBattle.audioManager.playSound(gameObject, gameObject.name + "_Attack_00", true);
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using th.nx;
namespace com.tianhe.map.logic
{
    /// <summary>
    /// 单兵
    /// </summary>
    public class Soldier : SpriteBase
    {

        /// <summary>
        /// 行走路径
        /// </summary>
        public IList<AStarNode> paths;
        /// <summary>
        /// 当前行走到第几格了
        /// </summary>
        //public int index = 1;
        /// <summary>
        /// 当前选中的目标点
        /// </summary>
        public Vec2<short> destination;
        public Vector3 _target;
        /// <summary>
        /// 当前是否选中
        /// </summary>
        public bool isSelected;
#region 
        public Soldier()
            : base()
        {

        }
        public Soldier(int x, int y, int type,int uid)
            : base(x, y, type,uid)
        {
            isSelected = false;
            destination = new Vec2<short>(-1, -1);
        }
#endregion

    }
}
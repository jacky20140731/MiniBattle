using UnityEngine;
using System.Collections;
namespace com.tianhe.map.logic
{
    /// <summary>
    /// 建筑
    /// </summary>
    public class Building : SpriteBase
    {
        #region
        public Building()
            : base()
        {

        }
        public Building(int x, int y, int type,int uid)
            : base(x, y, type,uid)
        {

        }
        #endregion
    }
}

using UnityEngine;
using System.Collections;
namespace com.tianhe.map.logic
{
    /// <summary>
    /// 基础精灵体
    /// </summary>
    public class SpriteBase
    {
        /// <summary>
        /// 唯一识别码
        /// </summary>
        public int Uid;
        /// <summary>
        /// 精灵类型
        /// </summary>
        public int type;
        /// <summary>
        /// 精灵体名字
        /// </summary>
        public string name;
        /// <summary>
        /// 模型名称
        /// </summary>
        public string moduleName;
        /// <summary>
        /// 所在格子，屏幕上的行数
        /// </summary>
        public int x;
        /// <summary>
        /// 所在格子，屏幕上的列数
        /// </summary>
        public int y;
        /// <summary>
        /// 长度(正方形的边长)
        /// </summary>
        public int width;

        public SpriteBase() { }

        public SpriteBase(int x, int y, int type,int uid)
        {
            this.x = x;
            this.y = y;
            this.type = type;
            this.Uid = uid;
            this.moduleName = getModuleName(type);
            this.width = getWidth(type);
        }

        public static string getModuleName(int type)
        {
            switch (type)
            {
                case 1:
                    return "1_1";
                case 2:
                    return "2_2";
                case 3:
                    return "4_4";
                case 4:
                    return "6_6";
                case 5:
                    return "8_8";
                case 9:
                    return "Marine";
                case 10:
                    return "Ghost";
                case 11:
                    return "InfestedMarine";
                case 12:
                    return "Marauder";
                case 13:
                    return "Tank";
                case 14:
                    return "VikingAssault";
                default:
                    return null;
            }
        }
        public static int getWidth(int type)
        {
            switch (type)
            {
                case 1:
                    return 2;
                case 2:
                    return 4;
                case 3:
                    return 6;
                case 4:
                    return 8;
                case 5:
                    return 10;
                default:
                    return 0;
            }
        }
    }
}
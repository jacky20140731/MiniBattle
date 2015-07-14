using UnityEngine;
using System.Collections;
using com.tianhe.map.logic;
using th.nx;

namespace com.tianhe.map.sprite
{
    /// <summary>
    /// 建筑精灵体
    /// </summary>
    public class SpriteBuilding : MonoBehaviour
    {
        /// <summary>
        /// 建筑数据
        /// </summary>
        public Building building;
        Map map;
        AsyncDispatchBehaviour dispatcher;
        short[,] mapData;
        float nodeW;
        float nodeH;
        void Start()
        {
            //Log.debug("ENTER SpriteBuilding.Start");

            map = GameObject.FindObjectOfType<Map>();
            dispatcher = map.transform.GetComponent<AsyncDispatchBehaviour>();
            mapData = dispatcher.mapData;
            nodeW = map.nodeW;
            nodeH = map.nodeH;

            //Log.debug("LEAVE SpriteBuilding.Start");
        }

        void OnMouseOver()
        {

        }

        void OnMouseUp_()
        {
            //Log.debug("ENTER SpriteBuilding.OnMouseUp_");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Finish"))
                {
                    for (int i = 0; i < building.width; i++)
                    {
                        dispatcher.mapData[building.x + i, building.y + i] = 0;
                    }
                    Vector3 pos = hit.point;
                    int x, y;
                    getDestination(pos, out y, out x);
                    for (int i = 0; i < building.width; i++)
                    {
                        dispatcher.mapData[x + i, y + i] = (short)building.type;
                    }
                    building.x = x;
                    building.y = y;
                    transform.position = new Vector3(nodeW / 2 + y * nodeW, 0, nodeH / 2 + x * nodeH);
                }
            }

            //Log.debug("LEAVE SpriteBuilding.OnMouseUp_");
        }

        void OnMouseDrag_()
        {
            //Log.debug("ENTER SpriteBuilding.OnMouseDrag_");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Finish"))
                {
                    Vector3 pos = hit.point;
                    pos.y = 0;
                    transform.position = pos;
                }
            }

            //Log.debug("LEAVE SpriteBuilding.OnMouseDrag_");
        }

        public void getDestination(Vector3 pointer, out int x, out int y)
        {
            int x1 = 0, y1 = 0;
            //bool isHitted = false;
            
            if (pointer.x >= 0 && pointer.x < mapData.GetLength(1) * nodeW
                    && pointer.z <= 0 && pointer.z > mapData.GetLength(0) * nodeH)
            {
                x1 = (short)(pointer.x / nodeW);
                y1 = (short)(pointer.z / nodeH);
            }
            else
            {
                x1 = y1 = -1;
            }
            
            x = x1;
            y = y1;
            
            //return isHitted;



            /*
            int xx = 0;
            int yy = 0;
            for (int i = 0; i < mapData.GetLength(0); i++)
            {
                for (int j = 0; j < mapData.GetLength(1); j++)
                {
                    if (pointer.x < 0 || (pointer.x > nodeW / 2 + mapData.GetLength(1) * nodeW) || pointer.z > 0 || (pointer.z < nodeH / 2 + mapData.GetLength(0) * nodeH))
                    {
                        x = -1;
                        y = -1;
                        return;
                    }
                    if (((nodeW + j * nodeW) > pointer.x) && ((nodeH + i * nodeH) < pointer.z))
                    {
                        xx = i;
                        yy = j;
                        x = xx;
                        y = yy;
                        return;
                    }
                }
            }
            x = xx;
            y = yy;
            //*/
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

namespace th.nx
{
    public class AStarPathFinder
    {
        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        public AStarPathFinder(short[,] map, IAStarSpeedConfig speedConfig, IAStarHCostEstimator estimator)
        {
            Debug.Assert(map != null && speedConfig != null && estimator != null);

            _map = map;
            _speedConfig = speedConfig;
            _hCostEstimator = estimator;
            
            _mapSize = new Vec2<short>((short)_map.GetLength(1), (short)_map.GetLength(0));

            _openList = new Dictionary<int, AStarNode>();
            _closeList = new Dictionary<int, AStarNode>();

            _endPos = new Vec2<short>();
            _roleSize = new Vec2<sbyte>();
        }

        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        public AStarPathFinder(short[,] map, IAStarSpeedConfig speedConfig)
                : this(map, speedConfig, new AStarHCostHuffman())
        {
        }

        public Errno findPath(int roleTypeId, 
                              sbyte roleSize,
                              Vec2<short> startPos, 
                              Vec2<short> endPos, 
                              IList<AStarNode> path)
        {
            return findPath(
                    roleTypeId, 
                    new Vec2<sbyte>(roleSize, roleSize),
                    startPos, 
                    endPos, 
                    path);
        }

        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        public Errno findPath(int roleTypeId, 
                              Vec2<sbyte> roleSizeInGrids,
                              Vec2<short> startPos, 
                              Vec2<short> endPos, 
                              IList<AStarNode> path)
        {
            Errno err = Errno.OK;

            if (roleSizeInGrids.x <= 0 || roleSizeInGrids.y <= 0 
                    || startPos.x < 0 || startPos.x >= _mapSize.x || startPos.y < 0 || startPos.y >= _mapSize.y
                    || endPos.x < 0 || endPos.x >= _mapSize.x || endPos.y < 0 || endPos.y >= _mapSize.y
                    || path == null)
                err = Errno.InvalidArg;

            AStarNode node = null;
            if (err == Errno.OK)
            {
                _roleTypeId = roleTypeId;
                _roleSize.assign(roleSizeInGrids);
                _endPos.assign(endPos);

                path.Clear();

                //node = new AStarNode(new Vec2<short>(startPos), _map[startPos.y, startPos.x]);
                node = new AStarNode(new Vec2<short>(startPos));
                node.tileId = _map[startPos.y, startPos.x];
                node.speed = _speedConfig.onGetMoveSpeed(this, _roleTypeId, node.tileId);

                if (Utils.floatCompare(node.speed, 0) <= 0)
                    err = Errno.General;

                Debug.Assert(err == Errno.OK);
            }

            if (err == Errno.OK)
            {
                _openList.Add(getKey(node), node);
                err = doFindPath();
            }

            _closeList.Clear();

            if (err == Errno.OK)
                node = getNodeFromList(getKey(_endPos), _openList);

            _openList.Clear();


            if (err == Errno.OK)
                generatePath(node, path);

            return err;
        }

        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        public short[,] getMapData()
        {
            return _map;
        }

        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        private Errno doFindPath()
        {
            Errno err = 0;

            int key = 0;
            AStarNode curNode = null;

            float [,] borderSpeeds = new float[_roleSize.y + 2, _roleSize.x + 2];
            float[,] neighborSpeeds = new float[3, 3];

            while (err == Errno.OK)
            {
                // 成功找到路径
                key = getKey(_endPos);
                if (getNodeFromList(key, _openList) != null)
                {
                    //Log.debug("FOUND PATH");
                    break;
                }

                if ((curNode = getMinimumCostNode()) == null)
                {
                    // 未能找到路径
                    //Log.debug("CAN NOT FIND PATH");
                    err = Errno.NoFound;
                    break;
                }

                fillBorderSpeeds(curNode, borderSpeeds.GetLength(0), borderSpeeds.GetLength(1), borderSpeeds);

                convertBorderSpeedsToNeighbor(borderSpeeds, 
                                              borderSpeeds.GetLength(0), 
                                              borderSpeeds.GetLength(1), 
                                              curNode, 
                                              neighborSpeeds);

                addNeighborsToOpenList(neighborSpeeds, curNode);
                    
            }  // while

            //Log.debug("END WHILE");

            return err;
        }


        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        private static int getKey(Vec2<short> pos)
        {
            int key = pos.x;
            key <<= 16;
            key += pos.y;

            return key;
        }

        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        private static int getKey(AStarNode node)
        {
            return getKey(node.pos);
        }

        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        private static AStarNode getNodeFromList(int key, IDictionary<int, AStarNode> list)
        {
            AStarNode node = null;
            if (list.ContainsKey(key))
                node = list[key];

            return node;
        }

        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        private void generatePath(AStarNode node, IList<AStarNode> path)
        {
            int n = 0;
            
            path.Clear();
            
            AStarNode node1 = node;
            while (node1 != null)
            {
                path.Add(node);
                node1 = node1.previousNode;
                ++n;
            }
            
            //--n;
            node1 = node;
            while (node1 != null /*&& n > 0*/)
            {
                path[--n] = node1;
                node1 = node1.previousNode;
            }
        }

        private AStarNode getMinimumCostNode()
        {
            AStarNode node = null;
            float minF = float.MaxValue, fValue = 0.0F;
            
            var enumerator = _openList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                fValue = enumerator.Current.Value.costG + enumerator.Current.Value.costH;
                if (fValue <= minF)
                {
                    minF = fValue;
                    node = enumerator.Current.Value;
                }
            }  // while
            
            if (node != null)
            {
                // 将openlist中代价最小的节点从迁移到closelist中，并指定为当前节点
                int key = getKey(node);
                _closeList.Add(key, node);
                _openList.Remove(key);
            }

            return node;
        }

        private void fillBorderSpeeds(AStarNode curNode, int rowNum, int colNum, float[,] speeds)
        {
            Vec2<short> curPos = curNode.pos;
            int row, col;

            for (row = 0; row < rowNum; ++row)
                for (col = 0; col < colNum; ++col)
                {
                    if (row != 0 && row != (rowNum - 1) && col != 0 && col != (colNum - 1))
                        speeds[row, col] = -1;
                    else
                        speeds[row, col] = 0;
                }


            bool isSkip = false;
            Vec2<short> pos = new Vec2<short>();
            int i, j;

            for (row = 0; row < rowNum; ++row)
                for (col = 0; col < colNum; ++col)
                {
                    isSkip = Utils.floatCompare(speeds[row, col], 0) < 0;
                    
                    if (!isSkip)
                    {
                        pos.x = (short)(curPos.x + col - 1);
                        pos.y = (short)(curPos.y + row - 1);
                        
                        if (pos.x < 0 || pos.x >= _mapSize.x)
                        {
                            for (i = 0; i < rowNum; ++i)
                                speeds[i, col] = -1;
                            
                            isSkip = true;
                        }
                    }
                    
                    if (!isSkip && (pos.y < 0 || pos.y >= _mapSize.y))
                    {
                        for (j = 0; j < colNum; ++j)
                            speeds[row, j] = -1;
                        
                        isSkip = true;
                    }
                    
                    if (!isSkip)
                    {
                        speeds[row, col] = _speedConfig.onGetMoveSpeed(
                                this, 
                                _roleTypeId, 
                                (short)_map[pos.y, pos.x]);
                        
                        if (Utils.floatCompare(speeds[row, col], 0) <= 0)
                        {
                            isSkip = true;
                            
                            if (row != 0 && row != (rowNum - 1))
                            {
                                for (i = 0; i < rowNum; ++i)
                                    speeds[i, col] = -1;
                            }
                            else if (col != 0 && col != (colNum - 1))
                            {
                                for (j = 0; j < colNum; ++j)
                                    speeds[row, j] = -1;
                            }
                        }
                    }
                }  // for
        }

        private void convertBorderSpeedsToNeighbor(
                float[,] borderSpeeds, int rowNum, int colNum, AStarNode curNode, 
                float[,] neighborSpeeds)
        {
            Vec2<short> curPos = curNode.pos;
            int row, col;

            for (row = 0; row < 3; ++row)
                for (col = 0; col < 3; ++col)
                    neighborSpeeds[row, col] = 0;
            
            neighborSpeeds[1, 1] = -1;


            bool isSkip = false;
            Vec2<short> pos = new Vec2<short>();
            int key;
            float speed;
            int i, j;
            
            for (row = 0; row < 3; ++row)
                for (col = 0; col < 3; ++col)
                {
                    isSkip = Utils.floatCompare(neighborSpeeds[row, col], 0) < 0;
                    
                    if (!isSkip)
                    {
                        pos.x = (short)(curPos.x + col - 1);
                        pos.y = (short)(curPos.y + row - 1);
                        
                        key = getKey(pos);
                        if (getNodeFromList(key, _closeList) != null)
                        {
                            neighborSpeeds[row, col] = -1;
                            isSkip = true;
                            
                            if (row == 1)
                            {
                                neighborSpeeds[0, col] = -1;
                                neighborSpeeds[2, col] = -1;
                            }
                            else if (col == 1)
                            {
                                neighborSpeeds[row, 0] = -1;
                                neighborSpeeds[row, 2] = -1;
                            }
                        }
                    }
                    
                    if (!isSkip)
                    {
                        speed = 0;
                        
                        if ((row == 0 || row == 2) && (col == 0 || col == 2))
                        {
                            i = (row == 0 ? 0 : rowNum - 1);
                            j = (col == 0 ? 0 : colNum - 1);
                            speed = borderSpeeds[i, j];
                        }
                        else if (row == 0 || row == 2)
                        {
                            i = (row == 0 ? 0 : rowNum - 1);
                            for (j = 1; j < colNum - 1; ++j)
                                speed += borderSpeeds[i, j];
                            
                            speed /= (float)(colNum - 2);
                        }
                        else //if (col == 0 || col == 2)
                        {
                            j = (col == 0 ? 0 : colNum - 1);
                            for (i = 1; i < rowNum - 1; ++i)
                                speed += borderSpeeds[i, j];
                            
                            speed /= (float)(rowNum - 2);
                        }
                        
                        neighborSpeeds[row, col] = speed;
                    }
                }
        }

        private void addNeighborsToOpenList(float[,] neighborSpeeds, AStarNode curNode)
        {
            int row, col;
            float speed, costG;
            Vec2<short> curPos = curNode.pos;
            Vec2<short> pos = new Vec2<short>();
            int key;
            AStarNode node = null;

            for (row = 0; row < 3; ++row)
                for (col = 0; col < 3; ++col)
                {
                    speed = neighborSpeeds[row, col];
                    
                    if (Utils.floatCompare(speed, 0) <= 0)
                        continue;
                    
                    if (row == 1 || col == 1)
                        costG = 10000.0F / speed;
                    else
                        costG = 14142.135623730950488016887242097F / speed;
                    
                    costG += curNode.costG;
                    
                    pos.x = (short)(curPos.x + col - 1);
                    pos.y = (short)(curPos.y + row - 1);
                    
                    key = getKey(pos);
                    node = getNodeFromList(key, _openList);
                    
                    if (node == null)
                    {
                        node = new AStarNode(new Vec2<short>(pos));
                        node.tileId = _map[pos.y, pos.x];
                        node.speed = speed;
                        node.costH = _hCostEstimator.onEvaluateHCost(this, pos, _endPos);
                        node.costG = costG;
                        node.previousNode = curNode;
                        
                        _openList.Add(key, node);
                    }
                    else if (costG < node.costG)
                    {
                        node.costG = costG;
                        node.previousNode = curNode;
                    }
                }  // for
        }
        
        
        private short[,] _map;
        private IAStarSpeedConfig _speedConfig;
        private IAStarHCostEstimator _hCostEstimator;
        
        private Vec2<short> _mapSize;
        
        private IDictionary<int, AStarNode> _openList;
        private IDictionary<int, AStarNode> _closeList;
        
        private int _roleTypeId;
        private Vec2<sbyte> _roleSize;
        private Vec2<short> _endPos;
    }
}

using System;

namespace th.nx
{
    public class AStarHCostHuffman : IAStarHCostEstimator
    {
        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        public float onEvaluateHCost(AStarPathFinder finder, Vec2<short> curPos, Vec2<short> endPos)
        {
            return (float)(Math.Abs(endPos.x - curPos.x) + Math.Abs(endPos.y - curPos.y));
        }
    }
}

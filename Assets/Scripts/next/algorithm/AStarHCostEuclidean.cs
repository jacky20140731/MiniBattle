using System;

namespace th.nx
{
    public class AStarHCostEuclidean : IAStarHCostEstimator
    {
        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        public float onEvaluateHCost(AStarPathFinder finder, Vec2<short> curPos, Vec2<short> endPos)
        {
            int dx = Math.Abs(endPos.x - curPos.x);
            int dy = Math.Abs(endPos.y - curPos.y);
            return (float)Math.Pow(Math.Pow(dx, 2) + Math.Pow(dy, 2), 0.5);
        }
    }
}

using System;

namespace th.nx
{
    public class AStarHCostChebyshev : IAStarHCostEstimator
    {
        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        public float onEvaluateHCost(AStarPathFinder finder, Vec2<short> curPos, Vec2<short> endPos)
        {
            int d1 = Math.Abs(endPos.x - curPos.x);
            int d2 = Math.Abs(endPos.y - curPos.y);
            return Math.Max(d1, d2);
        }
    }
}


namespace th.nx
{
    public interface IAStarHCostEstimator
    {
        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        float onEvaluateHCost(AStarPathFinder finder, Vec2<short> curPos, Vec2<short> endPos);
    }
}

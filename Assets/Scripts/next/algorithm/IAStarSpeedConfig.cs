
namespace th.nx
{
    public interface IAStarSpeedConfig
    {
        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        float onGetMoveSpeed(AStarPathFinder finder, int roleTypeId, short tileId);
    }
}

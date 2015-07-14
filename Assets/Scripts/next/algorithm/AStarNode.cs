using UnityEngine;

namespace th.nx
{
    public class AStarNode
    {
        public AStarNode(Vec2<short> pos)
        {
            this.pos = pos;
        }

        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        public Vec2<short> pos
        {
            get { return _pos; }
            set
            {
                Errno err = (value != null ? Errno.OK : Errno.InvalidArg);
                if (err == Errno.OK && (value.x < 0 || value.y < 0))
                    err = Errno.InvalidArg;

                if (err == Errno.OK)
                    _pos = value;

                Debug.Assert(err == Errno.OK);
            }
        }

        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        public short tileId
        {
            get { return _tileId; }
            set { _tileId = value; }
        }

        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        public float speed
        {
            get { return _speed; }
            set { _speed = value; }
        }


        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        internal float costG
        {
            get { return _costG; }
            set
            {
                Errno err = Errno.OK;
                if (Utils.floatCompare(value, 0) <= 0)
                    err = Errno.InvalidArg;
                else
                    _costG = value;

                Debug.Assert(err == Errno.OK);
            }
        }

        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        internal float costH
        {
            get { return _costH; }
            set
            {
                Errno err = Errno.OK;
                if (Utils.floatCompare(value, 0) < 0)
                    err = Errno.InvalidArg;
                else
                    _costH = value;

                Debug.Assert(err == Errno.OK);
            }
        }

        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        internal AStarNode previousNode
        {
            get { return _previousNode; }
            set { _previousNode = value; }
        }

        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        public override string ToString()
        {
            if (_toString == null)
            {
                _toString = "(" + _pos + " | " + _tileId + ")";
            }

            return _toString;
        }


        private Vec2<short> _pos;
        private short _tileId;
        private float _speed;

        private float _costG;
        private float _costH;

        private AStarNode _previousNode;
        private string _toString;
    }
}

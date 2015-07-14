
namespace th.nx
{
    public static class Utils
    {
        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        public static bool isFloatZero(float f)
        {
            return (floatCompare(f, 0.0F) == 0);
        }

        //*
        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        public static bool isFloatsEqual(float f1, float f2)
        {
            return (floatCompare(f1, f2) == 0);
        }
        //*/

        //*
        //------------------------------------------------------------------------------
        //
        //------------------------------------------------------------------------------
        public static sbyte floatCompare(float f1, float f2)
        {
            sbyte ret = 0;

            float delta = f1 - f2;
            if (delta >= float.Epsilon)
                ret = 1;
            else if (delta <= -float.Epsilon)
                ret = -1;

            return ret;
        }
        //*/


        public static short ntohs(byte[] buff, ref int i)
        {
            short s = (short)((buff[i++] << 8) & 0xFF00);
            s |= (short)((buff[i++]) & 0x00FF);
            return s;
        }

        public static short ntohs(byte[] buff)
        {
            int i = 0;
            return ntohs(buff, ref i);
        }

        public static int ntohi(byte[] buff, ref int i)
        {
            int i1 = (int)((buff[i++] << 24) & 0xFF000000);
            i1 |= (int)((buff[i++] << 16) & 0x00FF0000);
            i1 |= (int)((buff[i++] << 8) & 0x0000FF00);
            i1 |= (int)((buff[i++]) & 0x000000FF);
            return i1;
        }

        public static int ntohi(byte[] buff)
        {
            int i = 0;
            return ntohi(buff, ref i);
        }

        public static long ntohl(byte[] buff, ref int i)
        {
            long l = (long)(((ulong)(buff[i++] << 56)) & 0xFF00000000000000L);
            l |= (long)(((ulong)(buff[i++] << 48)) & 0x00FF000000000000L);
            l |= (long)(((ulong)(buff[i++] << 40)) & 0x0000FF0000000000L);
            l |= (long)(((ulong)(buff[i++] << 32)) & 0x000000FF00000000L);
            l |= (long)(((ulong)(buff[i++] << 24)) & 0x00000000FF000000L);
            l |= (long)(((ulong)(buff[i++] << 16)) & 0x0000000000FF0000L);
            l |= (long)(((ulong)(buff[i++] << 8)) & 0x000000000000FF00L);
            l |= (long)(((ulong)(buff[i++])) & 0x00000000000000FFL);
            return l;
        }

        public static long ntohl(byte[] buff)
        {
            int i = 0;
            return ntohl(buff, ref i);
        }

        public static void htons(short s, byte[] buff, ref int i)
        {
            byte b = (byte)((s >> 8) & 0x00FF);
            buff[i++] = b;
            b = (byte)(s & 0x00FF);
            buff[i++] = b;
        }

        public static void htons(short s, byte[] buff)
        {
            int i = 0;
            htons(s, buff, ref i);
        }

        public static void htoni(int i1, byte[] buff, ref int i)
        {
            byte b = (byte)((i1 >> 24) & 0x000000FF);
            buff[i++] = b;
            b = (byte)((i1 >> 16) & 0x000000FF);
            buff[i++] = b;
            b = (byte)((i1 >> 8) & 0x000000FF);
            buff[i++] = b;
            b = (byte)(i1 & 0x000000FF);
            buff[i++] = b;
        }

        public static void htoni(short s, byte[] buff)
        {
            int i = 0;
            htoni(s, buff, ref i);
        }

        public static void htonl(long l, byte[] buff, ref int i)
        {
            byte b = (byte)((l >> 56) & 0x00000000000000FFL);
            buff[i++] = b;
            b = (byte)((l >> 48) & 0x00000000000000FFL);
            buff[i++] = b;
            b = (byte)((l >> 40) & 0x00000000000000FFL);
            buff[i++] = b;
            b = (byte)((l >> 32) & 0x00000000000000FFL);
            buff[i++] = b;
            b = (byte)((l >> 24) & 0x00000000000000FFL);
            buff[i++] = b;
            b = (byte)((l >> 16) & 0x00000000000000FFL);
            buff[i++] = b;
            b = (byte)((l >> 8) & 0x00000000000000FFL);
            buff[i++] = b;
            b = (byte)(l & 0x00000000000000FFL);
            buff[i++] = b;
        }

        public static void htonl(short s, byte[] buff)
        {
            int i = 0;
            htonl(s, buff, ref i);
        }
    }
}

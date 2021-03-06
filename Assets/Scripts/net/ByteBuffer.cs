using System;
namespace com.tianhe.net
{
    /**
      * 类说明：二进制数组
      * 
      * @version 1.0
      */
    public class ByteBuffer
    {
        /** 当前读的位置 */
        private int readPos = 0;
        /** 当前写的位置 */
        private int writePos = 0;
        /** 内容数组 */
        private byte[] data;
        /** 数字缓冲的数组 */
        private byte[] dateTemp = new byte[8];

        /** 构造一个缓冲区,默认长度是50 */
        public ByteBuffer()
        {
            this.data = new byte[8];
        }

        /** 构造指定的长度的缓冲区 */
        public ByteBuffer(int count)
        {
            this.data = new byte[count];
        }

        /** 用指定的数组构造缓冲区 */
        public ByteBuffer(byte[] data)
        {
            this.data = data;
            this.readPos = 0;
            this.writePos = data.Length;
        }

        /** 用指定的数组,开始位置和长度构造缓冲区 */
        public ByteBuffer(byte[] data, int start, int len)
        {
            this.data = data;
            this.readPos = start;
            this.writePos = start + len;
        }

        /** 写逻辑值 */
        public ByteBuffer writeBoolean(bool b)
        {
            writeByte(b ? 1 : 0);
            return this;
        }

        /** 写字节序列 */
        public ByteBuffer writeByte(int value)
        {
            if (value > Byte.MaxValue || value < Byte.MinValue)
            {
                throw new Exception("越界的值 = " + value);
            }
            writeNumber(value, 1);
            return this;
        }

        /** 写字节序列 */
        public ByteBuffer writeBytes(byte[] bytes)
        {
            writeBytes(bytes, 0, bytes.Length);
            return this;
        }

        /** 写字节序列 */
        public ByteBuffer writeBytes(byte[] bytes, int offset, int count)
        {
            ensureCapacity(this.writePos + count);
            for (int i = 0; i < count; i++)
            {
                this.data[writePos++] = bytes[offset++];
            }
            return this;
        }

        /** 写一个短整数 */
        public ByteBuffer writeShort(int value)
        {
            if (value > short.MaxValue || value < short.MinValue)
            {
                throw new Exception("越界的值 = " + value);
            }
            writeNumber(value, 2);
            return this;
        }

        /** 写一个字符 */
        public ByteBuffer writeChar(char value)
        {
            writeNumber(value, 2);
            return this;
        }

        /** 写一个整数 */
        public ByteBuffer writeInt(int value)
        {
            writeNumber(value, 4);
            return this;
        }

        /** 写一个长整数 */
        public ByteBuffer writeLong(long value)
        {
            writeNumber(value, 8);
            return this;
        }

        /** 写一个字符串 */
        public ByteBuffer writeString(String s)
        {
            if (s == null || s.Length == 0)
            {
                writeShort(0);
            }
            else
            {
                if (s.Length > 32767)
                    throw new Exception("short string over flow");
                int count = s.Length;
                writeShort(count);
                char[] arr = s.ToCharArray();
                for (int i = 0; i < arr.Length; i++)
                    writeChar(arr[i]);
            }
            return this;
        }

        /** 写一个短字符串 */
        public ByteBuffer writeShortString(String s)
        {
            if (s == null || s.Length == 0)
            {
                writeByte(0);
            }
            else
            {
                if (s.Length > 255)
                    throw new Exception("short string over flow");
                int count = s.Length;
                writeByte(count);
                char[] arr = s.ToCharArray();
                for (int i = 0; i < arr.Length; i++)
                    writeChar(arr[i]);
            }
            return this;
        }

        /**
         * 写UTF
         */
        public ByteBuffer writeUTF(String s)
        {
            if (s == null)
            {
                s = "";
            }
            //int strLen = s.Length;
            char[] arr = s.ToCharArray();
            int utfLen = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                char c = arr[i];
                if (c < 127)
                {
                    utfLen++;
                }
                else if (c > 2047)
                {
                    utfLen += 3;
                }
                else
                {
                    utfLen += 2;
                }
            }
            if (utfLen > 65535)
            {
                throw new Exception("the string is too long:" + arr.Length);
            }

            ensureCapacity(utfLen + 2 + writePos);
            writeShort(utfLen);
            for (int i = 0; i < arr.Length; i++)
            {
                char c = arr[i];
                if (c < 127)
                {
                    this.data[writePos++] = (byte)c;
                }
                else if (c > 2047)
                {
                    this.data[writePos++] = (byte)(0xE0 | ((c >> 12) & 0x0F));
                    this.data[writePos++] = (byte)(0x80 | ((c >> 6) & 0x3F));
                    this.data[writePos++] = (byte)(0x80 | ((c >> 0) & 0x3F));
                }
                else
                {
                    this.data[writePos++] = (byte)(0xC0 | ((c >> 6) & 0x1F));
                    this.data[writePos++] = (byte)(0x80 | ((c >> 0) & 0x3F));
                }
            }
            return this;
        }

        /**
         * 写一个ByteBuffer
         */
        public ByteBuffer writeByteBuffer(ByteBuffer buffer)
        {
            writeByteBuffer(buffer, buffer.available());
            return this;
        }

        /**
         * 写一个ByteBuffer的指定长度的内容
         */
        public ByteBuffer writeByteBuffer(ByteBuffer buffer, int count)
        {
            int realCount = Math.Min(count, buffer.available());
            ensureCapacity(wPos() + realCount);
            for (int i = 0; i < realCount; i++)
                this.data[this.writePos++] = buffer.data[buffer.readPos++];
            return this;
        }

        /** 将缓冲区的内容写到输出流 */
        //public void writeTo(OutputStream out) throws IOException {
        //    for(; available() > 0; ){
        //        out.write(readByte());
        //    }
        //}

        /** 读逻辑值 */
        public bool readBoolean()
        {
            return readByte() != 0;
        }

        /** 读一个字节 */
        public byte readByte()
        {
            return this.data[this.readPos++];
        }

        /** 读一个无符号字节 */
        public int readUnsignedByte()
        {
            return this.data[this.readPos++] & 0xff;
        }

        /** 读指定长度的字节数组 */
        public byte[] readBytes(int count)
        {
            byte[] dest = new byte[count];
            for (int i = 0; i < count; i++)
                dest[i] = this.data[this.readPos++];
            return dest;
        }

        /** 读一个短整数 */
        public short readShort()
        {
            return (short)(readNumber(2) & 0xffff);
        }

        /** 读一个字符 */
        public char readChar()
        {
            return (char)(readNumber(2) & 0xffff);
        }

        /** 读一个无符号短整数 */
        public int readUnsignedShort()
        {
            return (int)(readNumber(2) & 0xffff);
        }

        /** 读一个整数 */
        public int readInt()
        {
            return (int)(readNumber(4) & 0xffffffff);
        }

        /** 读一个长整数 */
        public long readLong()
        {
            return readNumber(8);
        }

        /** 读出一个短字符串 */
        public String readShortString()
        {
            int len = readUnsignedByte();
            if (len == 0)
                return "";
            string buff = "";
            for (int i = 0; i < len; i++)
            {
                buff += (readChar());
            }
            return buff;
        }

        /** 读出一个字符串 */
        public String readString()
        {
            int len = readUnsignedShort();
            if (len == 0)
                return "";
            string buff = "";
            for (int i = 0; i < len; i++)
            {
                buff += (readChar());
            }
            return buff;
        }

        /**
         * 读UTF
         */
        public String readUTF()
        {
            int utflen = readUnsignedShort();
            if (utflen == 0)
                return "";
            char[] charArray = new char[utflen];
            int count = 0;
            int b1 = 0, b2 = 0, b3 = 0;
            int endpos = readPos + utflen;
            while (this.readPos < endpos)
            {
                b1 = this.data[this.readPos++] & 0xff;
                if (b1 < 127)
                {
                    charArray[count++] = (char)b1;
                }
                else if ((b1 >> 5) == 7)
                {
                    b2 = this.data[this.readPos++];
                    b3 = this.data[this.readPos++];
                    charArray[count++] = (char)((b1 & 0xf) << 12
                            | (b2 & 0x3f) << 6 | (b3 & 0x3f));
                }
                else
                {
                    b2 = this.data[this.readPos++];
                    charArray[count++] = (char)((b1 & 0x1f) << 6 | (b2 & 0x3f));
                }
            }
            return new String(charArray, 0, count);
        }

        /** 从输入流读出 */
        //public void readFrom(InputStream in) throws IOException {
        //    readFrom(in, in.available());
        //}


        /** 从输入流读出 */
        //public void readFrom(InputStream in, int len) throws IOException {
        //    for (int i = 0; i < len; i++){
        //        writeByte(in.read());
        //    }
        //}

        /** 缓冲区的总大小（包括未写的大小） */
        public int capacity()
        {
            return this.data.Length;
        }

        /** 缓冲区实际写到的位置 */
        public int wPos()
        {
            return this.writePos;
        }
        public void wPos(int pos)
        {
            if (pos >= 0 && pos < data.Length)
                this.writePos = pos;
            else
                throw new Exception("写操作下标越界");
        }
        public int rPos()
        {
            return this.readPos;
        }

        public void rPos(int pos)
        {
            if (pos >= 0 && pos <= this.writePos)
            {
                this.readPos = pos;
            }
            else
            {
                throw new Exception("位置越界: " + pos);
            }
        }

        /** 获得字节内容的一份副本(只包含有效的字节,也就是从readPos到writePos的位置) */
        public byte[] getBytesCopy()
        {
            byte[] bytes = new byte[wPos()];
            Array.Copy(this.data, this.readPos, bytes, 0, available());
            return bytes;
        }

        /** 获得字节内容 */
        public byte[] getBytes()
        {
            return this.data;
        }

        /** 有效字节数 */
        public int available()
        {
            return this.writePos - this.readPos;
        }

        /** 清除缓冲区 */
        public void clear()
        {
            this.writePos = this.readPos = 0;
        }

        /** 重置缓冲区,使它可以重新被读 */
        public void resetRead()
        {
            this.readPos = 0;
        }

        /**
         * 压缩缓冲区
         */
        public void compack()
        {
            if (this.readPos == 0)
            {
                return;
            }
            int count = available();
            for (int i = 0; i < count; i++)
            {
                this.data[i] = this.data[this.readPos++];
            }
            this.readPos = 0;
            this.writePos = count;
        }
        /// <summary>
        /// 压缩指定长度的缓冲区
        /// </summary>
        /// <param name="start">从start开始保留缓冲区后面的数据</param>
        public void compack(int start)
        {
            if(this.readPos==0)
                return;
            int count = available();
            if (start >= count)
            {
                this.readPos = 0;
                this.writePos = 0;
            }
            else
            {
                for (int i = 0, b = start; b < count; i++,b++)
                {
                    this.data[i] = this.data[b];
                }
                this.readPos = 0;
                this.writePos = count;
            }
        }
        /// <summary>
        /// 压缩指定长度的缓冲区
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public byte[] compack(int start, int count)
        {
            byte[] bytes = new byte[count];
            for (int i = start, index = 0; index < count; i++, index++)
            {
                bytes[index] = this.data[i];
            }
            return bytes;
        }
        /// <summary>
        /// 压缩缓冲区(临时使用)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public void compack(int start, int count,bool temp)
        {
            this.data = compack(start, count);
            this.readPos = 0;
            this.writePos = count;
        }
        /** 把字节序列转换成字符串 */
        //public String toString() {
        //    return new String(this.data, 0, this.writePos);
        //}

        /** 把自己的内容复制一个新的ByteBuffer */
        public ByteBuffer clone()
        {
            ByteBuffer buff = new ByteBuffer(this.writePos);
            Array.Copy(this.data, 0, buff.data, 0, this.writePos);
            buff.writePos = this.writePos;
            buff.readPos = this.readPos;
            return buff;
        }

        /** 写字节序列到缓冲区 */
        private void write(byte[] bytes, int offset, int len, int start)
        {
            ensureCapacity(start + len);
            Array.Copy(bytes, offset, this.data, start, len);
        }

        /** 冲缓冲区读出字节序列 */
        public void read(byte[] bytes, int offset, int len, int start)
        {
            Array.Copy(this.data, start, bytes, offset, len);
        }

        /** 保证缓冲区的大小 */
        private void ensureCapacity(int count)
        {
            if (count > this.data.Length)
            {
                byte[] tmp = new byte[count * 3 / 2];
                Array.Copy(this.data, 0, tmp, 0, this.writePos);
                this.data = tmp;

                /*	if(count > WARN_SIZE){
                        log.error(BaseUtils.str_C("ByteBuffer容量超过预估值，当前 = ", count + "", "，风险值 = ", WARN_SIZE + ""), new RuntimeException());
                    }
                */
            }
        }

        /** 写一个数字到缓冲区 */
        private void writeNumber(long value, int bytes)
        {
            for (int i = 0, j = bytes - 1; i < bytes && j>=0; i++, j--)
            {
                this.dateTemp[i] = (byte)(value >> j * 8);
            }
            write(this.dateTemp, 0, bytes, this.writePos);
            this.writePos += bytes;
        }

        /** 从缓冲区读一个数字到数字缓冲区 */
        private long readNumber(int bytes)
        {
            for (int i = 0; i < bytes; i++)
            {
                this.dateTemp[i] = this.data[this.readPos++];
            }
            long value = 0;
            for (int i = bytes - 1,j = 0; i >= 0 && j<bytes ; i--,j++)
            {
                value |= (long)(this.dateTemp[j] & 0xff) << (i * 8);
            }
            return value;
        }
    }

}
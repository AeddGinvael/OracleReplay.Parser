using Oracle.BitStream;

namespace Oracle.Packets.Unpacker
{
    public interface Packer
    {
        
    }
    public interface Unpacker<T>: Packer
    {
        T Unpack(BitArrayStream stream);
    }
}
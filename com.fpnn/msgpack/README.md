## FPNN MsgPack Implement

* **[msgpack SPEC](https://github.com/msgpack/msgpack/blob/master/spec.md)**

### For Packer:

* input kinds:

    null, bool, sbyte, byte, short, ushort, Int32, UInt32, Int64, Uint64, float, double, string

    Decimal, Tuple

    byte[], DateTime, IEnumerable (such as List, Dictionary, List<object>, Dictionary<object, object>, ...)

* usage:

        using com.fpnn.msgpack;
        void MsgPacker.Pack(Stream stream, Object obj);


### For Unpacker:

* output kinds:

    null, bool, sbyte, byte, short, ushort, Int32, UInt32, Int64, Uint64, float, double, string

    byte[], DateTime, List<object>, Dictionary<object, object>

* usage:

        using com.fpnn.msgpack;
        Dictionary<Object, Object> MsgUnpacker.Unpack(byte[] binary);
        Dictionary<Object, Object> MsgUnpacker.Unpack(byte[] binary, int offset);

        //-- unpack one object.
        Object MsgUnpacker.Unpack(byte[] binary, int offset, out int endOffset);
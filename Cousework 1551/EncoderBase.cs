public abstract class EncoderBase
{
    protected string InputString;
    protected int ShiftValue;

    // Constructor
    public EncoderBase(string input, int shift)
    {
        InputString = input;
        ShiftValue = shift;
    }

    // Abstract method for encoding
    public abstract string Encode();

    // Abstract method for decoding (reverse encoding)
    public abstract string Decode();
}

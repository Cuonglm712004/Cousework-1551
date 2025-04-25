using System;
using System.Text;

public class CaesarEncoder : EncoderBase
{
    // Constructor calls base class constructor
    public CaesarEncoder(string input, int shift) : base(input, shift)
    {
    }

    // Implementation of the Encode method
    public override string Encode()
    {
        StringBuilder encodedString = new StringBuilder();

        foreach (char c in InputString)
        {
            if (char.IsLetter(c))
            {
                char offset = char.IsUpper(c) ? 'A' : 'a';
                encodedString.Append((char)((((c + ShiftValue) - offset) % 26 + 26) % 26 + offset));
            }
            else
            {
                encodedString.Append(c);
            }
        }

        return encodedString.ToString();
    }

    // Implementation of the Decode method (reverse the shift)
    public override string Decode()
    {
        return Encode(); // For Caesar Cipher, decoding is the same as encoding with a negative shift
    }
}

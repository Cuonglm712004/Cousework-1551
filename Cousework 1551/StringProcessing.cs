using System;

public class StringProcessing
{
    private EncoderBase _encoder;

    // Constructor: Takes encoder object to enable polymorphism
    public StringProcessing(EncoderBase encoder)
    {
        _encoder = encoder;
    }

    // Method to perform encoding and return encoded string
    public string ProcessEncoding()
    {
        return _encoder.Encode();
    }

    // Method to perform decoding (reverse encoding) and return decoded string
    public string ProcessDecoding()
    {
        return _encoder.Decode();
    }

    // Method to print the encoded string
    public string PrintEncoded()
    {
        return ProcessEncoding();
    }

    // Method to save the encoded string to the database
    public void SaveToDatabase()
    {
        DatabaseHelper.SaveEncodedString(_encoder.InputString, ProcessEncoding(), _encoder.ShiftValue);
    }

    // Method to show ASCII codes of input and encoded strings
    public void ShowAsciiCodes()
    {
        int[] inputCodes = InputCode();
        int[] outputCodes = OutputCode();

        Console.WriteLine("Input ASCII Codes: ");
        foreach (var code in inputCodes)
        {
            Console.Write(code + " ");
        }

        Console.WriteLine("\nEncoded ASCII Codes: ");
        foreach (var code in outputCodes)
        {
            Console.Write(code + " ");
        }
    }

    // Method to get input string ASCII codes
    public int[] InputCode()
    {
        int[] asciiValues = new int[_encoder.InputString.Length];
        for (int i = 0; i < _encoder.InputString.Length; i++)
        {
            asciiValues[i] = (int)_encoder.InputString[i];
        }
        return asciiValues;
    }

    // Method to get encoded string ASCII codes
    public int[] OutputCode()
    {
        string encoded = ProcessEncoding();
        int[] asciiValues = new int[encoded.Length];
        for (int i = 0; i < encoded.Length; i++)
        {
            asciiValues[i] = (int)encoded[i];
        }
        return asciiValues;
    }

    // Method to sort the input string alphabetically
    public string SortInput()
    {
        char[] sortedChars = _encoder.InputString.ToCharArray();
        Array.Sort(sortedChars);
        return new string(sortedChars);
    }
}

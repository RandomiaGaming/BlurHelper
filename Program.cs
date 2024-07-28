using BlurHelper;
using System.Linq;
public static class Program
{
    [System.STAThread]
    public static void Main()
    {
        BlurRenderringHelper.Render("E:\\TITOAAOGMI99MOL\\Unblurred Frames", "E:\\TITOAAOGMI99MOL\\Frames", (uint)System.IO.Directory.GetFiles("E:\\TITOAAOGMI99MOL\\Unblurred Frames").LongLength, new System.Drawing.Size(192, 108), true, "E:\\TITOAAOGMI99MOL\\Keyframe Data.txt");
    }
    public static string ConcatStringArray(string[] array)
    {
        long outputLength = 0;
        foreach (string str in array)
        {
            outputLength += str.Length;
        }
        char[] outputCharArray = new char[outputLength];
        long currentIndex = 0;
        foreach (string str in array)
        {
            System.Array.Copy(str.ToCharArray(), 0, outputCharArray, currentIndex, str.Length);
            currentIndex += str.Length;
        }
        return new string(outputCharArray);
    }
    public static int[] ParseStringArray(string[] array)
    {
        int[] output = new int[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            output[i] = int.Parse(array[i]);
        }
        return output;
    }
}
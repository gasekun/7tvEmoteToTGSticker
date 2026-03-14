using System.Text;

namespace _7tvEmoteToTGSticker.Tools;

public static class StringExt
{
    public static string ReplaceChars(this string original, char[] charsToReplace, char replacementChar)
    {
        // Use a HashSet for efficient O(1) lookups
        var replaceable = new HashSet<char>(charsToReplace);
        var builder = new StringBuilder(original.Length); // Initialize capacity once

        foreach (var character in original)
            if (replaceable.Contains(character))
                builder.Append(replacementChar);
            else
                builder.Append(character);

        return builder.ToString();
    }
}
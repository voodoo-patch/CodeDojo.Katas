using System.Text;

namespace CodeDojo.Katas.Braille;

public class BrailleTranslator
{
    private const char BrailleSpaceLetter = '\u2800';

    public static string Translate(string text, int wrapAt = 100)
    {
        int lineLength = 0;
        var translation = new StringBuilder();
        var word = new StringBuilder();
        foreach (var letter in text)
        {
            var brailleLetter = TranslateLetter(letter);
            lineLength++;
            bool isSpace = brailleLetter == BrailleSpaceLetter;
            if (isSpace)
            {
                if (lineLength > wrapAt + 1)
                {
                    translation.Remove(translation.Length - 1, 1);
                    translation.Append('\n');
                    lineLength = word.Length + 1;
                }
                
                word.Append(brailleLetter);
                
                translation.Append(word.ToString());
                
                word.Clear();
            }
            else
                word.Append(brailleLetter);
        }

        if (word.Length > 0)
        {
            if (translation.Length > 0 &&  lineLength > wrapAt)
            {
                translation.Remove(translation.Length - 1, 1);
                translation.Append('\n');
            }
            translation.Append(word.ToString());
        }

        return translation.ToString();
    }

    private static char TranslateLetter(char letter) =>
        letter.ToString().ToLowerInvariant().ToCharArray()[0] switch
        {
            'a' => '⠁',
            'b' => '⠃',
            'c' => '⠉',
            'd' => '⠙',
            'e' => '⠑',
            'f' => '⠋',
            'g' => '⠛',
            'h' => '⠓',
            'i' => '⠊',
            'j' => '⠚',
            'k' => '⠅',
            'l' => '⠇',
            'm' => '⠍',
            'n' => '⠝',
            'o' => '⠕',
            'p' => '⠏',
            'q' => '⠟',
            'r' => '⠗',
            's' => '⠎',
            't' => '⠞',
            'u' => '⠥',
            'v' => '⠧',
            'w' => '⠺',
            'x' => '⠭',
            'y' => '⠽',
            'z' => '⠵',
            ' ' => BrailleSpaceLetter,
            _ => throw new ArgumentOutOfRangeException()
        };
}
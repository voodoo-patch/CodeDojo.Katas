using System.Text;

namespace CodeDojo.Katas.Braille;

public class BrailleTranslator
{
    public static string Translate(string text) =>
        string.Join("", text
            .Select(ch => TranslateLetter(ch.ToString())));

    private static string TranslateLetter(string letter) =>
        letter.ToLowerInvariant() switch
        {
            "a" => "⠁",
            "b" => "⠃",
            "c" => "⠉",
            "d" => "⠙",
            "e" => "⠑",
            "f" => "⠋",
            "g" => "⠛",
            "h" => "⠓",
            "i" => "⠊",
            "j" => "⠚",
            "k" => "⠅",
            "l" => "⠇",
            "m" => "⠍",
            "n" => "⠝",
            "o" => "⠕",
            "p" => "⠏",
            "q" => "⠟",
            "r" => "⠗",
            "s" => "⠎",
            "t" => "⠞",
            "u" => "⠥",
            "v" => "⠧",
            "w" => "⠺",
            "x" => "⠭",
            "y" => "⠽",
            "z" => "⠵",
            " " => "\u2800",
            _ => throw new ArgumentOutOfRangeException()
        };
}

public class BrailleTranslatorTests
{
    /*
     * As a user
     * When I parse a text string
     * I can output it as braille on the screen visibly
     */
    [Theory]
    [InlineData("hello", "\u2813\u2811\u2807\u2807\u2815")]
    [InlineData("world", "\u283a\u2815\u2817\u2807\u2819")]
    [InlineData("hello world", "\u2813\u2811\u2807\u2807\u2815\u2800\u283a\u2815\u2817\u2807\u2819")]
    public void Translate_GivenString_ShouldOutputBraille(string text, string expectedBraille)
    {
        var braille = BrailleTranslator.Translate(text);
        braille.Should().Be(expectedBraille);
    }
}
namespace CodeDojo.Katas.Braille;

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
    public void Translate_given_string_should_output_braille(string text, string expectedBraille)
    {
        var braille = BrailleTranslator.Translate(text);
        braille.Should().Be(expectedBraille);
    }
    
    /*
     * As a user
     * When I type in a long sentence
     * It word wraps my braille on the screen
     */
    [Theory]
    [InlineData("hello how are you", 
        "\u2813\u2811\u2807\u2807\u2815\u2800\u2813\u2815\u283a\n\u2801\u2817\u2811\u2800\u283d\u2815\u2825")]
    [InlineData("lorem ipsum dolor sit amet", 
        "\u2807\u2815\u2817\u2811\u280d\u2800\u280a\u280f\u280e\u2825\u280d\n\u2819\u2815\u2807\u2815\u2817\u2800\u280e\u280a\u281e\n\u2801\u280d\u2811\u281e")]
    [InlineData("verylongword", "\u2827\u2811\u2817\u283d\u2807\u2815\u281d\u281b\u283a\u2815\u2817\u2819")]
    public void Translate_given_long_string_should_wrap_words_every_11_chars(string text, string expectedBraille)
    {
        var braille = BrailleTranslator.Translate(text, 11);
        braille.Should().Be(expectedBraille);
    }
}
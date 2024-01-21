using CourierHub.Shared.Static;

namespace CourierHub.Test;
public class Base64CoderTest {
    [Fact]
    public void Encode_ShouldEncodeCorrectly_WhenProvidedString() {
        // Arrange
        string text = "AlaMaKotaW2024";
        string code = "QWxhTWFLb3RhVzIwMjQ=";
        // Act
        string result = Base64Coder.Encode(text);
        // Assert
        Assert.Equal(code, result);
    }
    [Fact]
    public void Decode_ShouldDecodeCorrectly_WhenProvidedString() {
        // Arrange
        string text = "AlaMaKotaW2024";
        string code = "QWxhTWFLb3RhVzIwMjQ=";
        // Act
        string result = Base64Coder.Decode(code);
        // Assert
        Assert.Equal(text, result);
    }

}

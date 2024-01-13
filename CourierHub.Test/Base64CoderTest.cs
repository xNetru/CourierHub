using CourierHub.Shared.Controllers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Static;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

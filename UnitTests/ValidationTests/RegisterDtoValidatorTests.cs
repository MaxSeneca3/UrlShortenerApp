using Xunit;
using BusinessLogic.Dtos;
using BusinessLogic.Validators;
using FluentValidation.TestHelper;

namespace UnitTests.ValidationTests;

public class RegisterDtoValidatorTests
{
    private readonly RegisterDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Null()
    {
        var model = new RegisterDto { Email = null };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email cannot be null");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = new RegisterDto { Email = "" };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new RegisterDto { Email = "invalid-email" };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email address");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Null()
    {
        var model = new RegisterDto { Password = null };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password cannot be null");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new RegisterDto { Password = "" };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required");
    }

    [Theory]
    [InlineData("12345", "Password must be at least 6 characters long")]
    [InlineData("123456", "Password must contain at least one uppercase letter")]
    [InlineData("abcdef", "Password must contain at least one number")]
    [InlineData("ABCDEF", "Password must contain at least one number")]
    [InlineData("Abcdef", "Password must contain at least one special character")]
    public void Should_Have_Error_When_Password_Does_Not_Meet_Criteria(string password, string expectedError)
    {
        var model = new RegisterDto { Password = password };
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(expectedError);
    }
}
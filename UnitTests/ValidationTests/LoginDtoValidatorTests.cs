using BusinessLogic.Dtos;
using BusinessLogic.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace UnitTests.ValidationTests;

public class LoginDtoValidatorTests
{
    private readonly LoginDtoValidator _validator = new();

    [Fact]
    public void Validate_ShouldHaveError_WhenEmailIsEmpty()
    {
        var model = new LoginDto { Email = "" };
        var result = _validator.TestValidate(model);
    
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenEmailIsInvalid()
    {
        var model = new LoginDto { Email = "invalid-email" };
        var result = _validator.TestValidate(model);
    
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenPasswordIsEmpty()
    {
        var model = new LoginDto { Password = "" };
        var result = _validator.TestValidate(model);
    
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_ShouldNotHaveErrors_WhenAllFieldsAreValid()
    {
        var model = new LoginDto { Email = "test@example.com", Password = "ValidPassword123" };
        var result = _validator.TestValidate(model);
    
        result.ShouldNotHaveAnyValidationErrors();
    }

}
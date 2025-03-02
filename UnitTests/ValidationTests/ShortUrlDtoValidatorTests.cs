using FluentValidation.TestHelper;
using BusinessLogic.Dtos;
using BusinessLogic.Validators;
using Xunit;

namespace UnitTests.ValidationTests
{
    public class CreateShortUrlDtoValidatorTests
    {
        private readonly CreateShortUrlDtoValidator _validator;

        public CreateShortUrlDtoValidatorTests()
        {
            _validator = new CreateShortUrlDtoValidator();
        }

        // Test when OriginalUrl is empty
        [Fact]
        public void Should_Have_Error_When_OriginalUrl_Is_Empty()
        {
            var dto = new CreateShortUrlDto { OriginalUrl = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.OriginalUrl);
        }

        // Test when OriginalUrl is invalid
        [Fact]
        public void Should_Have_Error_When_OriginalUrl_Is_Invalid()
        {
            var dto = new CreateShortUrlDto { OriginalUrl = "invalid-url" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.OriginalUrl);
        }

        // Test when OriginalUrl is valid
        [Fact]
        public void Should_Not_Have_Error_When_OriginalUrl_Is_Valid()
        {
            var dto = new CreateShortUrlDto { OriginalUrl = "http://example.com" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.OriginalUrl);
        }

        // Test when UserId is empty
        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            var dto = new CreateShortUrlDto { UserId = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        // Test when UserId is valid
        [Fact]
        public void Should_Not_Have_Error_When_UserId_Is_Valid()
        {
            var dto = new CreateShortUrlDto { UserId = "userId123" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }

        // Test when UserName is empty
        [Fact]
        public void Should_Have_Error_When_UserName_Is_Empty()
        {
            var dto = new CreateShortUrlDto { UserName = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.UserName);
        }

        // Test when UserName is too long
        [Fact]
        public void Should_Have_Error_When_UserName_Is_Too_Long()
        {
            var dto = new CreateShortUrlDto { UserName = new string('A', 51) }; // More than 50 characters
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.UserName);
        }

        // Test when UserName is valid
        [Fact]
        public void Should_Not_Have_Error_When_UserName_Is_Valid()
        {
            var dto = new CreateShortUrlDto { UserName = "testuser" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        }

        // Test when Role is empty
        [Fact]
        public void Should_Have_Error_When_Role_Is_Empty()
        {
            var dto = new CreateShortUrlDto { Role = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Role);
        }

        // Test when Role is invalid
        [Fact]
        public void Should_Have_Error_When_Role_Is_Invalid()
        {
            var dto = new CreateShortUrlDto { Role = "InvalidRole" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Role);
        }

        // Test when Role is valid
        [Fact]
        public void Should_Not_Have_Error_When_Role_Is_Valid()
        {
            var dto = new CreateShortUrlDto { Role = "Admin" };
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.Role);

            dto.Role = "User";
            result = _validator.TestValidate(dto);
            result.ShouldNotHaveValidationErrorFor(x => x.Role);
        }

        // Test when all fields are valid
        [Fact]
        public void Should_Not_Have_Any_Validation_Errors_When_All_Fields_Are_Valid()
        {
            var dto = new CreateShortUrlDto
            {
                OriginalUrl = "http://example.com",
                UserId = "userId123",
                UserName = "testuser",
                Role = "User"
            };

            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

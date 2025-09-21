using FluentAssertions;
using OrderOperations.Application.DTOs.ProductDTOs;
using OrderOperations.Application.Validator.Product;

namespace OrderOperations.UnitTests.Validators
{
    public class CreateProductValidatorTests
    {
        private readonly CreateProductValidator _validator;

        public CreateProductValidatorTests()
        {
            _validator = new CreateProductValidator();
        }

        [Fact]
        public void Validator_ShouldFail_WhenNameIsEmpty()
        {
            var model = new CreateProductViewModel
            {
                Name = "",
                Description = "Description",
                CategoryId = 1,
                Price = 10.0
            };

            var result = _validator.Validate(model);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
        }

        [Fact]
        public void Validator_ShouldPass_WhenAllFieldsAreValid()
        {
            var model = new CreateProductViewModel
            {
                Name = "Valid Product",
                Description = "Valid Description",
                CategoryId = 1,
                Price = 20.0
            };

            var result = _validator.Validate(model);
            result.IsValid.Should().BeTrue();
        }
    }
}

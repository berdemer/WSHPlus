using FluentValidation;

namespace ISG.Business.ValidationRules
{
    public class FluentValidatorTool
    {
        public static void Validate(IValidator validator, object entity)
        {
            var result = validator.Validate(entity);
            if (result.Errors.Count > 0) {//hata sayısı 0 dan fazla ise hataları fırlat...
                throw new ValidationException(result.Errors);
            }         
        }
    }
}

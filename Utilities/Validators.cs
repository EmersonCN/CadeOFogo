using System;
using System.ComponentModel.DataAnnotations;

namespace CadeOFogo.Utilities
{
  public class DataPassadoAttribute : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var data_a_validar = Convert.ToDateTime(value);
      return data_a_validar <= DateTime.UtcNow ? ValidationResult.Success : new ValidationResult(ErrorMessage);
    }
  }
}
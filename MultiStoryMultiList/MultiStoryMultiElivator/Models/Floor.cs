using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Configuration;

namespace MultiStoryMultiElivator.Models
{
    public class Floor : IValidatableObject
    {
        [Required]
        [Range(-2,10, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int FloorNum { get; set; }
        public Floor(int FloorNum)
        {
            this.FloorNum = FloorNum;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            Validator.TryValidateProperty(this.FloorNum,
                    new ValidationContext(this, null, null) { MemberName = "FloorNum" },
                    results);
            return results;

        }


    }
}

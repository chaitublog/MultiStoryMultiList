using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MultiStoryEf.Calc.Models
{
    public class Floor : IValidatableObject
    {
        [Required]
        [Range(-2, 10, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int FloorNum { get; set; }

       

        private ObservableCollection<Elevator> _elevators;

        public ObservableCollection<Elevator> elevators
        {
            get
            {
                if (_elevators == null)
                {
                    this._elevators = new ObservableCollection<Elevator>();
                    this._elevators.CollectionChanged += collectionChanged;
                }
                return this._elevators;
            }
            set
            {
                _elevators = value;
            }
            
        }
        

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
        private void collectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            e.Action.ToString();

        }

        public List<ValidationResult> ValidateFloor()
        {
            bool validateAllProperties = false;

            var results = new List<ValidationResult>();
            var destRes = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this,
                                                        new ValidationContext(this, null, null),
                                                        results,
                                                        validateAllProperties);


            return results;

        }

    }
}

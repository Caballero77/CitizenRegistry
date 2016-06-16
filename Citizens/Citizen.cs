using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citizens
{
    public class Citizen : ICitizen
    {
        private string vatId = string.Empty;

        private string firstName;

        private DateTime dateOfBirth;

        private string lastName;

        private Gender gender;

        public Citizen(string firstName, string lastName, DateTime dateOfBirth, Gender gender)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.BirthDate = dateOfBirth;
            this.Gender = gender;
        }

        public string FirstName
        {
            get
            {
                return this.firstName;
            }

            set
            {
                this.firstName = char.ToUpper(value[0]) + new string(value.ToLower().Skip(1).ToArray());
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }

            set
            {
                this.lastName = char.ToUpper(value[0]) + new string(value.ToLower().Skip(1).ToArray());
            }
        }

        public DateTime BirthDate
        {
            get
            {
                return this.dateOfBirth;
            }

            private set
            {
                if (value > SystemDateTime.Now())
                {
                    throw new ArgumentException("Are you shore about birth date? I don`t think so.");
                }

                this.dateOfBirth = value.Date;
            }
        }

        public Gender Gender
        {
            get
            {
                return this.gender;
            }

            private set
            {
                if (!Enum.IsDefined(typeof(Gender), value))
                {
                    throw new ArgumentOutOfRangeException("Invalid gender");
                }

                this.gender = value;
            }
        }

        public string VatId { get; set; }
    }
}

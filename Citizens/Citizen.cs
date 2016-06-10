using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citizens
{
    public class Citizen:ICitizen
    {
        private string vatId = String.Empty;

        private string firstName;

        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = (Char.ToUpper(value[0]) + new string(value.ToLower().Reverse().Take(value.Length - 1).Reverse().ToArray()));
            }
        }

        private string lastName;

        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = (Char.ToUpper(value[0]) + new string(value.ToLower().Reverse().Take(value.Length - 1).Reverse().ToArray()));
            }
        }

        private DateTime dateOfBirth;

        private Gender gender;

        public DateTime BirthDate
        {
            get
            {
                return dateOfBirth;
            }
            private set
            {
                if(value > SystemDateTime.Now())
                {
                    throw new ArgumentException("Are you shore about birth date? I don`t think so.");
                }
                dateOfBirth = value.Date;

            }
        }

        public Gender Gender
        {
            get
            {
                return gender;
            }
            private set
            {
                if(((Int32)value >= 2) || ((Int32)value < 0))
                {
                    throw new ArgumentOutOfRangeException("Invalid gender");
                }
                gender = value;
            }
        }

        public string VatId { get; set; }

        public Citizen(string firstName, string lastName, DateTime dateOfBirth, Gender gender)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = dateOfBirth;
            Gender = gender;
        }
    }
}

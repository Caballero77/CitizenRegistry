namespace Citizens
{
    using System;
    using System.Linq;
    using Humanizer;
    using System.Threading;

    public class SimpleRegistry : ICitizenRegistry
    {
        private ICitizen[] citizens = new ICitizen[20];

        private int[] genderCounts = new int[2];

        public ICitizen this[string id]
        {
            get
            {
                if(String.IsNullOrEmpty(id))
                {
                    throw new ArgumentNullException();
                }
                foreach(Citizen c in citizens)
                    if(c!=null)
                    {
                        if(c.VatId == id)
                        {
                            return c;
                        }
                    }
                return null;
            }
        }

        private DateTime? lastRegistrationDateTime;

        private int counterForAddNewCitizen = 0;

        public void Register(ICitizen citizen)
        {
            if (citizens.Contains(citizen, new Helpers.CitizenEqualsComparer()))
            {
                throw new InvalidOperationException("You already add this citizen in registry.");
            }

            var newCitizen = new Citizen(citizen.FirstName, citizen.LastName, citizen.BirthDate, citizen.Gender);

            if (string.IsNullOrEmpty(citizen.VatId))
            {
                citizen.VatId = newCitizen.VatId = this.CreateId(newCitizen);
            }
            else
            {
                newCitizen.VatId = citizen.VatId;
            }

            for (int i = 0; i < citizens.Length; ++i)
            {
                if(citizens[i] == null)
                {
                    citizens[i] = newCitizen;
                    genderCounts[(int)newCitizen.Gender]++;
                    lastRegistrationDateTime = SystemDateTime.Now();
                    break;
                }
            }

            if(citizens[citizens.Length - 1] != null)
            {
                DoubleCitizens();
            }
        }

        private void DoubleCitizens()
        {
            var newCitizens = new ICitizen[citizens.Length + 20];
            for (int i = 0; i < citizens.Length; i++)
            {
                newCitizens[i] = citizens[i];
            }

            counterForAddNewCitizen += 20;
        }

        public string CreateId(ICitizen citizen)
        {
            var id = String.Empty;

            id += new string(Convert.ToString((citizen.BirthDate - new DateTime(1899, 12, 31)).Days + 100000).ToArray(),1,5);

            var count = citizens.Count(cit => cit?.Gender == citizen.Gender) + 1;

            id += new String(Convert.ToString((count - 1 + (int)citizen.Gender)* 2 - (int)citizen.Gender + 10000).Reverse().Take(4).Reverse().ToArray());

            id += Convert.ToString(((-Convert.ToInt32(id[0])
                + Convert.ToInt32(id[1]) * 5
                + Convert.ToInt32(id[2]) * 7
                + Convert.ToInt32(id[3]) * 9
                + Convert.ToInt32(id[4]) * 4
                + Convert.ToInt32(id[5]) * 6
                + Convert.ToInt32(id[6]) * 10
                + Convert.ToInt32(id[7]) * 5
                + Convert.ToInt32(id[8]) * 7)%11)%10);

            return id;
        }

        public string Stats()
        {
            var s = lastRegistrationDateTime.HasValue ? ". Last registration was " + lastRegistrationDateTime.Value.Humanize(dateToCompareAgainst:SystemDateTime.Now()) : "";
            return $"{genderCounts[(int)Gender.Male]} m{(char)(97 + Convert.ToInt32(genderCounts[(int)Gender.Male] != 1) * 4)}n and {genderCounts[(int)Gender.Female]} wom{(char)(97 + Convert.ToInt32(genderCounts[(int)Gender.Female] != 1) * 4)}n" + s;
        }
    }
}

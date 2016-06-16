namespace Citizens
{
    using System;
    using System.Linq;
    using System.Text;
    using Humanizer;

    public class SimpleRegistry : ICitizenRegistry
    {
        private readonly DateTime thenEverythingsBegun = new DateTime(1899, 12, 31);

        private ICitizen[] citizens = new ICitizen[20];

        private int[] genderCounts = new int[2];

        private DateTime? lastRegistrationDateTime;

        public ICitizen this[string id]
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentNullException();
                }

                foreach (Citizen c in this.citizens)
                {
                    if (c != null)
                    {
                        if (c.VatId == id)
                        {
                            return c;
                        }
                    }
                }

                return null;
            }
        }

        public void Register(ICitizen citizen)
        {
            if (this.citizens.Contains(citizen, new Helpers.CitizenEqualsComparer()))
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

            for (int i = 0; i < this.citizens.Length; ++i)
            {
                if (this.citizens[i] == null)
                {
                    this.citizens[i] = newCitizen;
                    this.genderCounts[(int)newCitizen.Gender]++;
                    this.lastRegistrationDateTime = SystemDateTime.Now();
                    break;
                }
            }

            if (this.citizens[this.citizens.Length - 1] != null)
            {
                this.DoubleCitizens();
            }
        }

        public string Stats()
        {
            var s = this.lastRegistrationDateTime.HasValue ? ". Last registration was " + this.lastRegistrationDateTime.Value.Humanize(dateToCompareAgainst: SystemDateTime.Now()) : string.Empty;
            return $"{this.genderCounts[(int)Gender.Male]} m{(char)(97 + (Convert.ToInt32(this.genderCounts[(int)Gender.Male] != 1) * 4))}n and {this.genderCounts[(int)Gender.Female]} wom{(char)(97 + (Convert.ToInt32(this.genderCounts[(int)Gender.Female] != 1) * 4))}n" + s;
        }

        private void DoubleCitizens()
        {
            Array.Resize(ref this.citizens, this.citizens.Length + 20);
        }

        private string CreateId(ICitizen citizen)
        {
            var id = new StringBuilder(10);

            id.Append(string.Format("{0:00000}", (citizen.BirthDate - this.thenEverythingsBegun).Days));

            var count = this.citizens.Where(cit => cit != null).Count(cit => (cit.Gender == citizen.Gender) && (Convert.ToString(cit.VatId.Take(5)) == Convert.ToString(id.ToString().Take(5)))) + 1;

            id.Append(new string(Convert.ToString(((count - 1 + (int)citizen.Gender) * 2) - (int)citizen.Gender + 10000).Reverse().Take(4).Reverse().ToArray()));

            id.Append(Convert.ToString((-Convert.ToInt32(id[0])
                + (Convert.ToInt32(id[1]) * 5)
                + (Convert.ToInt32(id[2]) * 7)
                + (Convert.ToInt32(id[3]) * 9)
                + (Convert.ToInt32(id[4]) * 4)
                + (Convert.ToInt32(id[5]) * 6)
                + (Convert.ToInt32(id[6]) * 10)
                + (Convert.ToInt32(id[7]) * 5)
                + ((Convert.ToInt32(id[8]) * 7) % 11)) % 10));

            return id.ToString();
        }
    }
}

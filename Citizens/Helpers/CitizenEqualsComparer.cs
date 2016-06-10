using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citizens.Helpers
{
    class CitizenEqualsComparer : IEqualityComparer<ICitizen>
    {
        public bool Equals(ICitizen x, ICitizen y)
        {
            if (x == null)
            {
                return false;
            }
            return x.VatId == y.VatId;
        }

        public int GetHashCode(ICitizen obj)
        {
            return obj.VatId.GetHashCode();
        }
    }
}

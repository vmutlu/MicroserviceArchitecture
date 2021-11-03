using DotnetMicroserviceArchitecture.Order.Core.Entities;
using System.Collections.Generic;

namespace DotnetMicroserviceArchitecture.Order.Domain.Aggregate
{
    public class Adress : BaseValueObject
    {
        public string City { get; private set; }
        public string Town { get; private set; }
        public string Street { get; private set; }
        public string ZipCode { get; private set; }
        public string Line { get; private set; }
        public Adress(string city, string town, string street, string zipCode, string line)
        {
            City = city;
            Town = town;
            Street = street;
            ZipCode = zipCode;
            Line = line;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return City;
            yield return Town;
            yield return Street;
            yield return ZipCode;
            yield return Line;
        }
    }
}

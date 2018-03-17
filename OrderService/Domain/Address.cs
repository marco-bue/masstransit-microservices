using System;
namespace OrderService.Domain
{
    public class Address : IEquatable<Address>
    {
        private readonly string street;

        private readonly string zip;

        private readonly string city;

        private readonly string countryIso;

        public Address(string street, string zip, string city, string countryIso)
        {
            // To keep it simple here, arguments are valid by definition.
            this.street = street;
            this.zip = zip;
            this.city = city;
            this.countryIso = countryIso;
        }

        public string Street => street;

        public string Zip => zip;

        public string City => city;

        public string CountryIso => countryIso;

        public bool Equals(Address other)
        {
            return Street == other.Street
                  && Zip == other.Zip
                  && City == other.City
                  && CountryIso == other.CountryIso;
        }

        public string GetLabel()
        {
            return $"{Street} - {Zip} {City} - {CountryIso}";
        }
    }
}

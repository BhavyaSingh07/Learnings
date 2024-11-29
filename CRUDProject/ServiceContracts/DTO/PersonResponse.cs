using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO Class that is used as return type of most methods of Persons Service
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != typeof(Person))
            {
                return false;
            }

            PersonResponse person = (PersonResponse)obj;
            return this.PersonID == person.PersonID && this.PersonName == person.PersonName && this.DateOfBirth == person.DateOfBirth && this.Gender == person.Gender && this.CountryId == person.CountryId && this.Address == person.Address && this.ReceiveNewsLetters == person.ReceiveNewsLetters;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest() { PersonID = PersonID, PersonName = PersonName, Email = Email, DateOfBirth = DateOfBirth, Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true), Address = Address, CountryId = CountryId, ReceiveNewsLetters = ReceiveNewsLetters };
        }
    }

    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse() { PersonID = person.PersonID, PersonName = person.PersonName, Email = person.Email, DateOfBirth = person.DateOfBirth, Address = person.Address, ReceiveNewsLetters = person.ReceiveNewsLetters, CountryId = person.CountryId, Gender = person.Gender, Age = (person.DateOfBirth != null)? Math.Round((DateTime.Today - person.DateOfBirth.Value).TotalDays / 365.25) : null, Country = person.Country?.CountryName};
            }
        
    }

}

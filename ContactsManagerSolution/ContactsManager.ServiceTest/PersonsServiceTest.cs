using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts;
using Xunit;
using Services;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Entities;
//using Microsoft.EntityFrameworkCore;
//using EntityFrameworkCoreMock;
using AutoFixture;
using RepositoryContracts;
using Moq;
using System.Linq.Expressions;
using System.Diagnostics;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly IcountriesService _countriesService;
        private readonly Mock<IPersonsRepository> _personRepositoryMock;
        private readonly IPersonsRepository _personsrepository;
        private readonly Mock<ILogger<PersonsService>> _loggerMock;
        private readonly ILogger<PersonsService> _logger;
        private readonly IFixture _fixture;

        public PersonsServiceTest()
        {
            _loggerMock = new Mock<ILogger<PersonsService>>();
            _logger = _loggerMock.Object;
            _personRepositoryMock = new Mock<IPersonsRepository>();
            _personsrepository = _personRepositoryMock.Object;
            var countriesInitialData = new List<Country>() { };
            var personsInitialData = new List<Person>() { };    
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options
                );
            //var dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            ApplicationDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);
            //_countriesService = new CountriesService(null);

            _personsService = new PersonsService(_personsrepository, _logger);
            _fixture = new Fixture();
            //foreach (var behavior in _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList())
            //{
            //    _fixture.Behaviors.Remove(behavior);
            //}
            //_fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        }

        #region AddPerson

        //when we supply null values as addpersonRequest, should throw nullException
        [Fact]
        
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
        {
            //arrange
            PersonAddRequest? personAddRequest = null;
            //act 
           // _personsService.AddPerson(personAddRequest);
            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                await _personsService.AddPerson(personAddRequest);
            });
        }

        [Fact]
        public async Task AddPerson_PersonNameIsNull_ToBeArgumentException()
        {
            //PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };
            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(t => t.PersonName, null as string).Create();

            Person pesron = personAddRequest.ToPerson();
            _personRepositoryMock.Setup(t => t.AddPerson(It.IsAny<Person>()));
            //_personsService.AddPerson(personAddRequest);
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                 await _personsService.AddPerson(personAddRequest);
            });

        }

        [Fact]
        public async Task AddPerson_FullPersonDetails_ToBeSuccessful() {

            //PersonAddRequest? personAddRequest = new PersonAddRequest() {PersonName = "Person",Email = "person@example.com",DateOfBirth = DateTime.Parse("2002-09-04"),Gender = GenderOptions.Male,CountryId = Guid.NewGuid(),Address = "sample address",ReceiveNewsLetters = true };
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(t => t.Email, "sample@example.com").Create();

            Person person = personAddRequest.ToPerson();
            PersonResponse person_response_expected = person.ToPersonResponse();
            _personRepositoryMock.Setup(t => t.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);



            PersonResponse person_response_from_add = await _personsService.AddPerson(personAddRequest);
            //List<PersonResponse> person_list = await _personsService.GetAllPersons();
            person_response_expected.PersonID = person_response_from_add.PersonID;

            

            Assert.True(person_response_from_add.PersonID != Guid.Empty);
            //Assert.Contains(person_response_from_add, person_list);
            Assert.Equivalent(person_response_from_add, person_response_expected);
        }
        #endregion

        #region getpersonByPersonID

        [Fact]
        //supply null id --- person as null response
        public async Task GetPersonByPersonID_NullPerson_ToBeNull()
        {
            Guid? personId = null;
            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonID(personId);
            Assert.Null(person_response_from_get);
        }

        //valid person id should return valid details
        [Fact]
        public async Task GetPersonByPersonID_WithPersonID_ToBeSuccessful()
        {
            //Arange
            //CountryAddRequest country_request = new CountryAddRequest() { CountryName = "Canada" };


            //CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();
            //CountryResponse country_response = await _countriesService.AddCountry(country_request);

            //PersonAddRequest person_request = new PersonAddRequest() { PersonName = "person name...", Email = "email@sample.com", Address = "address", CountryId = country_response.CountryId, DateOfBirth = DateTime.Parse("2000-01-01"), Gender = GenderOptions.Male, ReceiveNewsLetters = false };
            Person person = _fixture.Build<Person>().With(temp => temp.Email, "sample@example.com").With(temp => temp.Country, null as Country).Create();
            PersonResponse person_response_expected = person.ToPersonResponse();

            //PersonResponse person_response_from_add = await _personsService.AddPerson(person);
            
            _personRepositoryMock.Setup(t => t.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);
            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonID(person.PersonID);

            //Assert
            Assert.Equivalent(person_response_expected, person_response_from_get);
        }

        #endregion


        #region getallpersons

        [Fact]
        public async Task GetAllPersons_ToBeEmptyList()
        {
            var person = new List<Person>();
            _personRepositoryMock.Setup(t => t.GetAllPersons()).ReturnsAsync(person);
            List<PersonResponse> persons_from_get = await _personsService.GetAllPersons();

            Assert.Empty(persons_from_get);
        }


        [Fact]
        public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
        {
            //Arrange
            //CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
           // CountryAddRequest country_request_1 = _fixture.Create<CountryAddRequest>();
           // CountryAddRequest country_request_2 = _fixture.Create<CountryAddRequest>();
           //// CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

           // CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
           // CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);
           
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>().With(t => t.Email, "sample1@example.com").With(t => t.Country, null as Country).Create(),
                _fixture.Build<Person>().With(t => t.Email, "sample2@example.com").With(t => t.Country, null as Country).Create(),
                _fixture.Build<Person>().With(t => t.Email, "sample3@example.com").With(t => t.Country, null as Country).Create()
            };

            //PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryId = country_response_1.CountryId, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

            //PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

            //PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

            //List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            //List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();
            List<PersonResponse> person_response_list_expected = persons.Select(t => t.ToPersonResponse()).ToList();

            //foreach (PersonAddRequest person_request in person_requests)
            //{
            //    PersonResponse person_response = await _personsService.AddPerson(person_request);
            //    person_response_list_from_add.Add(person_response);
            //}

            //Act
            _personRepositoryMock.Setup(t => t.GetAllPersons()).ReturnsAsync(persons);
            List<PersonResponse> persons_list_from_get =await _personsService.GetAllPersons();

            //Assert
            Assert.Equivalent(persons_list_from_get, person_response_list_expected);
            //foreach (PersonResponse person_response_from_add in person_response_list_expected)
            //{
            //    Assert.Contains(person_response_from_add, persons_list_from_get);
            //}
        }
        #endregion

        #region getfilteredpersons
        [Fact]
        public async Task GetFilteredPersons_AllExistingPersons()
        {
            //Arrange
            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>().With(t => t.Email, "sample1@example.com").With(t => t.Country, null as Country).Create(),
                _fixture.Build<Person>().With(t => t.Email, "sample2@example.com").With(t => t.Country, null as Country).Create(),
                _fixture.Build<Person>().With(t => t.Email, "sample3@example.com").With(t => t.Country, null as Country).Create()
                //
            };

            List<PersonResponse> person_response_list_expected = persons.Select(t => t.ToPersonResponse()).ToList();

            //List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();


            //Act
            _personRepositoryMock.Setup(t => t.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()));
            List<PersonResponse> persons_list_from_search =await _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

            //Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    Assert.Contains(person_response_from_add, persons_list_from_search);
            //}
            Assert.Equivalent(persons_list_from_search, person_response_list_expected);
        }


        [Fact]
        public async Task GetFilteredPersons_SearchByPersonName()
        {
            ////Arrange
            //CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            //CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            //CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            //CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            //PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryId = country_response_1.CountryId, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

            //PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

            //PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

            //List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            //List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            //foreach (PersonAddRequest person_request in person_requests)
            //{
            //    PersonResponse person_response = await _personsService.AddPerson(person_request);
            //    person_response_list_from_add.Add(person_response);
            //}

            ////Act
            //List<PersonResponse> persons_list_from_search =await _personsService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            ////Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    if(person_response_from_add.PersonName!=null){
            //        if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
            //        { 
            //            Assert.Contains(person_response_from_add, persons_list_from_search); 
            //        }
            //    }
            //}

            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>().With(t => t.Email, "sample1@example.com").With(t => t.Country, null as Country).Create(),
                _fixture.Build<Person>().With(t => t.Email, "sample2@example.com").With(t => t.Country, null as Country).Create(),
                _fixture.Build<Person>().With(t => t.Email, "sample3@example.com").With(t => t.Country, null as Country).Create()
            };

            List<PersonResponse> person_response_list_expected = persons.Select(t => t.ToPersonResponse()).ToList();

            //List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();


            //Act
            _personRepositoryMock.Setup(t => t.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()));
            List<PersonResponse> persons_list_from_search = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "sa");

            //Assert
            //foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            //{
            //    Assert.Contains(person_response_from_add, persons_list_from_search);
            //}
            Assert.Equivalent(persons_list_from_search, person_response_list_expected);




        }


        #endregion

        #region GetSortedPersons

        [Fact]
        public async Task GetSortedPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest() { PersonName = "Smith", Email = "smith@example.com", Gender = GenderOptions.Male, Address = "address of smith", CountryId = country_response_1.CountryId, DateOfBirth = DateTime.Parse("2002-05-06"), ReceiveNewsLetters = true };

            PersonAddRequest person_request_2 = new PersonAddRequest() { PersonName = "Mary", Email = "mary@example.com", Gender = GenderOptions.Female, Address = "address of mary", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("2000-02-02"), ReceiveNewsLetters = false };

            PersonAddRequest person_request_3 = new PersonAddRequest() { PersonName = "Rahman", Email = "rahman@example.com", Gender = GenderOptions.Male, Address = "address of rahman", CountryId = country_response_2.CountryId, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            
            List<PersonResponse> allPersons =await _personsService.GetAllPersons();

            //Act
            List<PersonResponse> persons_list_from_sort = await _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();

            //Assert
            for (int i = 0; i < person_response_list_from_add.Count; i++)
            {
                Assert.Equivalent(person_response_list_from_add[i], persons_list_from_sort[i]);
            }
        }


        #endregion


        #region UpdatePerson

        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                //Act
                await _personsService.UpdatePerson(person_update_request);
            });
        }


        //When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = new PersonUpdateRequest() { PersonID = Guid.NewGuid() };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => {
                //Act
                await _personsService.UpdatePerson(person_update_request);
            });
        }


        //When PersonName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            //CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
            //CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

            //PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryId = country_response_from_add.CountryId, Email="ss@example.com", Gender = GenderOptions.Male };
            //PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            //PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            //person_update_request.PersonName = null;


            ////Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () => {
            //    //Act
            //   await _personsService.UpdatePerson(person_update_request);
            //});

            Person person = _fixture.Build<Person>().With(t => t.PersonName, null as string).With(t => t.Email, "samp@ex.com").With(t => t.Country, null as Country).Create();
            PersonResponse person_response_from_add = person.ToPersonResponse();
            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personsService.UpdatePerson(person_update_request);
            });

        }


        //First, add a new person and try to update the person name and email
        [Fact]
        //[Theory]
        //[InlineData(null)]
        public async Task UpdatePerson_PersonFullDetailsUpdation()
        {
            //Arrange
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryId = country_response_from_add.CountryId, Address = "Abc road", DateOfBirth = DateTime.Parse("2000-01-01"), Email = "abc@example.com", Gender = GenderOptions.Male, ReceiveNewsLetters = true };

            PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "William";
            person_update_request.Email = "william@example.com";

            //Act
            PersonResponse person_response_from_update = await _personsService.UpdatePerson(person_update_request);

            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonID(person_response_from_update.PersonID);

            //Assert
            Assert.Equivalent(person_response_from_get, person_response_from_update);

        }


        #endregion


        #region DeletePerson

        [Fact]
        public async Task DeletePerson_ValidPersonID()
        {
            //Arrange
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "USA" };
            CountryResponse country_response_from_add =await _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "Jones", Address = "address", CountryId = country_response_from_add.CountryId, DateOfBirth = Convert.ToDateTime("2010-01-01"), Email = "jones@example.com", Gender = GenderOptions.Male, ReceiveNewsLetters = true };

            PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);


            //Act
            bool isDeleted = await _personsService.DeletePerson(person_response_from_add.PersonID);

            //Assert
            Assert.True(isDeleted);
        }


        //If you supply an invalid PersonID, it should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {
            //Act
            bool isDeleted = await _personsService.DeletePerson(Guid.NewGuid());

            //Assert
            Assert.False(isDeleted);
        }


        #endregion
    }
}

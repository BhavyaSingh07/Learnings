using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using ServiceContracts;
using CRUDProject.Controllers;
using ServiceContracts.DTO;
using Entities;
using ServiceContracts.Enums;
using Microsoft.AspNetCore.Mvc;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace CRUDTests
{
    public class PersonsControllerTest
    {
        private readonly IPersonsService _personsService;
        private readonly IcountriesService _countriesService;
        private readonly Mock<IcountriesService> _countriesServiceMock;
        private readonly Mock<IPersonsService> _personsServiceMock;
        private readonly ILogger<PersonsController> _logger;
        private readonly Mock<ILogger<PersonsController>> _loggerMock;
        private readonly Fixture _fixture;
        public PersonsControllerTest()
        {
            _fixture = new Fixture();
            _loggerMock = new Mock<ILogger<PersonsController>>();
            _logger = _loggerMock.Object;
            _countriesServiceMock = new Mock<IcountriesService>();
            _personsServiceMock = new Mock<IPersonsService>();
            _personsService = _personsServiceMock.Object;
            _countriesService = _countriesServiceMock.Object;
        }

        #region Index

        [Fact]
        //[Theory]
        //[InlineData(null)]

        public async Task Index_ShouldReturnvalidIndexView()
        {

            #region arrange
            List<PersonResponse> person_response_list = _fixture.Create<List<PersonResponse>>();
            PersonsController personsController = new PersonsController(_personsService, _countriesService, _logger);

            _personsServiceMock.Setup(t => t.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(person_response_list);
            _personsServiceMock.Setup(t => t.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>())).ReturnsAsync(person_response_list);
            #endregion

            #region act
            IActionResult result = await personsController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderOptions>());
            #endregion

            #region assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<IEnumerable<PersonResponse>>(viewResult.ViewData.Model);
            #endregion
        }

        #endregion
    }
}

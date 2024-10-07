using Microsoft.EntityFrameworkCore;
using Moq;
using SparePartsModule.Core.Exceptions;
using SparePartsModule.Core.Library;
using SparePartsModule.Domain.Context;
using SparePartsModule.Domain.Models.Library;
using SparePartsModule.Infrastructure.ViewModels.Models.Library.LibraryTariff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SparePartsModule.Test.Library
{
    public class LibraryTariffServiceTests
    {
        private readonly Mock<SparePartsModuleContext> _contextMock;
        private readonly LibraryTariffService _tariffService;

        public LibraryTariffServiceTests()
        {
            _contextMock = new Mock<SparePartsModuleContext>();
            _tariffService = new LibraryTariffService(_contextMock.Object);
        }

        [Fact]
        public async Task AddLibraryTariff_WithValidModel_ShouldAddTariffAndReturnSuccessResponse()
        {
            // Arrange
            var model = new AddLibraryTariffModel
            {
                TariffCode = "TAR001",
                TariffPer = 10,
                Status = 1
            };
            var userId = 1;

             _contextMock.Setup(c => c.MasterSPGeneralTariff.AddAsync(It.IsAny<MasterSPGeneralTariff>(), default))
                .Returns( null);
            _contextMock.Setup(c => c.SaveChangesAsync(default))
                .ReturnsAsync(1);

            // Act
            var response = await _tariffService.AddLibraryTariff(model, userId);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(200, response.StatusCode);
            Assert.Null(response.Data);

            _contextMock.Verify(c => c.MasterSPGeneralTariff.AddAsync(It.IsAny<MasterSPGeneralTariff>(), default), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task AddLibraryTariff_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange
            AddLibraryTariffModel model = null;
            var userId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>await _tariffService.AddLibraryTariff(model, userId));
        }

        [Fact]
        public async Task AddLibraryTariff_WithExistingTariffCode_ShouldThrowManagerProcessException()
        {
            // Arrange
            var model = new AddLibraryTariffModel
            {
                TariffCode = "TAR001",
                TariffPer = 10,
                Status = 1
            };
            var userId = 1;

            _contextMock.Setup(c => c.MasterSPGeneralTariff.FirstOrDefault(It.IsAny<Func<MasterSPGeneralTariff, bool>>()))
                .Returns(new MasterSPGeneralTariff());

            // Act & Assert
            await Assert.ThrowsAsync<ManagerProcessException>(async () =>await _tariffService.AddLibraryTariff(model, userId));
        }

        // Add more test cases for other methods...

    }
}

using System;

namespace Brimma.LOSServiceTest
{
    public class LoansControllerTest
    {
        //[Fact]
        //public void GetLoanSummary_ReturnsValidResult()
        //{
        //    // Arrange
        //    var loansService = new Mock<ILoansService>();
        //    loansService.Setup(service => service.GetSummary(It.IsAny<string>())).ReturnsAsync(GetLoanSummary());
        //    LoansController loansController = new LoansController(loansService.Object);

        //    // Act
        //    dynamic result = loansController.GetLoanSummary(Guid.NewGuid().ToString());

        //    // Assert
        //    Assert.NotNull(result);
        //}

        //[Fact]
        //public void GetLoanSummary_ReturnsBadResult()
        //{
        //    // Arrange
        //    var loansService = new Mock<ILoansService>();
        //    loansService.Setup(service => service.GetSummary(It.IsAny<string>())).ReturnsAsync(GetLoanSummary());
        //    LoansController loansController = new LoansController(loansService.Object);

        //    // Act
        //    Object response = loansController.GetLoanSummary("loanguid");

        //    // Assert
        //    Assert.IsType<BadRequestResult>(response);
        //}

        //[Fact]
        //public void GetLoanSummary_ReturnsNotFoundResult()
        //{
        //    // Arrange
        //    var loansService = new Mock<ILoansService>();
        //    loansService.Setup(service => service.GetSummary(It.IsAny<string>())).ReturnsAsync(new StatusCodeResult(404));
        //    LoansController loansController = new LoansController(loansService.Object);

        //    // Act
        //    var result = loansController.GetLoanSummary(Guid.NewGuid().ToString()) as StatusCodeResult;

        //    // Assert
        //    Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        //}

        //[Fact]
        //public void GetPropertyInfo_ReturnsValidResult()
        //{
        //    // Arrange
        //    var loansService = new Mock<ILoansService>();
        //    loansService.Setup(service => service.GetPropertyInfo(It.IsAny<string>())).ReturnsAsync(GetPropertyInfo());
        //    LoansController loansController = new LoansController(loansService.Object);

        //    // Act
        //    dynamic result = loansController.GetPropertyInfo(Guid.NewGuid().ToString());

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.NotNull(result.propertyInfo);
        //}

        //[Fact]
        //public void GetPropertyInfo_ReturnsBadResult()
        //{
        //    // Arrange
        //    var loansService = new Mock<ILoansService>();
        //    loansService.Setup(service => service.GetPropertyInfo(It.IsAny<string>())).ReturnsAsync(GetPropertyInfo());
        //    LoansController loansController = new LoansController(loansService.Object);

        //    // Act
        //    var result = loansController.GetPropertyInfo("loanGuid");

        //    // Assert
        //    Assert.IsType<BadRequestResult>(result);
        //}

        //[Fact]
        //public void GetPropertyInfo_ReturnsNotFoundResult()
        //{
        //    // Arrange
        //    var loansService = new Mock<ILoansService>();
        //    loansService.Setup(service => service.GetPropertyInfo(It.IsAny<string>())).ReturnsAsync(new StatusCodeResult(404));
        //    LoansController loansController = new LoansController(loansService.Object);

        //    // Act
        //    var result = loansController.GetPropertyInfo(Guid.NewGuid().ToString()) as StatusCodeResult;

        //    // Assert
        //    Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        //}

        //[Fact]
        //public void GetLoanInfo_ReturnsValidResult()
        //{
        //    // Arrange
        //    var loansService = new Mock<ILoansService>();
        //    loansService.Setup(service => service.GetLoanInfo(It.IsAny<string>())).ReturnsAsync(GetLoanInfo());
        //    LoansController loansController = new LoansController(loansService.Object);

        //    // Act
        //    dynamic result = loansController.GetLoanInfo(Guid.NewGuid().ToString());

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.NotNull(result.loanInfo);
        //}

        //[Fact]
        //public void GetLoanInfo_ReturnsBadResult()
        //{
        //    // Arrange
        //    var loansService = new Mock<ILoansService>();
        //    loansService.Setup(service => service.GetLoanInfo(It.IsAny<string>())).ReturnsAsync(GetLoanInfo());
        //    LoansController loansController = new LoansController(loansService.Object);

        //    // Act
        //    var result = loansController.GetLoanInfo("loanGuid");

        //    // Assert
        //    Assert.IsType<BadRequestResult>(result);
        //}

        //[Fact]
        //public void GetLoanInfo_ReturnsNotFoundResult()
        //{
        //    // Arrange
        //    var loansService = new Mock<ILoansService>();
        //    loansService.Setup(service => service.GetLoanInfo(It.IsAny<string>())).ReturnsAsync(new StatusCodeResult(404));
        //    LoansController loansController = new LoansController(loansService.Object);

        //    // Act
        //    var result = loansController.GetLoanInfo(Guid.NewGuid().ToString()) as StatusCodeResult;

        //    // Assert
        //    Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        //}

        //[Fact]
        //public void GetLoanAndPropertyInfo_ReturnsValidResult()
        //{
        //    // Arrange
        //    var loansService = new Mock<ILoansService>();
        //    loansService.Setup(service => service.GetLoanAndPropertyInfo(It.IsAny<string>())).ReturnsAsync(GetLoanAndPropertyInfo());
        //    LoansController loansController = new LoansController(loansService.Object);

        //    // Act
        //    dynamic result = loansController.GetLoanAndPropertyInfo(Guid.NewGuid().ToString());

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.NotNull(result.propertyInfo);
        //    Assert.NotNull(result.loanInfo);
        //}

        //[Fact]
        //public void GetLoanAndPropertyInfo_ReturnsBadResult()
        //{
        //    // Arrange
        //    var loansService = new Mock<ILoansService>();
        //    loansService.Setup(service => service.GetLoanAndPropertyInfo(It.IsAny<string>())).ReturnsAsync(GetLoanAndPropertyInfo());
        //    LoansController loansController = new LoansController(loansService.Object);

        //    // Act
        //    var result = loansController.GetLoanAndPropertyInfo("loanGuid");

        //    // Assert
        //    Assert.IsType<BadRequestResult>(result);
        //}

        //[Fact]
        //public void GetLoanAndPropertyInfo_ReturnsNotFoundResult()
        //{
        //    // Arrange
        //    var loansService = new Mock<ILoansService>();
        //    loansService.Setup(service => service.GetLoanAndPropertyInfo(It.IsAny<string>())).ReturnsAsync(new StatusCodeResult(404));
        //    LoansController loansController = new LoansController(loansService.Object);

        //    // Act
        //    var result = loansController.GetLoanAndPropertyInfo(Guid.NewGuid().ToString()) as StatusCodeResult;

        //    // Assert
        //    Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        //}

        private Object GetLoanSummary()
        {
            var response = new
            {
                borrower = new
                {
                    firstName = "Mike",
                    lastName = "Abramov"
                },
                coBorrower = new
                {
                    firstName = "",
                    lastName = ""
                },
                propertyAddress = new
                {
                    street = "3446, Carrick Hill CT",
                    city = "Wake Forest",
                    state = "NC",
                    zip = "27587"
                },
                loanAmount = 1000000.0,
                rateLockStatus = "Expired",
                estClosingDate = "2019-08-08T00=00=00Z",
                loanPurpose = "Purchase",
                ltv = 91.0,
                cltv = 91.0,
                hcltv = 91.0,
                dti = new
                {
                    topRatioPercent = 14.467,
                    bottomRatioPercent = 14.467
                },
                noteRate = 4.0
            };
            return response;
        }

        private Object GetPropertyInfo()
        {
            var response = new
            {
                borrower = new
                {
                    firstName = "Mike",
                    lastName = "Abramov"
                },
                coBorrower = new
                {
                    firstName = "",
                    lastName = ""
                },
                propertyInfo = new
                {
                    street = "3446, Carrick Hill CT",
                    city = "Wake Forest",
                    state = "NC",
                    zip = "27587",
                    county = "Gaston",
                    numberOfUnits = 1,
                    unitNumber = "",
                    propertyValue = 1100000.0,
                    occupancy = "PrimaryResidence",
                    mixedUseProperty = "",
                    manufacturingHome = ""
                }
            };
            return response;
        }

        private Object GetLoanInfo()
        {
            var response = new
            {
                borrower = new
                {
                    firstName = "Mike",
                    lastName = "Abramov"
                },
                coBorrower = new
                {
                    firstName = "",
                    lastName = ""
                },
                loanInfo = new
                {
                    loanAmount = 1000000.0,
                    loanPurpose = "Purchase"
                }
            };
            return response;
        }

        private Object GetLoanAndPropertyInfo()
        {
            var response = new
            {
                borrower = new
                {
                    firstName = "Mike",
                    lastName = "Abramov"
                },
                coBorrower = new
                {
                    firstName = "",
                    lastName = "",
                },
                propertyInfo = new
                {
                    street = "3446, Carrick Hill CT",
                    city = "Wake Forest",
                    state = "NC",
                    zip = "27587",
                    county = "Gaston",
                    numberOfUnits = 1,
                    unitNumber = "",
                    propertyValue = 1100000.0,
                    occupancy = "PrimaryResidence",
                    mixedUseProperty = "",
                    manufacturingHome = ""
                },
                loanInfo = new
                {
                    loanAmount = 1000000.0,
                    loanPurpose = "Purchase"
                }
            };
            return response;
        }
    }
}

using Moq;
using RoadStatusService;
using System.Net;
using Xunit;

namespace RoadStatusTest
{
    public class RoadStatusTests
    {
        private readonly string _mockAppId = "mockAppId";
        private readonly string _mockAppKey = "mockAppKey";

        [Fact]
        public async Task GetRoadStatusAsync_Returns_RoadStatus_When_Response_Is_Ok()
        {
            // Arrange
            var mockHttpClient = new Mock<ITflClient>();
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[{\"$type\":\"Tfl.Api.Presentation.Entities.RoadCorridor, Tfl.Api.Presentation.Entities\",\"id\":\"a2\",\"displayName\":\"A2\",\"statusSeverity\":\"Good\",\"statusSeverityDescription\":\"No Exceptional Delays\",\"bounds\":\"[[-0.0857,51.44091],[0.17118,51.49438]]\",\"envelope\":\"[[-0.0857,51.44091],[-0.0857,51.49438],[0.17118,51.49438],[0.17118,51.44091],[-0.0857,51.44091]]\",\"url\":\"/Road/a2\"}]" )
            };
            mockHttpClient.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(mockResponse);
            var service = new RoadStatusService.RoadStatusService(mockHttpClient.Object, _mockAppId, _mockAppKey);

            // Act
            var (errorCode, roadStatus) = await service.GetRoadStatusAsync("A2");

            // Assert
            Assert.Equal(0, errorCode);
            Assert.NotNull(roadStatus);
            Assert.Equal("A2", roadStatus.DisplayName);
            Assert.Equal("Good", roadStatus.StatusSeverity);
            Assert.Equal("No Exceptional Delays", roadStatus.StatusSeverityDescription);
        }

        [Fact]
        public async Task GetRoadStatusAsync_Returns_Null_RoadStatus_When_Response_Is_NotFound()
        {
            // Arrange
            var mockHttpClient = new Mock<ITflClient>();
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };
            mockHttpClient.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(mockResponse);
            var service = new RoadStatusService.RoadStatusService(mockHttpClient.Object, _mockAppId, _mockAppKey);

            // Act
            var (errorCode, roadStatus) = await service.GetRoadStatusAsync("InvalidRoadId");

            // Assert
            Assert.Equal(1, errorCode);
            Assert.Null(roadStatus);
        }

        [Fact]
        public async Task GetRoadStatusAsync_Throws_ApplicationException_When_Response_Is_Unsuccessful()
        {
            // Arrange
            var mockHttpClient = new Mock<ITflClient>();
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };
            mockHttpClient.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(mockResponse);
            var service = new RoadStatusService.RoadStatusService(mockHttpClient.Object, _mockAppId, _mockAppKey);

            // Act + Assert
            await Assert.ThrowsAsync<ApplicationException>(() => service.GetRoadStatusAsync("A2"));
        }
    }
}
using Microsoft.Extensions.Logging;
using Moq;

namespace SGP.Tests.Mocks
{
    public static class LoggerFactoryMock
    {
        public static ILoggerFactory Create()
        {
            var loggerFactoryMock = new Mock<ILoggerFactory>();

            loggerFactoryMock
                .Setup(s => s.CreateLogger(It.IsAny<string>()))
                .Returns(Mock.Of<ILogger>());

            return loggerFactoryMock.Object;
        }
    }
}

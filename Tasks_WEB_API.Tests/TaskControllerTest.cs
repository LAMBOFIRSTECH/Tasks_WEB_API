using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Tasks_WEB_API.Interfaces;

namespace Tasks_WEB_API.Tests
{
    public class TaskControllerTest
    {
        Mock<IReadUsersMethods>  mockReadMethods = new Mock<IReadUsersMethods>();
		Mock<IWriteUsersMethods>  mockWriteMethods1 = new Mock<IWriteUsersMethods>();
		Mock<IWriteUsersMethods>  mockWriteMethods2 = new Mock<IWriteUsersMethods>();
    }
}
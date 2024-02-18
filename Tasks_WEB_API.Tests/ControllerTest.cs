using Microsoft.EntityFrameworkCore;
using Xunit;
using Tasks_WEB_API.Controllers;
using Tasks_WEB_API.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tasks_WEB_API.Tests
{
	public class ControllerTest
	{
		// [Fact]
		// public async Task Get_ReturnsNotFound_ForEmptyUsersData()
		// {

		// 	// Arrange
		// 	var contextMock = new Mock<DailyTasksMigrationsContext>();
		// 	contextMock.Setup<DbSet<Utilisateur>>(x => x.Utilisateurs).Returns(TestDataHelper.GetFakeEmployeeList());

		// 	//Act
		// 	UserManagementController controller = new(contextMock.Object);
		// 	var utilisateur = await controller.Get();

		// 	//Assert
		// 	Assert.NotNull(utilisateur);
		// }
	}
}


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tasks_WEB_API.Controllers;
using Tasks_WEB_API.Interfaces;
using Xunit;

namespace Tasks_WEB_API.Tests
{
	public class UserControllerTest
	{
		const int userID = 1;
		Mock<IReadUsersMethods>  mockReadMethods = new Mock<IReadUsersMethods>();
		Mock<IWriteUsersMethods>  mockWriteMethods1 = new Mock<IWriteUsersMethods>();
		Mock<IWriteUsersMethods>  mockWriteMethods2 = new Mock<IWriteUsersMethods>();
		
		[Fact]
		public async Task GetUsersReturns_OkResult_1()
		{
			// Arrange
			var expectedUserList = new List<Utilisateur>();
			mockReadMethods.Setup(m => m.GetUsers()).ReturnsAsync(expectedUserList);
			var controller = new UsersManagementController(mockReadMethods.Object, null!);

			// Act
			var result = await controller.GetUsers();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var actualUserList = Assert.IsAssignableFrom<IEnumerable<Utilisateur>>(okResult.Value);
			Assert.Equal(expectedUserList, actualUserList);
		}

		[Fact]
		public async Task GetUsersByIdReturns_NotFound_or_OkResult_2()
		{
			// Arrange
			mockReadMethods.SetupSequence(m => m.GetUserById(userID))
			.ReturnsAsync(new Utilisateur() { ID = userID, Nom = "nom", Pass = "password", Role = Utilisateur.Privilege.UserX })
			.ReturnsAsync((Utilisateur)null!);

			var controller = new UsersManagementController(mockReadMethods.Object, null!);

			// Act
			var result1 = await controller.GetUserById(1);
			var result2 = await controller.GetUserById(2);
			
			//Assert
			var okResult = Assert.IsType<OkObjectResult>(result1);
			Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

			var notResult = Assert.IsType<NotFoundObjectResult>(result2);
			Assert.Equal(StatusCodes.Status404NotFound, notResult.StatusCode);
		}

		[Fact]
		public async Task CreateUserReturns_BadRequest_or_Conflict_or_CorrectsData_3()
		{
			// Arrange
			var user = new Utilisateur()
			{
				ID = 1,
				Nom = "nom",
				Pass = "pass",
				Role = Utilisateur.Privilege.Admin
			};

			var users = new List<Utilisateur> { user };
			mockReadMethods.Setup(m => m.GetUsers()).ReturnsAsync(users);
			mockWriteMethods1.Setup(m => m.CreateUser(It.IsAny<Utilisateur>())).ThrowsAsync(new Exception());
			var controller1 = new UsersManagementController(mockReadMethods.Object, mockWriteMethods1.Object);

			mockWriteMethods2.Setup(m => m.CreateUser(It.IsAny<Utilisateur>())).ReturnsAsync((Utilisateur)null!);
			var controller2 = new UsersManagementController(mockReadMethods.Object, mockWriteMethods2.Object);

			// Act
			var result1 = await controller1.CreateUser(2, "nom", "password", "Excepted Admin/UserX");
			var result2 = await controller2.CreateUser(3, "nom", "password", "Admin");
			var result3 = await controller2.CreateUser(4, "nom", "password", "UserX");

			//Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result1);
			Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
			var conflictResult = Assert.IsType<ConflictObjectResult>(result2);
			Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
			var okResult = Assert.IsType<OkObjectResult>(result3);
			Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
		}

		[Fact]
		public async Task DeleteUserReturns_NotFound_or_CorrectDeleting_4()
		{
			//Arrange
			mockReadMethods.SetupSequence(m => m.GetUserById(userID))
			.ReturnsAsync(new Utilisateur() { ID = userID })
			.ReturnsAsync((Utilisateur)null!);
			
			var controller = new UsersManagementController(mockReadMethods.Object, mockWriteMethods1.Object);

			//Act
			var result1 = await controller.DeleteUserById(userID);
			var result2 = await controller.DeleteUserById(userID);

			//Assert
			var notFound = Assert.IsType<NotFoundObjectResult>(result2);
			Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);

			var okResult = Assert.IsType<OkObjectResult>(result1);
			Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

		}

		[Fact]
		public async Task UpdateUserReturns_NotFound_or_OkUpdating_5()
		{
			//Arrange
			var user1 = new Utilisateur() { ID = 1, Nom = "nom", Pass = "password", Role = Utilisateur.Privilege.UserX };
			var user2 = new Utilisateur() { ID = 2, Nom = "nom", Pass = "password", Role = Utilisateur.Privilege.Admin };

			mockReadMethods.SetupSequence(m => m.GetUserById(userID))
			.ReturnsAsync(new Utilisateur() { ID = userID })
			.ReturnsAsync((Utilisateur)null!);

			var controller = new UsersManagementController(mockReadMethods.Object, mockWriteMethods1.Object);

			//Act
			var result1 = await controller.UpdateUser(user1);
			var result2 = await controller.UpdateUser(user2);

			//Assert
			var okResult = Assert.IsType<OkObjectResult>(result1);
			Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

			var notFound = Assert.IsType<NotFoundObjectResult>(result2);
			Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);

		}
	}
}



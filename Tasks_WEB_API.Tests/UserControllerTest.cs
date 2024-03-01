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



		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		// [Fact]
		// public async Task GetUsers_Returns_OkResult_WithCorrectData()
		// {
		// 	// Arrange
		// 	var expectedUsers = new List<Utilisateur>()
		// 	{
		// 		// Ajoutez des utilisateurs fictifs si nécessaire
		// 		new Utilisateur(){ID=1,Nom="lambo",Role=new Utilisateur.Privilege(),Pass="toto"}

		// 	};

		// 	var mockReadMethods = new Mock<IReadUsersMethods>();
		// 	mockReadMethods.Setup(m => m.GetUsers()).ReturnsAsync(expectedUsers);

		// 	var controller = new UsersManagementController(mockReadMethods.Object, null);

		// 	// Act
		// 	var result = await controller.GetUsers();

		// 	// Assert
		// 	var okResult = Assert.IsType<OkObjectResult>(result);
		// 	var actualUsers = Assert.IsAssignableFrom<IEnumerable<Utilisateur>>(okResult.Value);
		// 	Assert.Equal(expectedUsers, actualUsers);
		// }
		// [Fact]
		// public async Task GetUsersById_Returns_OkResult()
		// {
		// 	// Arrange
		// 	var userId = 1;
		// 	var expectedUser = new Utilisateur() { ID = userId, Nom = "lambo", Role = new Utilisateur.Privilege(), Pass = "toto" };
		// 	var mockReadMethods = new Mock<IReadUsersMethods>();
		// 	mockReadMethods.Setup(m => m.GetUserById(expectedUser.ID)).ReturnsAsync(expectedUser);
		// 	var controller = new UsersManagementController(mockReadMethods.Object, null);

		// 	// Act
		// 	var result = await controller.GetUserById(expectedUser.ID);
		// 	//Assert
		// 	var okResult = Assert.IsType<OkObjectResult>(result);
		// 	var actualUser = Assert.IsAssignableFrom<Utilisateur>(okResult.Value);
		// 	Assert.Equal(expectedUser, actualUser);
		// }
		// [Fact]
		// public async Task GetUsersById_Returns_NotFound()
		// {
		// 	// Arrange
		// 	var userId = 1;
		// 	var mockReadMethods = new Mock<IReadUsersMethods>();
		// 	mockReadMethods.Setup(m => m.GetUserById(userId)).ReturnsAsync((Utilisateur)null);
		// 	var controller = new UsersManagementController(mockReadMethods.Object, null);
		// 	// Act
		// 	var result = await controller.GetUserById(userId);
		// 	//Assert
		// 	var okResult = Assert.IsType<NotFoundObjectResult>(result);
		// }
		// public async Task GetUsersById_Returns_StatusCode()
		// {
		// 	// Arrange
		// 	var userId = 2;
		// 	var mockReadMethods = new Mock<IReadUsersMethods>();
		// 	mockReadMethods.Setup(m => m.GetUserById(userId)).ThrowsAsync(new Exception("Simulated error"));
		// 	var controller = new UsersManagementController(mockReadMethods.Object, null);

		// 	// Act
		// 	var result = await controller.GetUserById(userId);
		// 	//Assert
		// 	var okResult = Assert.IsType<StatusCodeResult>(result);
		// 	Assert.Equal(500, okResult.StatusCode);

		// }

		[Fact]
		public async Task CreateUser_Returns_BadRequest_or_Returns_ConflictForIdenticUsername_Privilege()
		{
			
			var user = new Utilisateur()
			{
				ID = 1,
				Nom = "nom",
				Pass = "password",
				Role = Utilisateur.Privilege.Admin
			};

			var users = new List<Utilisateur> { user };
			var mockReadMethods = new Mock<IReadUsersMethods>();
			mockReadMethods.Setup(m => m.GetUsers()).ReturnsAsync(users);

			var mockWriteMethods1 = new Mock<IWriteUsersMethods>();
			mockWriteMethods1.Setup(m => m.CreateUser(It.IsAny<Utilisateur>())).ThrowsAsync(new Exception());
			var controller1 = new UsersManagementController(mockReadMethods.Object, mockWriteMethods1.Object);

			var mockWriteMethods2 = new Mock<IWriteUsersMethods>();
			mockWriteMethods2.Setup(m => m.CreateUser(It.IsAny<Utilisateur>())).ReturnsAsync((Utilisateur)null!);
			var controller2 = new UsersManagementController(mockReadMethods.Object, mockWriteMethods2.Object);

			var mockWriteMethods3 = new Mock<IWriteUsersMethods>();
			mockWriteMethods3.Setup(m => m.CreateUser(It.IsAny<Utilisateur>())).ReturnsAsync((Utilisateur)null!);
			var controller3 = new UsersManagementController(mockReadMethods.Object, mockWriteMethods3.Object);
			// Act
			var result1 = await controller1.CreateUser(2, "nom", "password", "Excepted Admin/UserX");
			var result2 = await controller2.CreateUser(3, "nom", "password", "Admin");
			var result3 = await controller3.CreateUser(4, "nom", "password", "UserX");

			//Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result1);
			Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
			var conflictResult = Assert.IsType<ConflictObjectResult>(result2);
			Assert.Equal(StatusCodes.Status409Conflict, conflictResult.StatusCode);
			var okResult = Assert.IsType<OkObjectResult>(result3);
			Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
			

		}
	}


}



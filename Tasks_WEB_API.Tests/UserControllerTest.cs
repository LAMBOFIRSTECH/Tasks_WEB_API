using Microsoft.AspNetCore.Mvc;
using Moq;
using SQLitePCL;
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
		[Fact]
		public async Task GetUsers_Returns_OkResult_WithCorrectData()
		{
			// Arrange
			var expectedUsers = new List<Utilisateur>()
			{
				// Ajoutez des utilisateurs fictifs si nécessaire
				new Utilisateur(){ID=1,Nom="lambo",Role=new Utilisateur.Privilege(),Pass="toto"}

			};

			var mockReadMethods = new Mock<IReadUsersMethods>();
			mockReadMethods.Setup(m => m.GetUsers()).ReturnsAsync(expectedUsers);

			var controller = new UsersManagementController(mockReadMethods.Object, null);

			// Act
			var result = await controller.GetUsers();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var actualUsers = Assert.IsAssignableFrom<IEnumerable<Utilisateur>>(okResult.Value);
			Assert.Equal(expectedUsers, actualUsers);
		}
		[Fact]
		public async Task GetUsersById_Returns_OkResult()
		{
			// Arrange
			var userId = 1;
			var expectedUser = new Utilisateur() { ID = userId, Nom = "lambo", Role = new Utilisateur.Privilege(), Pass = "toto" };
			var mockReadMethods = new Mock<IReadUsersMethods>();
			mockReadMethods.Setup(m => m.GetUserById(expectedUser.ID)).ReturnsAsync(expectedUser);
			var controller = new UsersManagementController(mockReadMethods.Object, null);

			// Act
			var result = await controller.GetUserById(expectedUser.ID);
			//Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			var actualUser = Assert.IsAssignableFrom<Utilisateur>(okResult.Value);
			Assert.Equal(expectedUser, actualUser);
		}
			[Fact]
		public async Task GetUsersById_Returns_NotFound()
		{
			// Arrange
			var userId = 1;
			var mockReadMethods = new Mock<IReadUsersMethods>();
			mockReadMethods.Setup(m => m.GetUserById(userId)).ReturnsAsync((Utilisateur)null);
			var controller = new UsersManagementController(mockReadMethods.Object, null);
			// Act
			var result = await controller.GetUserById(userId);
			//Assert
			var okResult = Assert.IsType<NotFoundObjectResult>(result);
		}
		public async Task GetUsersById_Returns_StatusCode()
		{
			// Arrange
			var userId = 2;
			var mockReadMethods = new Mock<IReadUsersMethods>();
			mockReadMethods.Setup(m => m.GetUserById(userId)).ThrowsAsync(new Exception("Simulated error"));
			var controller = new UsersManagementController(mockReadMethods.Object, null);

			// Act
			var result = await controller.GetUserById(userId);
			//Assert
			var okResult = Assert.IsType<StatusCodeResult>(result);
			Assert.Equal(500,okResult.StatusCode);
			
		}
	}
}


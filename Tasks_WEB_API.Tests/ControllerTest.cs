using Tasks_WEB_API.Controllers;
using Tasks_WEB_API.Models;
using Xunit;
namespace Tasks_WEB_API.Tests;

public class ControllerTest
{
	private readonly DailyTasksMigrationsContext _content;
	[Fact]
	//[Theory]
	public async Task Get_AllUsers( )
	{
		  // Arrange
		  UserManagementController context = new UserManagementController(_content);
		  // Act
		  await context.Get();
		  // Assert
	}
}
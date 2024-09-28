using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMonitoring.BLL.DTOs.Implementations.Requests.User;
using AutoMonitoring.BLL.DTOs.Implementations.Responses.Token;
using AutoMonitoring.Tests.IntegrationTests.Config;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AutoMonitoring.Tests.IntegrationTests;

public class UserIntegrationTests : IClassFixture<WebApplicationFactory<TestStartup>> // Укажите правильное пространство имен для вашего API
{
    private readonly WebApplicationFactory<TestStartup> _factory;
    private readonly HttpClient _client;

    public UserIntegrationTests(WebApplicationFactory<TestStartup> factory) // Программа вашего API
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    // Сценарий 1: Администратор создает пользователя с валидным и уникальным логином
    [Fact]
    public async Task Admin_Creates_User_With_Valid_And_Unique_Login()
    {
        var createUserDto = new UserDTO
        {
            Login = "newUser1",
            Password = "StrongPassword123"
        };

        var response = await _client.PostAsJsonAsync("/api/admin/user/create", createUserDto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // Сценарий 2: Администратор создает пользователя с невалидным, но уникальным логином
    [Fact]
    public async Task Admin_Creates_User_With_Invalid_Login()
    {
        var createUserDto = new UserDTO
        {
            Login = "invalid login",
            Password = "StrongPassword123"
        };

        var response = await _client.PostAsJsonAsync("/api/admin/user/create", createUserDto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // Сценарий 3: Администратор создает пользователя с валидным логином, который уже существует
    [Fact]
    public async Task Admin_Creates_User_With_Existing_Login()
    {
        // Создадим пользователя с логином newUser1
        var existingUserDto = new UserDTO
        {
            Login = "newUser1",
            Password = "StrongPassword123"
        };
        await _client.PostAsJsonAsync("/api/admin/user/create", existingUserDto);

        // Попробуем создать пользователя с таким же логином
        var newUserDto = new UserDTO
        {
            Login = "newUser1",
            Password = "StrongPassword123"
        };

        var response = await _client.PostAsJsonAsync("/api/admin/user/create", newUserDto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // Сценарий 4: Пользователь заходит с правильным deviceName
    [Fact]
    public async Task User_Login_With_Valid_DeviceName()
    {
        var loginDto = new LoginDTO
        {
            Login = "newUser1",
            Password = "StrongPassword123",
            DeviceName = "device1"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // Сценарий 5: Пользователь пытается войти с другим deviceName
    [Fact]
    public async Task User_Login_With_Invalid_DeviceName_Should_Fail()
    {
        var loginDto = new LoginDTO
        {
            Login = "newUser1",
            Password = "StrongPassword123",
            DeviceName = "device2"  // Другой deviceName
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // Сценарий 6: Пользователь заходит с тем же deviceName (с другого браузера)
    [Fact]
    public async Task User_Login_Again_With_Same_DeviceName()
    {
        var loginDto = new LoginDTO
        {
            Login = "newUser1",
            Password = "StrongPassword123",
            DeviceName = "device1"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // Сценарий 7: Пользователь выходит, и другой пользователь заходит с другим deviceName
    [Fact]
    public async Task User_Logout_And_Other_User_Login()
    {
        // Логинимся первым пользователем
        var loginDto = new LoginDTO
        {
            Login = "newUser1",
            Password = "StrongPassword123",
            DeviceName = "device1"
        };
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
        var loginContent = await loginResponse.Content.ReadFromJsonAsync<TokenDTO>();

        // Логаут
        var logoutResponse = await _client.PutAsync($"/api/auth/logout/{loginContent.UserId}", null);
        logoutResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        // Логинимся вторым пользователем с другим deviceName
        var newLoginDto = new LoginDTO
        {
            Login = "newUser1",
            Password = "StrongPassword123",
            DeviceName = "device2"
        };
        var newLoginResponse = await _client.PostAsJsonAsync("/api/auth/login", newLoginDto);
        
        newLoginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
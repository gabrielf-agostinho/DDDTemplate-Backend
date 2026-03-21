using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using DDDTemplate.IntegrationTests.Fixtures;

namespace DDDTemplate.IntegrationTests.API.Users;

public class CreateUserTest(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client = factory.CreateClient();

  [Fact]
  public async Task Should_Create_User()
  {
    var response = await _client.PostAsJsonAsync("/api/v1/users", new
    {
      name = "Teste"
    });

    response.EnsureSuccessStatusCode();
  }
}
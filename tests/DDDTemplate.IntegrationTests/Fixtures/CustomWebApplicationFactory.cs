using Microsoft.AspNetCore.Mvc.Testing;

namespace DDDTemplate.IntegrationTests.Fixtures;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class;
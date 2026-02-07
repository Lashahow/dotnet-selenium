using NBomber.CSharp;
using NBomber.Http.CSharp;

namespace OpencartTests.Tests.PerformanceTests;

/// <summary>
/// Load/Performance tests using NBomber.
/// Functional tests (ApiCrudTests) check "does it work?" with 1 request.
/// Load tests check "how fast is it?" with many requests at once.
/// </summary>
[TestFixture]
[Category("Performance")]
public class ApiLoadTests
{
    // jsonplaceholder is a free API built for testing — it won't block us
    private const string BaseUrl = "https://jsonplaceholder.typicode.com";

    [Test]
    public void GET_Posts_Should_Handle_Constant_Load()
    {
        // Create a shared HTTP client (like a browser that all virtual users share)
        using var httpClient = Http.CreateDefaultClient();

        // SCENARIO = what one virtual user does (sends a GET request)
        var scenario = Scenario.Create("get_posts", async context =>
        {
            // Create a simple GET request to fetch posts
            var request = Http.CreateRequest("GET", $"{BaseUrl}/posts")
                .WithHeader("Accept", "application/json");

            // Send it — NBomber tracks success/failure automatically
            var response = await Http.Send(httpClient, request);

            return response;
        })
        // LOAD SIMULATION = how many requests and for how long
        // "Send 10 new requests every 1 second, for 30 seconds total"
        .WithLoadSimulations(
            Simulation.Inject(
                rate: 10,                              // 10 requests
                interval: TimeSpan.FromSeconds(1),     // every 1 second
                during: TimeSpan.FromSeconds(30)       // for 30 seconds
            )
        );

        // RUN the scenario and collect stats
        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .WithReportFormats(NBomber.Contracts.Stats.ReportFormat.Html)  // generate HTML report
            .WithReportFolder("TestResults/PerformanceReports")           // save report here
            .Run();

        // GET the results for our scenario
        var scenarioStats = stats.ScenarioStats[0];

        // ASSERT: 95% of requests should respond in under 5 seconds
        // (relaxed threshold — it's a free public API over the internet)
        Assert.That(scenarioStats.Ok.Latency.Percent95, Is.LessThan(5000),
            $"p95 latency was {scenarioStats.Ok.Latency.Percent95}ms — should be under 5000ms");

        // ASSERT: at least some requests should succeed
        Assert.That(scenarioStats.Ok.Request.Count, Is.GreaterThan(0),
            "At least some requests should succeed");

        // Print a clean summary
        TestContext.Out.WriteLine($"--- Load Test Results ---");
        TestContext.Out.WriteLine($"Total requests: {scenarioStats.Ok.Request.Count + scenarioStats.Fail.Request.Count}");
        TestContext.Out.WriteLine($"Successful:     {scenarioStats.Ok.Request.Count}");
        TestContext.Out.WriteLine($"Failed:         {scenarioStats.Fail.Request.Count}");
        TestContext.Out.WriteLine($"RPS:            {scenarioStats.Ok.Request.RPS}");
        TestContext.Out.WriteLine($"p50 latency:    {scenarioStats.Ok.Latency.Percent50}ms");
        TestContext.Out.WriteLine($"p95 latency:    {scenarioStats.Ok.Latency.Percent95}ms");
        TestContext.Out.WriteLine($"p99 latency:    {scenarioStats.Ok.Latency.Percent99}ms");
    }
}

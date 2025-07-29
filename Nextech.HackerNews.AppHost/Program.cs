var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Nextech_HackerNews_Server>("nextech-hackernews-server")
    .WithCommand(
        name: "start-client",
        displayName: "Start Client",
        executeCommand: async (ExecuteCommandContext context) => CommandResults.Success(),
        commandOptions: new CommandOptions
        {
            UpdateState = ctx => ResourceCommandState.Enabled,
            Description = "Starts the frontend client",
            IconName = "Play",      // optional icon
            IsHighlighted = true    // optional highlight
        }
    ).WithReference(builder.AddNpmApp("client", "../nextech.hackernews.client"));

builder.Build().Run();
var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Nextech_HackerNews_Server>("nextech-hackernews-server");

builder.Build().Run();

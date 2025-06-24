var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

builder.AddDockerComposeEnvironment("compose");

var apiService = builder.AddProject<Projects.AspireApp1_ApiService>("apiservice");

var frontend = builder.AddProject<Projects.AspireApp1_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddYarp("apiGateWay")
    .WithConfigFile("yarp.json")
    .WithReference(apiService)
    .WithReference(frontend)
    //.WithContainerFiles("/etc", new ContainerFile[]
    //{
    //    new ContainerFile()
    //    {
    //        Contents = File.ReadAllText("yarp.json"),
    //        Name = "yarp.config",
    //        //Mode = UnixFileMode.UserExecute | UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.GroupRead | UnixFileMode.GroupWrite,
    //        Mode = UnixFileMode.UserRead | UnixFileMode.OtherRead,
    //    },
    //},
    //defaultOwner: 1000)
    .WithExternalHttpEndpoints();

builder.Build().Run();

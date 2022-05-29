namespace Yarp.Reverse.Proxy;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add the reverse proxy to capability to the server
        var proxyBuilder = services.AddReverseProxy();
        // Initialize the reverse proxy from the "ReverseProxy" section of configuration
        var proxyConfig = Configuration.GetSection("ReverseProxy");
        proxyBuilder.LoadFromConfig(proxyConfig);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Enable endpoint routing, required for the reverse proxy
        app.UseRouting();
        // Register the reverse proxy routes
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapReverseProxy();
        });
    }
}

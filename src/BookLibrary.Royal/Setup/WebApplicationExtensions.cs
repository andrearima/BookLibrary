namespace BookLibrary.Royal.Setup;

internal static class WebApplicationExtensions
{
    internal static WebApplication Configure(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddServices(builder.Configuration);
        builder.Services.AddCors();

        var app = builder.Build();

        app.UseCors(policies =>
        {
            policies.AllowAnyOrigin();
        });
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.MapControllers();

        return app;
    }
}

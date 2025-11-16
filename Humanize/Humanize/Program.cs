using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore;
using Humanize.Infrastructure.Persistence;
using Humanize.Infrastructure.Persistence.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Entity Framework com Oracle
builder.Services.AddDbContext<HumanizeContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Registro dos Repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEquipeRepository, EquipeRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<IPerguntaRepository, PerguntaRepository>();
builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();
builder.Services.AddScoped<IRespostaRepository, RespostaRepository>();

// Configuração dos Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
      options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Configuração do CORS
builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
          .AllowAnyMethod()
    .AllowAnyHeader();
    });
});

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "Humanize API", 
        Version = "v1",
        Description = "API para sistema de avaliação de bem-estar dos colaboradores"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
app.UseSwaggerUI(c =>
 {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Humanize API v1");
        c.RoutePrefix = "swagger";
    });
}
else
{
    app.UseExceptionHandler("/Error");
 app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// Endpoint de informações da API
app.MapGet("/", () => new
{
    Application = "Humanize API",
    Version = "1.0.0",
    Environment = app.Environment.EnvironmentName,
    Timestamp = DateTime.Now
}).WithTags("Info");

app.Run();

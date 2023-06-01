using ControlePontoAPI.Helpers;
using ControlePontoAPI.Interfaces;
using ControlePontoAPI.Repositories;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IRegistroPontoRepository, RegistroPontoRepository>();
builder.Services.AddSingleton<IRelatorioHelper, RelatorioHelper>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Controle de Ponto API", Version = "v1" });

    // As 2 variáveis abaixo formam o caminho do arquivo .xml gerado pelo Swagger:
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    // A seguir, garantimos que os comentários XML adicionados às controllers
    // sejam incluídos no arquivo .xml gerado pelo Swagger:
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

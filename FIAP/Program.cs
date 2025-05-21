using AutoMapper;
using FIAP;
using FIAP.Data;
using FIAP.Models;
using FIAP.Repository;
using FIAP.Repository.Interfaces;
using FIAP.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllAcess", policy =>
    {
        //policy.WithOrigins(new[] { "admin@admin.com.br", "matheus.limamst@gmail.com" });
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});
#endregion

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
        options.SuppressMapClientErrors = true;
    });

var connectionString = builder.Configuration.GetConnectionString("databaseUrl");
builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging()
);

#region Repository
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
#endregion

#region Autenticacao
var secretToken = Encoding.UTF8.GetBytes(Settings.SECRET_TOKEN);

bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
{
    if (expires != null)
    {
        return expires > DateTime.UtcNow;
    }
    return false;
}

builder.Services
            .AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer( opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = "fiap",
                    IssuerSigningKey = new SymmetricSecurityKey(secretToken),
                    RequireExpirationTime = true,
                    LifetimeValidator = CustomLifetimeValidator
                };
            });
#endregion

#region AutoMapper

var mapperConfig = new AutoMapper.MapperConfiguration(m =>
{
    m.AllowNullCollections = true;
    m.AllowNullDestinationValues = true;

    m.CreateMap<UsuarioModel, LoginResponseVM>();
    m.CreateMap<LoginRequestVM, UsuarioModel>();
    m.CreateMap<UsuarioModel, UsuarioResponseVM>();
    m.CreateMap<ProdutoModel, ProdutoResponseVM>()
    .ForMember(dest => dest.NomeCategoria, opt => opt.MapFrom(src => src.Categoria.NomeCategoria))
    .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.Usuario.EmailUsuario));

});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion

#region versao
builder.Services.AddApiVersioning(options =>
{
    options.UseApiBehavior = false;
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(3, 0);
    options.ApiVersionReader =
        ApiVersionReader.Combine(
            new HeaderApiVersionReader("x-api-version"),
            new QueryStringApiVersionReader(),
            new UrlSegmentApiVersionReader());
});

builder.Services.AddVersionedApiExplorer(setup => {
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion


var app = builder.Build();

app.UseApiVersioning();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllAcess");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

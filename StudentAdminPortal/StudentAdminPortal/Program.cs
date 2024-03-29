using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.Repositories;
using StudentAdminPortal.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors((options) => {
        options.AddPolicy("angularApplication",
        (builder) =>
        {
            builder.WithOrigins("http://localhost:4200").AllowAnyHeader().
                WithMethods("GET", "POST", "PUT", "DELETE").WithExposedHeaders("*");
        });
    });


builder.Services.AddControllers();
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<StudentAdminContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("StudentAdminPortalDB")));
builder.Services.AddScoped<IStudentRepository, SqlStudentRepository>(); 
builder.Services.AddScoped<IImageRepository, LocalStorageImageRepository>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(Environment.ContentRootPath, "Resources")),
//    RequestPath = "/Resources"
//});

app.UseCors("angularApplication");

app.UseAuthorization();

app.MapControllers();

app.Run();

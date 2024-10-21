using bangnaAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// เปิดใช้งาน CORS และอนุญาตการเข้าถึงจากโดเมนอื่นๆ
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin() // อนุญาตให้ทุกโดเมนเข้าถึง
               .AllowAnyMethod() // อนุญาตให้ใช้ทุก Method เช่น GET, POST
               .AllowAnyHeader(); // อนุญาตให้ส่ง Headers ทั้งหมด
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// เชื่อมต่อกับฐานข้อมูลโดยใช้ Connection String
builder.Services.AddDbContext<db_bangna1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// เรียกใช้งาน CORS ใน pipeline
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();

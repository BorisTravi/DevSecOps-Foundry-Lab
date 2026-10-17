var builder = WebApplication.CreateBuilder(args);

// 1. Agrega esta línea para habilitar los controladores en el contenedor de dependencias
builder.Services.AddControllers(); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 2. Agrega esta línea para mapear las rutas de tu AgenteController
app.MapControllers(); 

app.Run();
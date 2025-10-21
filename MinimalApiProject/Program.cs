using Microsoft.EntityFrameworkCore;
using MinimalApiProject.Data;
using MinimalApiProject.Models;

var builder = WebApplication.CreateBuilder(args);


var dbFileName = "ViniciusBueno_LuANA_VICENTE.db";
var connectionString = $"Data Source={dbFileName}";

builder.Services.AddDbContext<ConsumoContext>(options =>
	options.UseSqlite(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<ConsumoContext>();
	db.Database.EnsureCreated();
}

app.MapPost("/api/consumo/cadastrar", async (Consumo consumo, ConsumoContext db) =>
{
	if (consumo.Mes < 1 || consumo.Mes > 12)
		return Results.BadRequest("Mes deve estar entre 1 e 12.");
	if (consumo.Ano < 2000)
		return Results.BadRequest("Ano deve ser >= 2000.");
	if (consumo.M3Consumidos <= 0)
		return Results.BadRequest("m3Consumidos deve ser > 0.");

	var exists = await db.Consumos.AnyAsync(c => c.Cpf == consumo.Cpf && c.Mes == consumo.Mes && c.Ano == consumo.Ano);
	if (exists)
		return Results.Conflict("Leitura já cadastrada para este CPF, mês e ano.");

	consumo.ConsumoFaturado = consumo.M3Consumidos < 10 ? 10 : consumo.M3Consumidos;

	double tarifa;
	var cf = consumo.ConsumoFaturado;
	if (cf <= 10) tarifa = 2.50;
	else if (cf <= 20) tarifa = 3.50;
	else if (cf <= 50) tarifa = 5.00;
	else tarifa = 6.50;
	consumo.Tarifa = tarifa;

	consumo.ValorAgua = consumo.ConsumoFaturado * consumo.Tarifa;

	
	var bandeira = (consumo.Bandeira ?? "Verde").ToLower();
	double adicionalPercent = 0;
	if (bandeira.Contains("amarela")) adicionalPercent = 0.10;
	else if (bandeira.Contains("vermelha")) adicionalPercent = 0.20;
	else adicionalPercent = 0.0; 

	consumo.AdicionalBandeira = consumo.ValorAgua * adicionalPercent;

	consumo.TaxaEsgoto = consumo.PossuiEsgoto ? (consumo.ValorAgua + consumo.AdicionalBandeira) * 0.80 : 0.0;

	consumo.Total = consumo.ValorAgua + consumo.AdicionalBandeira + consumo.TaxaEsgoto;

	db.Consumos.Add(consumo);
	await db.SaveChangesAsync();

	return Results.Created($"/api/consumo/buscar/{consumo.Cpf}/{consumo.Mes}/{consumo.Ano}", consumo);
});

app.MapGet("/api/consumo/listar", async (ConsumoContext db) =>
{
	var list = await db.Consumos.ToListAsync();
	if (list == null || list.Count == 0) return Results.NotFound();
	return Results.Ok(list);
});

app.MapGet("/api/consumo/buscar/{cpf}/{mes:int}/{ano:int}", async (string cpf, int mes, int ano, ConsumoContext db) =>
{
	var item = await db.Consumos.FirstOrDefaultAsync(c => c.Cpf == cpf && c.Mes == mes && c.Ano == ano);
	if (item == null) return Results.NotFound();
	return Results.Ok(item);
});

app.MapDelete("/api/consumo/remover/{cpf}/{mes:int}/{ano:int}", async (string cpf, int mes, int ano, ConsumoContext db) =>
{
	var item = await db.Consumos.FirstOrDefaultAsync(c => c.Cpf == cpf && c.Mes == mes && c.Ano == ano);
	if (item == null) return Results.NotFound();
	db.Consumos.Remove(item);
	await db.SaveChangesAsync();
	return Results.Ok();
});

app.MapGet("/api/consumo/total-geral", async (ConsumoContext db) =>
{
	var any = await db.Consumos.AnyAsync();
	if (!any) return Results.NotFound();
	var total = await db.Consumos.SumAsync(c => c.Total);
	return Results.Ok(new { totalGeral = total });
});

app.Run();


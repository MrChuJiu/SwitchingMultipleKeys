using System.Text;
using Microsoft.EntityFrameworkCore;
using SwitchingMultipleKeys;
using SwitchingMultipleKeys.SqlServer;
using Test_MultipleKets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<SqlServerMultipleKeyContext>(o => o.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=SwitchingMultipleKeys;Trusted_Connection=True"
));

builder.Services.AddMultipleKeysSqlServer(options =>
{
    options.Keys.Add(new DiMultipleKeyEntity() { KeyId = "a", Password = "11111" });
    options.Keys.Add(new DiMultipleKeyEntity() { KeyId = "b", Password = "222222222222" });
    options.Keys.Add(new DiMultipleKeyEntity() { KeyId = "c", Password = "33333333333333333333" });
    options.Keys.Add(new SMZDMMultipleKeyEntity() { KeyId = "b", HttpUrl = "www.baidu.com" });
    options.Keys.Add(new SMZDMMultipleKeyEntity() { KeyId = "c", HttpUrl = "www.google.com" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseMultipleKeysSqlServerSeedData();

app.Run(async context =>
{
    var keyProviderDi = context.RequestServices.GetService<IMultipleKeysProvider<DiMultipleKeyEntity>>();
    for (int i = 1; i < 120; i++)
    {
        string name = $"I am Flag:{i}";
        await Task.Run(() =>
        {
            var key = keyProviderDi.GetMultipleKeys();
            Console.WriteLine(name + "--------------------" + (key != null ? ((DiMultipleKeyEntity)key).Password : "null111"));
        });
    }

    var keyProviderSMZDM = context.RequestServices.GetService<IMultipleKeysProvider<SMZDMMultipleKeyEntity>>();
    for (int i = 1; i < 120; i++)
    {
        string name = $"I am Flag:{i}";
        await Task.Run(() =>
        {
            var key = keyProviderSMZDM.GetMultipleKeys();
            Console.WriteLine(name + "--------------------" + (key != null ? ((SMZDMMultipleKeyEntity)key).HttpUrl : "Url11111111113213"));
        });
    }


});

app.Run();
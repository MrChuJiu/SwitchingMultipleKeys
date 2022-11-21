using Microsoft.EntityFrameworkCore;
using SwitchingMultipleKeys;
using SwitchingMultipleKeys.SqlServer;
using Test_MultipleKets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<SqlServerMultipleKeyContext>(o => o.UseSqlServer(
                @"Server=192.168.31.178; Database=EFQuerying; User=sa; Password=shinetech:123; Connection Timeout=600;MultipleActiveResultSets=true;"
));

builder.Services.AddMultipleKeysSqlServer(options =>
{
    options.Keys.Add(new DiMultipleKeyEntity() { KeyId = "a", PassWord = "11111" });
    options.Keys.Add(new DiMultipleKeyEntity() { KeyId = "b", PassWord = "222222222222" });
    options.Keys.Add(new DiMultipleKeyEntity() { KeyId = "c", PassWord = "33333333333333333333" });
});




var app = builder.Build();

using var context = app.Services.GetService<SqlServerMultipleKeyContext>();
context.Database.EnsureDeleted();
context.Database.EnsureCreated();

var data = new List<DiMultipleKeyEntity>()
    {
        new DiMultipleKeyEntity() { KeyId = "a", PassWord = "11111" },
        new DiMultipleKeyEntity() { KeyId = "b", PassWord = "222222222222" },
        new DiMultipleKeyEntity() { KeyId = "c", PassWord = "33333333333333333333" }
    };

foreach (var item in data)
{
    var info = new SqlServerMultipleKeyInfo()
    {
        KeyName = nameof(DiMultipleKeyEntity),
        Maximum = 40,
        ResidueDegree = 40,
        Data = item,
        CreateTime = DateTime.Now
    };
    context.MultipleKeyInfo.Add(info);
}

context.SaveChanges();
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


app.Run(async context =>
{
    var keyProvider = context.RequestServices.GetService<IMultipleKeysProvider<DiMultipleKeyEntity>>();
    for (int i = 1; i < 120; i++)
    {
        string name = $"I am Flag:{i}";
        await Task.Run(() =>
        {
            var key = keyProvider.GetMultipleKeys();
            Console.WriteLine(name + "--------------------" + (key != null ? ((DiMultipleKeyEntity)key).PassWord : "null111"));
        });

    }
});

app.Run();

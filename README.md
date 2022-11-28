
# SwitchingMultipleKeys


### Default
```cs
builder.Services.AddMultipleKeys(options =>
{
    options.Keys.Add(new DiMultipleKeyEntity(LifeCycle.Day) { KeyId = "a", Password = "11111"});
    options.Keys.Add(new DiMultipleKeyEntity(LifeCycle.Month) { KeyId = "b", Password = "222222222222"});
    options.Keys.Add(new DiMultipleKeyEntity(LifeCycle.Year) { KeyId = "c", Password = "33333333333333333333"});

    options.Keys.Add(new SMZDMMultipleKeyEntity(LifeCycle.Month) { KeyId = "b", HttpUrl = "www.baidu.com"});
    options.Keys.Add(new SMZDMMultipleKeyEntity(LifeCycle.Year) { KeyId = "c", HttpUrl = "www.google.com"});
});
```


### SQLServer
```cs
builder.Services.AddDbContext<SqlServerMultipleKeyContext>(o => o.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=SwitchingMultipleKeys;Trusted_Connection=True"
));

builder.Services.AddMultipleKeysSqlServer(options =>
{
    options.Keys.Add(new DiMultipleKeyEntity(LifeCycle.Day) { KeyId = "a", Password = "11111" });
    options.Keys.Add(new DiMultipleKeyEntity(LifeCycle.Month) { KeyId = "b", Password = "222222222222" });
    options.Keys.Add(new DiMultipleKeyEntity(LifeCycle.Year) { KeyId = "c", Password = "33333333333333333333" });

    options.Keys.Add(new SMZDMMultipleKeyEntity(LifeCycle.Month) { KeyId = "b", HttpUrl = "www.baidu.com" });
    options.Keys.Add(new SMZDMMultipleKeyEntity(LifeCycle.Year) { KeyId = "c", HttpUrl = "www.google.com" });
});
```

```cs
app.UseMultipleKeysSqlServerSeedData();
```



## Test

```cs
app.Run(async context =>
{
    var keyProviderDi = context.RequestServices.GetService<IMultipleKeysProvider<DiMultipleKeyEntity>>();
    for (int i = 1; i < 120; i++)
    {
        string name = $"I am Flag:{i}";
        await Task.Run(() =>
        {
            var key = keyProviderDi.GetMultipleKeys();
            Console.WriteLine(name + "--------------------" + (key != null ? key.Password : "null111"));
        });
    }

    var keyProviderSMZDM = context.RequestServices.GetService<IMultipleKeysProvider<SMZDMMultipleKeyEntity>>();
    for (int i = 1; i < 120; i++)
    {
        string name = $"I am Flag:{i}";
        await Task.Run(() =>
        {
            var key = keyProviderSMZDM.GetMultipleKeys();
            Console.WriteLine(name + "--------------------" + (key != null ? key.HttpUrl : "Url11111111113213"));
        });
    }
});
```





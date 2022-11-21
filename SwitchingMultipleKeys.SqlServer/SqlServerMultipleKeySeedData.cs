﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SwitchingMultipleKeys;

namespace SwitchingMultipleKeys.SqlServer;

public class SqlServerMultipleKeySeedData : IMultipleKeySeedData
{
    private readonly IServiceProvider _serviceProvider;
    protected MultipleKeysOptions Options { get; }

    public SqlServerMultipleKeySeedData(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Options = _serviceProvider.GetService<IOptions<MultipleKeysOptions>>().Value;
    }

    public async Task SeedAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetService<SqlServerMultipleKeyContext>();

        foreach (var item in Options.Keys)
        {
            var maximum = ((MultipleKeyAttribute)item.GetType().GetCustomAttributes(false).FirstOrDefault(x => x.GetType().FullName == typeof(MultipleKeyAttribute).FullName)).Maximum; 
            var info = new SqlServerMultipleKeyInfo()
            {
                KeyName = item.GetType().Name,
                Maximum = maximum,
                ResidueDegree = maximum,
                Data = item,
                CreateTime = DateTime.Now
            };
            context.MultipleKeyInfo.Add(info);
        }
        await context.SaveChangesAsync().ConfigureAwait(false);
    }
}


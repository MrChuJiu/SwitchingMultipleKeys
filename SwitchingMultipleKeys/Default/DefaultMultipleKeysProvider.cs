using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SwitchingMultipleKeys
{
    public class DefaultMultipleKeysProvider<T> : IMultipleKeysProvider<T> where T : IMultipleKeyEntity
    {
        protected IDictionary<Type, DefaultMultipleKeyInfo> MultipleKeysDefinitions =>  _lazyMultipleKeysDefinitions.Value;
        private readonly Lazy<Dictionary<Type, DefaultMultipleKeyInfo>> _lazyMultipleKeysDefinitions;

        private static readonly object objLock = new object();

        protected MultipleKeysOptions Options { get; }

        public DefaultMultipleKeysProvider(IOptions<MultipleKeysOptions> options)
        {
            Options = options.Value;

            _lazyMultipleKeysDefinitions = new Lazy<Dictionary<Type, DefaultMultipleKeyInfo>>(
                InitializationMultipleKeysDefinitions,
                isThreadSafe: true
            );
        }

        public T GetMultipleKeys()
        {
            lock (objLock)
            {
                if (MultipleKeysDefinitions[typeof(T)].CurrentLocation >=  MultipleKeysDefinitions[typeof(T)].Maximum)
                {
                    MultipleKeysDefinitions[typeof(T)].CurrentKeysIndex += 1;
                    MultipleKeysDefinitions[typeof(T)].CurrentLocation = 0;
                }

                if (MultipleKeysDefinitions[typeof(T)].CurrentKeysIndex == MultipleKeysDefinitions[typeof(T)].Keys.Count)
                {

                    return default;
                }

                var multipleKeyInfo = MultipleKeysDefinitions[typeof(T)];
                var key = multipleKeyInfo.Keys[multipleKeyInfo.CurrentKeysIndex];

                MultipleKeysDefinitions[typeof(T)].CurrentLocation += 1;

                return (T)key;
            }

        }

        protected virtual Dictionary<Type, DefaultMultipleKeyInfo> InitializationMultipleKeysDefinitions()
        {
            var data = new Dictionary<Type, List<IMultipleKeyEntity>>();
            foreach (var key in Options.Keys)
            {
                var keyName = key.GetType();
                if (!data.ContainsKey(keyName))
                {
                    data[keyName] = new List<IMultipleKeyEntity>();
                }
                data[keyName].Add(key);
            }

            var result = new Dictionary<Type, DefaultMultipleKeyInfo>();
            foreach (var item in data.Keys)
            {
                if (!result.ContainsKey(item))
                {
                    result[item] = new DefaultMultipleKeyInfo(((MultipleKeyAttribute)item.GetCustomAttributes(false)[2]).Maximum);
                }
                result[item].Keys = data[item];
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SwitchingMultipleKeys
{
    public class DefaultMultipleKeysProvider<T> : IMultipleKeysProvider<T> where T : MultipleKeyEntity
    {
        protected IDictionary<Type, List<MultipleKeyEntity>> MultipleKeysDefinitions =>  _lazyMultipleKeysDefinitions.Value;
        private readonly Lazy<Dictionary<Type, List<MultipleKeyEntity>>> _lazyMultipleKeysDefinitions;



        protected IDictionary<Type, List<MultipleKeyEntity>> MultipleKeysRecordDefinitions =
            new Dictionary<Type, List<MultipleKeyEntity>>();


        private static readonly object objLock = new object();

        protected MultipleKeysOptions Options { get; }

        public DefaultMultipleKeysProvider(IOptions<MultipleKeysOptions> options)
        {
            Options = options.Value;

            _lazyMultipleKeysDefinitions = new Lazy<Dictionary<Type, List<MultipleKeyEntity>>>(
                InitializationMultipleKeysDefinitions,
                isThreadSafe: true
            );

        }

        public T GetMultipleKeys()
        {
            lock (objLock)
            {
                var multipleKeyInfo = MultipleKeysDefinitions[typeof(T)].Where(x=> (x.ExpirationDate > DateTime.Now.Date || x.ExpirationDate == null) && x.ResidueDegree > 0).OrderBy(x=>x.ResidueDegree).ThenBy(x=>x.LifeCycle).FirstOrDefault();

                if (multipleKeyInfo == null)
                {
                    if (MultipleKeysDefinitions[typeof(T)].Any(x => x.ExpirationDate > DateTime.Now.Date))
                    {
                        var multipleKeyList = MultipleKeysRecordDefinitions[typeof(T)].ToList();
                        MultipleKeysDefinitions[typeof(T)] = multipleKeyList;

                        multipleKeyInfo = GetMultipleKeys();
                        if (multipleKeyInfo != default)
                        {
                            return (T)multipleKeyInfo;
                        }
                    }

                    return default;
                }

                multipleKeyInfo.ResidueDegree -= 1;

                return (T)multipleKeyInfo;
            }

        }

        protected virtual Dictionary<Type, List<MultipleKeyEntity>> InitializationMultipleKeysDefinitions()
        {
            var result = new Dictionary<Type, List<MultipleKeyEntity>>();
            foreach (var key in Options.Keys)
            {
                var keyName = key.GetType();
                if (!result.ContainsKey(keyName))
                {
                    result[keyName] = new List<MultipleKeyEntity>();
                }
                result[keyName].Add(key);
            }
            return result;
        }
    }
}

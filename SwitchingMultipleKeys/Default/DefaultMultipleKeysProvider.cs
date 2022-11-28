using Microsoft.Extensions.Options;

namespace SwitchingMultipleKeys
{
    public class DefaultMultipleKeysProvider<T> : IMultipleKeysProvider<T> where T : MultipleKeyEntity
    {
        protected IDictionary<Type, List<MultipleKeyEntity>> MultipleKeysDefinitions =>  _lazyMultipleKeysDefinitions.Value;
        private readonly Lazy<Dictionary<Type, List<MultipleKeyEntity>>> _lazyMultipleKeysDefinitions;

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
                var multipleKeyInfo = MultipleKeysDefinitions[typeof(T)].Where(x=> x.StartDate <= DateTime.Now && (x.ExpirationDate > DateTime.Now || x.ExpirationDate == null) && x.ResidueDegree > 0).OrderBy(x=>x.ResidueDegree).ThenBy(x=>x.LifeCycle).FirstOrDefault();

                if (multipleKeyInfo == null)
                {
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

        public void TimingUpdateMultipleKeys()
        {
            lock (objLock)
            {
                var multipleKeys = new Dictionary<Type, List<MultipleKeyEntity>>();
                foreach (var keysDefinition in MultipleKeysDefinitions)
                {
                    multipleKeys[keysDefinition.Key] = keysDefinition.Value
                        .Where(x => x.StartDate <= DateTime.Now && x.ExpirationDate > DateTime.Now).ToList();
                }

                foreach (var keysDefinition in multipleKeys)
                {
                    foreach (var value in keysDefinition.Value)
                    {
                        if (value.ExpirationDate > DateTime.Now && value.ExpirationDate < DateTime.Now.AddHours(24))
                        {
                            var data = (MultipleKeyEntity)value.Clone();
                            data.UpdateLifeCycle(DateTime.Today.AddDays(1), value.LifeCycle);
                            MultipleKeysDefinitions[keysDefinition.Key].Add(data);
                        }
                    }
                }
            }
        }
    }
}

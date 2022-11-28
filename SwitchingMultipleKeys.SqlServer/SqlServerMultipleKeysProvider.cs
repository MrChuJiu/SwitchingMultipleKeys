using Microsoft.Extensions.Options;

namespace SwitchingMultipleKeys.SqlServer
{
    public class SqlServerMultipleKeysProvider<T> : IMultipleKeysProvider<T> where T : MultipleKeyEntity
    {
        private static readonly object objLock = new object();
        protected MultipleKeysOptions Options { get; }

        private readonly SqlServerMultipleKeyContext _context;

        public SqlServerMultipleKeysProvider(IOptions<MultipleKeysOptions> options, SqlServerMultipleKeyContext context)
        {
            Options = options.Value;
            _context = context;
        }

        public T GetMultipleKeys()
        {
            lock (objLock)
            {
                var multipleKeyInfo =  _context.MultipleKeyInfo.Where(x =>  x.KeyName == typeof(T).Name && x.StartDate <= DateTime.Now && (x.ExpirationDate > DateTime.Now || x.ExpirationDate == null) && x.ResidueDegree > 0).OrderBy(x => x.ResidueDegree).FirstOrDefault();

                if (multipleKeyInfo == null)
                {
                    return default;
                }

                multipleKeyInfo.ResidueDegree -= 1;

                _context.MultipleKeyInfo.Update(multipleKeyInfo);
                _context.SaveChanges();

                return (T)multipleKeyInfo.Data;
            }
        }

        public void TimingUpdateMultipleKeys()
        {
            lock (objLock)
            {
                var multipleKeys = _context.MultipleKeyInfo
                    .Where(x => x.StartDate <= DateTime.Now && x.ExpirationDate > DateTime.Now).ToList();

                foreach (var value in multipleKeys)
                {
                    if (value.ExpirationDate > DateTime.Now && value.ExpirationDate < DateTime.Now.AddHours(24))
                    {
                        var data = (MultipleKeyEntity)value.Data.Clone();
                        data.UpdateLifeCycle(DateTime.Today.AddDays(1), value.LifeCycle);

                        var info = new SqlServerMultipleKeyInfo(data.LifeCycle,data.Maximum)
                        {
                            KeyName = data.GetType().Name,
                            Data = data,
                        };
                        info.UpdateLifeCycle(DateTime.Today.AddDays(1), value.LifeCycle);
                        _context.MultipleKeyInfo.Add(info);
                    }
                }
                _context.SaveChanges();
            }
        }
    }
}

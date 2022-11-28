namespace SwitchingMultipleKeys
{
    public class MultipleKeyEntity: ICloneable
    {
        public MultipleKeyEntity(LifeCycle lifeCycle = LifeCycle.Day, int maximum = 40)
        {
            this.LifeCycle = lifeCycle;
            this.Maximum = maximum;
            this.ResidueDegree = maximum;
            this.UpdateLifeCycle(DateTime.Now.Date);
        }

        public void UpdateLifeCycle(DateTime startDate, LifeCycle lifeCycle = LifeCycle.Day)
        {
            this.StartDate = startDate;
            switch (lifeCycle)
            {
                case LifeCycle.NotRepeat:
                    this.ExpirationDate = null;
                    break;
                case LifeCycle.Day:
                    this.ExpirationDate = DateTime.Today.AddDays(1).AddSeconds(-1);
                    break;
                case LifeCycle.Month:
                    this.ExpirationDate = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(1).AddDays(-1);
                    break;
                case LifeCycle.Year:
                    this.ExpirationDate = DateTime.Now.AddYears(1).AddDays(-1);
                    break;
            }
        }

        public LifeCycle LifeCycle { get; set; }

        public int Maximum { get; set; }

        public int ResidueDegree { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime StartDate { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
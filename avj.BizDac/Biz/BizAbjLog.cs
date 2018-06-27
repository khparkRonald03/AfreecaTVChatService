namespace avj.BizDac
{
    public class BizAbjLog
    {
        DacAbjLog Dac { get; set; }

        public BizAbjLog()
        {
            Dac = new DacAbjLog();
        }

        public void SetAbjLog(string log)
        {
            var query = ApiLog.InsertAbjLog;

            Dac.SetApiLog(query, log);
        }
    }
}

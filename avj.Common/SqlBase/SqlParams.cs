namespace avj.Common
{
    public class DacMysqlParam
    {
        public string ParamName { get; set; }
        public MysqlDacDbType DbType { get; set; }
        public object ParamValue { get; set; }

        public DacMysqlParam(string p_ParamName, MysqlDacDbType p_DbType, object p_ParamValue)
        {
            ParamName = p_ParamName;
            DbType = p_DbType;
            ParamValue = p_ParamValue;
        }
    }

    public class DacSqlServerParam
    {
        public string ParamName { get; set; }
        public SqlServerDbType DbType { get; set; }
        public object ParamValue { get; set; }

        public DacSqlServerParam(string p_ParamName, SqlServerDbType p_DbType, object p_ParamValue)
        {
            ParamName = p_ParamName;
            DbType = p_DbType;
            ParamValue = p_ParamValue;
        }
    }
}

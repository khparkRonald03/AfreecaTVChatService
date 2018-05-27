namespace avj.Common
{
    public class MysqlParam
    {
        public string ParamName { get; set; }
        public MysqlDbType DbType { get; set; }
        public object ParamValue { get; set; }

        public MysqlParam(string p_ParamName, MysqlDbType p_DbType, object p_ParamValue)
        {
            ParamName = p_ParamName;
            DbType = p_DbType;
            ParamValue = p_ParamValue;
        }
    }

    public class SqlServerParam
    {
        public string ParamName { get; set; }
        public SqlServerDbType DbType { get; set; }
        public object ParamValue { get; set; }

        public SqlServerParam(string p_ParamName, SqlServerDbType p_DbType, object p_ParamValue)
        {
            ParamName = p_ParamName;
            DbType = p_DbType;
            ParamValue = p_ParamValue;
        }
    }
}

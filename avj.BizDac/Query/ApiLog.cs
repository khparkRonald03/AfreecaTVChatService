using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace avj.BizDac
{
    public class ApiLog
    {
        public static string InsertAbjLog = @"
            INSERT INTO `abjchat`.`abj_Api_Log`
            (`log`,
            `AddDate`)
            VALUES
            (@log,
            now());
            ";
    }
}

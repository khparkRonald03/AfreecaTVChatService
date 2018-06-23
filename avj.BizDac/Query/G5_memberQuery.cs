namespace avj.BizDac
{
    public class G5_memberQuery
    {
        public static string SelectAllG5_member = @"
            SELECT *
              FROM `abjchat`.`g5_member`;
            ";

        public static string SelectAllG5_memberByAbjID = @"
            SELECT *
              FROM `abjchat`.`g5_member`
             WHERE mb_1 = '{0}';
            ";
    }
}

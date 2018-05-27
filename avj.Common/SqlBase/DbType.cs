namespace avj.Common
{
    public enum MysqlDbType
    {
        //
        // 요약:
        //     MySql.Data.MySqlClient.MySqlDbType.Decimal
        //     A fixed precision and scale numeric value between -1038 -1 and 10 38 -1.
        Decimal = 0,
        //
        // 요약:
        //     MySql.Data.MySqlClient.MySqlDbType.Byte
        //     The signed range is -128 to 127. The unsigned range is 0 to 255.
        Byte = 1,
        //
        // 요약:
        //     MySql.Data.MySqlClient.MySqlDbType.Int16
        //     A 16-bit signed integer. The signed range is -32768 to 32767. The unsigned range
        //     is 0 to 65535
        Int16 = 2,
        //
        // 요약:
        //     MySql.Data.MySqlClient.MySqlDbType.Int32
        //     A 32-bit signed integer
        Int32 = 3,
        //
        // 요약:
        //     System.Single
        //     A small (single-precision) floating-point number. Allowable values are -3.402823466E+38
        //     to -1.175494351E-38, 0, and 1.175494351E-38 to 3.402823466E+38.
        Float = 4,
        //
        // 요약:
        //     MySql.Data.MySqlClient.MySqlDbType.Double
        //     A normal-size (double-precision) floating-point number. Allowable values are
        //     -1.7976931348623157E+308 to -2.2250738585072014E-308, 0, and 2.2250738585072014E-308
        //     to 1.7976931348623157E+308.
        Double = 5,
        //
        // 요약:
        //     A timestamp. The range is '1970-01-01 00:00:00' to sometime in the year 2037
        Timestamp = 7,
        //
        // 요약:
        //     MySql.Data.MySqlClient.MySqlDbType.Int64
        //     A 64-bit signed integer.
        Int64 = 8,
        //
        // 요약:
        //     Specifies a 24 (3 byte) signed or unsigned value.
        Int24 = 9,
        //
        // 요약:
        //     Date The supported range is '1000-01-01' to '9999-12-31'.
        Date = 10,
        //
        // 요약:
        //     Time
        //     The range is '-838:59:59' to '838:59:59'.
        Time = 11,
        //
        // 요약:
        //     Datetime The supported range is '1000-01-01 00:00:00' to '9999-12-31 23:59:59'.
        Datetime = 12,
        //
        // 요약:
        //     A year in 2- or 4-digit format (default is 4-digit). The allowable values are
        //     1901 to 2155, 0000 in the 4-digit year format, and 1970-2069 if you use the 2-digit
        //     format (70-69).
        Year = 13,
        //
        // 요약:
        //     Obsolete Use Datetime or Date type
        Newdate = 14,
        //
        // 요약:
        //     A variable-length string containing 0 to 65535 characters
        VarString = 15,
        //
        // 요약:
        //     Bit-field data type
        Bit = 16,
        //
        // 요약:
        //     JSON
        JSON = 245,
        //
        // 요약:
        //     New Decimal
        NewDecimal = 246,
        //
        // 요약:
        //     An enumeration. A string object that can have only one value, chosen from the
        //     list of values 'value1', 'value2', ..., NULL or the special "" error value. An
        //     ENUM can have a maximum of 65535 distinct values
        Enum = 247,
        //
        // 요약:
        //     A set. A string object that can have zero or more values, each of which must
        //     be chosen from the list of values 'value1', 'value2', ... A SET can have a maximum
        //     of 64 members.
        Set = 248,
        //
        // 요약:
        //     A binary column with a maximum length of 255 (2^8 - 1) characters
        TinyBlob = 249,
        //
        // 요약:
        //     A binary column with a maximum length of 16777215 (2^24 - 1) bytes.
        MediumBlob = 250,
        //
        // 요약:
        //     A binary column with a maximum length of 4294967295 or 4G (2^32 - 1) bytes.
        LongBlob = 251,
        //
        // 요약:
        //     A binary column with a maximum length of 65535 (2^16 - 1) bytes.
        Blob = 252,
        //
        // 요약:
        //     A variable-length string containing 0 to 255 bytes.
        VarChar = 253,
        //
        // 요약:
        //     A fixed-length string.
        String = 254,
        //
        // 요약:
        //     Geometric (GIS) data type.
        Geometry = 255,
        //
        // 요약:
        //     Unsigned 8-bit value.
        UByte = 501,
        //
        // 요약:
        //     Unsigned 16-bit value.
        UInt16 = 502,
        //
        // 요약:
        //     Unsigned 32-bit value.
        UInt32 = 503,
        //
        // 요약:
        //     Unsigned 64-bit value.
        UInt64 = 508,
        //
        // 요약:
        //     Unsigned 24-bit value.
        UInt24 = 509,
        //
        // 요약:
        //     Fixed length binary string.
        Binary = 600,
        //
        // 요약:
        //     Variable length binary string.
        VarBinary = 601,
        //
        // 요약:
        //     A text column with a maximum length of 255 (2^8 - 1) characters.
        TinyText = 749,
        //
        // 요약:
        //     A text column with a maximum length of 16777215 (2^24 - 1) characters.
        MediumText = 750,
        //
        // 요약:
        //     A text column with a maximum length of 4294967295 or 4G (2^32 - 1) characters.
        LongText = 751,
        //
        // 요약:
        //     A text column with a maximum length of 65535 (2^16 - 1) characters.
        Text = 752,
        //
        // 요약:
        //     A guid column
        Guid = 800
    }

    //
    // 요약:
    //     System.Data.SqlClient.SqlParameter에 사용할 필드, 속성의 SQL Server 데이터 형식을 지정합니다.
    public enum SqlServerDbType
    {
        //
        // 요약:
        //     System.Int64.64비트 부호 있는 정수입니다.
        BigInt = 0,
        //
        // 요약:
        //     System.Byte 형식의 System.Array입니다.범위가 1바이트에서 8,000바이트까지인 이진 데이터의 고정 길이 스트림입니다.
        Binary = 1,
        //
        // 요약:
        //     System.Boolean.0, 1 또는 null일 수 있는 부호 없는 숫자 값입니다.
        Bit = 2,
        //
        // 요약:
        //     System.String.범위가 1자에서 8,000자까지이고 유니코드가 아닌 문자의 고정 길이 스트림입니다.
        Char = 3,
        //
        // 요약:
        //     System.DateTime.3.33밀리초의 정확성으로 값의 범위가 1753년 1월 1일에서 9999년 12월 31일까지인 날짜 및 시간
        //     데이터입니다.
        DateTime = 4,
        //
        // 요약:
        //     System.Decimal.-10 38 -1과 10 38 -1 사이의 고정 전체 자릿수 및 소수 자릿수 값입니다.
        Decimal = 5,
        //
        // 요약:
        //     System.Double.범위가 -1.79E +308에서 1.79E +308까지인 부동 소수점 숫자입니다.
        Float = 6,
        //
        // 요약:
        //     System.Byte 형식의 System.Array입니다.범위가 0바이트에서 2 31 -1(또는 2,147,483,647)바이트까지인 이진
        //     데이터의 가변 길이 스트림입니다.
        Image = 7,
        //
        // 요약:
        //     System.Int32.32비트 부호 있는 정수입니다.
        Int = 8,
        //
        // 요약:
        //     System.Decimal.정확성이 통화 단위의 10000분의 1이고 범위가 -2 63(또는 -922,337,203,685,477.5808)에서
        //     2 63 -1(또는 +922,337,203,685,477.5807)까지인 통화 값입니다.
        Money = 9,
        //
        // 요약:
        //     System.String.범위가 1자에서 4,000자까지인 유니코드 문자의 고정 길이 스트림입니다.
        NChar = 10,
        //
        // 요약:
        //     System.String.최대 길이가 2 30 - 1(또는 1,073,741,823)자인 유니코드 데이터의 가변 길이 스트림입니다.
        NText = 11,
        //
        // 요약:
        //     System.String.범위가 1자에서 4,000자까지인 유니코드 문자의 가변 길이 스트림입니다.문자열이 4,000자보다 더 큰 경우 암시적
        //     변환이 실패합니다.4,000자보다 더 긴 문자열로 작업할 경우 개체를 명시적으로 설정합니다.데이터 열이 nvarchar(max)일 경우 System.Data.SqlDbType.NVarChar를
        //     사용합니다.
        NVarChar = 12,
        //
        // 요약:
        //     System.Single.범위가 -3.40E +38에서 3.40E +38까지인 부동 소수점 숫자입니다.
        Real = 13,
        //
        // 요약:
        //     System.Guid.GUID(Globally Unique IDentifier)입니다.
        UniqueIdentifier = 14,
        //
        // 요약:
        //     System.DateTime.1분의 정확성으로 값의 범위가 1900년 1월 1일에서 2079년 6월 6일까지인 날짜 및 시간 데이터입니다.
        SmallDateTime = 15,
        //
        // 요약:
        //     System.Int16.16비트 부호 있는 정수입니다.
        SmallInt = 16,
        //
        // 요약:
        //     System.Decimal.통화 단위의 10000분의 1 정확성으로 범위가 -214,748.3648에서 +214,748.3647까지인 통화
        //     값입니다.
        SmallMoney = 17,
        //
        // 요약:
        //     System.String.최대 길이가 2 31 -1(또는 2,147,483,647)자이고 유니코드가 아닌 데이터의 가변 길이 스트림입니다.
        Text = 18,
        //
        // 요약:
        //     System.Byte 형식의 System.Array입니다.데이터베이스 내에서 고유한 자동 생성되는 이진 숫자입니다.timestamp는 일반적으로
        //     버전이 표시되는 테이블 행에 대한 메커니즘으로 사용됩니다.저장소 크기는 8바이트입니다.
        Timestamp = 19,
        //
        // 요약:
        //     System.Byte.8비트 부호 없는 정수입니다.
        TinyInt = 20,
        //
        // 요약:
        //     System.Byte 형식의 System.Array입니다.범위가 1바이트에서 8,000바이트까지인 이진 데이터의 가변 길이 스트림입니다.바이트
        //     배열이 8.000바이트보다 더 큰 경우 암시적 변환이 실패합니다.8.000바이트보다 더 큰 바이트 배열로 작업할 경우 개체를 명시적으로 설정합니다.
        VarBinary = 21,
        //
        // 요약:
        //     System.String.범위가 1문자에서 8,000문자까지인 비유니코드 문자의 가변 길이 스트림입니다.데이터 열이 varchar(max)일
        //     경우 System.Data.SqlDbType.VarChar를 사용합니다.
        VarChar = 22,
        //
        // 요약:
        //     System.Object.SQL Server 값 Empty 및 Null뿐만 아니라 숫자, 문자열, 이진 데이터 또는 날짜 데이터를 포함할
        //     수 있는 특수 데이터 형식으로 다른 데이터 형식이 선언되지 않으면 이 형식이 사용됩니다.
        Variant = 23,
        //
        // 요약:
        //     XML 값입니다.System.Data.SqlClient.SqlDataReader.GetValue(System.Int32) 메서드나 System.Data.SqlTypes.SqlXml.Value
        //     속성을 사용하여 XML을 문자열로 가져오거나 System.Data.SqlTypes.SqlXml.CreateReader 메서드를 호출하여 XML을
        //     System.Xml.XmlReader로 가져옵니다.
        Xml = 25,
        //
        // 요약:
        //     SQL Server 사용자 정의 형식(UDT)입니다.
        Udt = 29,
        //
        // 요약:
        //     테이블 반환 매개 변수에 들어 있는 구조적 데이터를 지정하기 위한 특수 데이터 형식입니다.
        Structured = 30,
        //
        // 요약:
        //     값 범위가 서기 1년 1월 1일에서 서기 9999년 12월 31일 사이인 날짜 데이터입니다.
        Date = 31,
        //
        // 요약:
        //     24시간제 시간 데이터입니다.Time 값 범위는 00:00:00부터 23:59:59.9999999까지이며 정확도는 100나노초입니다.SQL
        //     Server time 값에 해당합니다.
        Time = 32,
        //
        // 요약:
        //     날짜 및 시간 데이터입니다.날짜 값 범위는 서기 1년 1월 1일에서 서기 9999년 12월 31일 사이입니다.Time 값 범위는 00:00:00부터
        //     23:59:59.9999999까지이며 정확도는 100나노초입니다.
        DateTime2 = 33,
        //
        // 요약:
        //     표준 시간대를 고려한 날짜 및 시간 데이터입니다.날짜 값 범위는 서기 1년 1월 1일에서 서기 9999년 12월 31일 사이입니다.Time
        //     값 범위는 00:00:00부터 23:59:59.9999999까지이며 정확도는 100나노초입니다.표준 시간대 값의 범위는 -14:00에서 +14:00
        //     사이입니다.
        DateTimeOffset = 34
    }
}

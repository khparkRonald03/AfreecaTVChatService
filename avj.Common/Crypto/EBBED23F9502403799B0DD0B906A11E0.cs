using System;

namespace avj.Common
{
    /// <summary>
    /// 문자열 암/복호화
    /// </summary>
    public class EBBED23F9502403799B0DD0B906A11E0
    {
        /// <summary>
        /// 복호화
        /// </summary>
        /// <param name="FQSL"></param>
        /// <param name="EEQN"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        static public string EBBED23F9502403799B0DD0B906A11E0X(string FQSL, string EEQN, int i, int j)
        {
            //ex_ EBBED23F9502403799B0DD0B906A11E0.EBBED23F9502403799B0DD0B906A11E0X(string.Empty, INPUT.DBPassword, 0, 0));
            string value = string.Empty;
            try
            {
                value = CryptHelper.DecryptData(EEQN, new byte[] { 127, 64, 27, 71, 212, 121, 197, 62 });
            }
            catch (Exception)
            {
            }
            if (string.IsNullOrEmpty(value) == true) value = EEQN;
            return value;
        }

        /// <summary>
        /// 암호화
        /// </summary>
        /// <param name="FQSL"></param>
        /// <param name="EEQN"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        static public string EBBED23F9502403799B0DD0B906A11E0Y(string FQSL, string EEQN, int i, int j)
        {
            // ex_ EBBED23F9502403799B0DD0B906A11E0.EBBED23F9502403799B0DD0B906A11E0Y(string.Empty, item.Password, 0, 0);
            try
            {
                return CryptHelper.EncryptData(EEQN, new byte[] { 127, 64, 27, 71, 212, 121, 197, 62 });
            }
            catch (Exception)
            {
            }
            return EEQN;
        }
    }
}

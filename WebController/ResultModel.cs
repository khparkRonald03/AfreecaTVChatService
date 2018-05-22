using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebController
{
    /// <summary>
    /// 함수의 결과를 반환합니다.
    /// </summary>
    public class ResultModel<T>
    {
        /// <summary>
        /// 결과값
        /// </summary>
        public T ResultValue { get; set; }

        /// <summary>
        /// 실행 결과
        /// </summary>
        public bool ResultFlag { get; set; } = true;

        /// <summary>
        /// 에러
        /// </summary>
        private string err;
        public string Err
        {
            get
            {
                return err;
            }

            set
            {
                err = value;
                ResultFlag = false;
            }
        }
    }
}

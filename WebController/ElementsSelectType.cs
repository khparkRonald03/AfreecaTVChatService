using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebController
{
    /// <summary>
    /// 선택자 타입
    /// </summary>
    public enum ElementsSelectType
    {
        ClassName,
        CssSelector,
        Id,
        LinkText,
        Name,
        PartialLinkText,
        TagName,
        XPath,
    }
}

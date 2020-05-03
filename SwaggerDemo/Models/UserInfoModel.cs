using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerDemo.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoModel
    {
        /// <summary>
        /// 用户编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }
}

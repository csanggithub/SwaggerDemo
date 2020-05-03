using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SwaggerDemo.Models;

namespace SwaggerDemo.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="logger"></param>
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 测试方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                throw new Exception("异常消息");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "这是一个异常消息内容");
            }

            return Ok("OK");
        }

        /// <summary>
        /// 方法2
        /// </summary>
        /// <param name="postData">POST数据</param>
        /// <returns></returns>
        [HttpPost]
        public UserInfoDto Index2([FromBody] UserInfoModel postData)
        {
            var result = new UserInfoDto
            {
                Code = postData.Code,
                Name = postData.Name
            };
            return result;
        }
        /// <summary>
        /// 方法3
        /// </summary>
        /// <param name="postData">POST数据</param>
        /// <returns></returns>
        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<UserInfoDto> Index3([FromBody] UserInfoModel postData)
        {
            var result = new UserInfoDto
            {
                Code = postData.Code,
                Name = postData.Name
            };
            return result;
        }

        /// <summary>
        /// 方法4
        /// </summary>
        /// <param name="postData">POST数据</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<UserInfoDto> Index4([FromBody] UserInfoModel postData)
        {
            var result = new UserInfoDto
            {
                Code = postData.Code,
                Name = postData.Name
            };
            return result;
        }
    }
}
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerDemo.Services
{
    /// <summary>
    /// 定时任务
    /// </summary>
    public interface IHangfireServices
    {
        /// <summary>
        /// 任务1
        /// </summary>
        public void TaskOne();

        /// <summary>
        /// 任务2
        /// </summary>
        public void TaskTwo();
    }

    /// <summary>
    /// 定时任务
    /// </summary>
    public class HangfireServices: IHangfireServices
    {
        private readonly ILogger<HangfireServices> _logger;
        /// <summary>
        /// 定时任务
        /// </summary>
        /// <param name="logger"></param>
        public HangfireServices(ILogger<HangfireServices> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 任务1
        /// </summary>
        public void TaskOne()
        {
            _logger.LogDebug("十分钟执行一次");
        }
        /// <summary>
        /// 任务2
        /// </summary>
        public void TaskTwo()
        {
            _logger.LogDebug("一分钟执行一次");
        }
    }
}

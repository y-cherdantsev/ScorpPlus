using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ScorpPlusBackend.Services
{
    public class BackgroundAnalyzer
    {
        internal void Start()
        {
            var tasks = new List<Task> {CheckClimate(), CheckEmployeeAttendance(), CheckEmployeeTrustworthiness(), NotificationsQueueProceeding()};
        }

        private async Task CheckClimate()
        {
            var a = new int[]{};
            while (true)
            {
                await Task.Delay(10000);
                try
                {
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private async Task CheckEmployeeAttendance()
        {
            while (true)
            {
                await Task.Delay(60000);
                try
                {
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private async Task CheckEmployeeTrustworthiness()
        {
            while (true)
            {
                await Task.Delay(3600000);
                try
                {
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
        
        private async Task NotificationsQueueProceeding()
        {
            while (true)
            {
                await Task.Delay(5000);
                try
                {
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
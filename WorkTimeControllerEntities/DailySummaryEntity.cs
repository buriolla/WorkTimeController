using System;
using System.Collections.Generic;

namespace WorkTimeControllerEntities
{
    public class DailySummaryEntity
    {
        public List<ActionEntity> Notes { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan WorkTime { get; set; }
    }
}

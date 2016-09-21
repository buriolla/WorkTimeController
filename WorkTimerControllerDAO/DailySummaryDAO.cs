using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkTimeControllerEntities;

namespace WorkTimeControllerDAO
{
    public class DailySummaryDAO
    {
        public void Insert(DailySummaryEntity dailySummary)
        {
            SummaryDAO summaryDAO = new SummaryDAO();
            SummaryEntity summary = summaryDAO.GetSummary();
            if (summary == null)
                summary = new SummaryEntity();

            if (summary.SummaryList == null)
                summary.SummaryList = new List<DailySummaryEntity>();
            summary.SummaryList.Add(dailySummary);
            summaryDAO.Insert(summary);
        }

        public List<DailySummaryEntity> GetList()
        {
            List<DailySummaryEntity> result = new List<DailySummaryEntity>();

            SummaryDAO summaryDAO = new SummaryDAO();
            SummaryEntity summary = summaryDAO.GetSummary();
            
            if (summary == null)
            {
                summary = new SummaryEntity();
                summary.SummaryList = new List<DailySummaryEntity>();
            }

            result = summary.SummaryList;

            return result;
        }
    }
}

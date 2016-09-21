using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkTimeControllerEntities;
using WorkTimeControllerDAO;

namespace WorkTimeControllerBusiness
{
    public class DailySummaryBusiness
    {
        public void SaveDailySummary(List<ActionEntity> actionList, TimeSpan workTime)
        {
            DailySummaryEntity dailySummary = new DailySummaryEntity();
            dailySummary.Date = DateTime.Now;
            dailySummary.Notes = actionList;
            dailySummary.WorkTime = workTime;

            DailySummaryDAO dailySummaryDAO = new DailySummaryDAO();
            dailySummaryDAO.Insert(dailySummary);
        }

        public List<DailySummaryEntity> ListDailySummary()
        {
            DailySummaryDAO dao = new DailySummaryDAO();
            return dao.GetList();
        }
    }
}

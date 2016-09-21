using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkTimeControllerEntities;
using Newtonsoft.Json;

namespace WorkTimeControllerDAO
{
    public class SummaryDAO
    {
        public void Insert(SummaryEntity summary)
        {
            string serializesSummary = JsonConvert.SerializeObject(summary);
            DAOBase dao = new DAOBase();
            dao.WriteFile(serializesSummary);
        }

        public SummaryEntity GetSummary()
        {
            DAOBase dao = new DAOBase();
            string serializedSummary = dao.ReadFile();
            return JsonConvert.DeserializeObject<SummaryEntity>(serializedSummary);
        }
    }
}

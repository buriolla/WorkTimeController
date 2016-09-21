using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WorkTimeControllerBusiness;
using WorkTimeControllerEntities;

namespace WorkTimeController
{
    public partial class frmReport : Form
    {
        #region Constructor
        public frmReport()
        {
            InitializeComponent();
        } 
        #endregion

        #region Events
        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0)
                e.Value = ((DateTime)e.Value).ToString("dd/MM/yyyy");

            if (e.ColumnIndex == 1)
                e.Value = ((TimeSpan)e.Value).ToString(@"hh\:mm\:ss");
        }
        #endregion

        #region Methods
        private void BindGrid()
        {
            DailySummaryBusiness buss = new DailySummaryBusiness();
            List<DailySummaryEntity> dailySummaryList = buss.ListDailySummary();
            if (dailySummaryList != null && dailySummaryList.Count > 0)
            {
                dataGridView1.AutoGenerateColumns = false;
                BindingList<DailySummaryEntity> sourceList = new BindingList<DailySummaryEntity>(dailySummaryList);
                dataGridView1.DataSource = sourceList;
            }
            else
            {
                MessageBox.Show("Não existem lançamentos a serem exibidos.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        } 
        #endregion
    }
}

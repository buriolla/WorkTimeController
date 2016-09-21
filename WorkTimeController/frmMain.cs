using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WorkTimeControllerEntities;
using WorkTimeControllerBusiness;

namespace WorkTimeController
{
    public partial class frmMain : Form
    {
        #region Attributes
        KeyboardHook ActionShortCut = new KeyboardHook();
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        private List<ActionEntity> actionList;
        private BindingList<ActionEntity> sourceList;
        private TimeSpan workTime;
        private Timer stopwatch;
        private TimeSpan second = new TimeSpan(0, 0, 1);
        #endregion

        #region Constructor
        public frmMain()
        {
            InitializeComponent();

            RegisterHotKeys();
            SetUpContextMenu();
            SetUpTrayIcon();
            SetUpStopWatch();

            actionList = new List<ActionEntity>();
            lblWortTime.Text = "00:00:00";
        }
        #endregion

        #region Methods
        private void SetUpStopWatch()
        {
            stopwatch = new Timer();
            stopwatch.Interval = 1000;
            stopwatch.Tick += new EventHandler(stopwatch_Tick);
        } 

        private void SetUpTrayIcon()
        {
            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Work Time Controller";

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            trayIcon.DoubleClick += new EventHandler(trayIcon_DoubleClick);
            trayIcon.Icon = new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "stopwatch.ico"));
        }

        private void SetUpContextMenu()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exibir", OnShow);
            trayMenu.MenuItems.Add("Sair", OnExit);
        }

        private void RegisterHotKeys()
        {
            // register the event that is fired after the key press.
            ActionShortCut.KeyPressed +=
                new EventHandler<KeyPressedEventArgs>(ActionShortCut_KeyPressed);
            // register the control + alt + F12 combination as hot key.
            ActionShortCut.RegisterHotKey(WorkTimeController.ModifierKeys.Control | WorkTimeController.ModifierKeys.Alt,
                Keys.F10);
        }

        private void ControlVisibility(bool visibilityFlag)
        {
            Visible = visibilityFlag;
            ShowInTaskbar = visibilityFlag;
        }

        private void ShowForm()
        {
            ControlVisibility(true);
        }

        private void HideForm()
        {
            ControlVisibility(false);
        }

        private void BindGrid()
        {
            dataGridView1.AutoGenerateColumns = false;
            sourceList = new BindingList<ActionEntity>(actionList);
            dataGridView1.DataSource = sourceList;
        }

        private void SetWorkTimeText()
        {
            lblWortTime.Text = workTime.ToString(@"hh\:mm\:ss");
            
            if (workTime.Hours >= 8)
                lblWortTime.ForeColor = Color.Green;
            else
                lblWortTime.ForeColor = Color.Red;
        }

        private void SetupNewAction()
        {
            if (actionList != null)
            {
                if (actionList.Count > 0)
                {
                    int lastActionIndex = actionList.Count - 1;
                    ActionType type = actionList[lastActionIndex].Type;

                    if (type == ActionType.Start)
                    {
                        stopwatch.Stop();
                        actionList.Add(new ActionEntity(ActionType.Pause)
                        {
                            ActionTime = DateTime.Now.TimeOfDay
                        });
                    }

                    if (type == ActionType.Pause)
                    {
                        stopwatch.Start();
                        actionList.Add(new ActionEntity(ActionType.Start)
                        {
                            ActionTime = DateTime.Now.TimeOfDay
                        });
                    }
                }
                else
                {
                    stopwatch.Start();
                    actionList.Add(new ActionEntity(ActionType.Start)
                    {
                        ActionTime = DateTime.Now.TimeOfDay
                    });
                }
            }
        }

        private void CleanForm()
        {
            workTime = new TimeSpan();
            actionList = new List<ActionEntity>();
            sourceList = new BindingList<ActionEntity>();
            stopwatch.Stop();
            BindGrid();
            SetWorkTimeText();
        }
        #endregion

        #region Events
        private void ActionShortCut_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            SetupNewAction();
            BindGrid();
            frmSobre form = new frmSobre();
            form.ShowDialog();
        }

        protected override void OnLoad(EventArgs e)
        {
            HideForm();
            base.OnLoad(e);
        }

        private void stopwatch_Tick(object sender, EventArgs e)
        {
            workTime = workTime.Add(second);
            SetWorkTimeText();
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnShow(object sender, EventArgs e)
        {
            ShowForm();
            WindowState = FormWindowState.Normal;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                HideForm();
            }
        }

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            HideForm();
        }
        
        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEncerrar_Click(object sender, EventArgs e)
        {
            actionList.Add(new ActionEntity(ActionType.Finish) {
                ActionTime = DateTime.Now.TimeOfDay
            });

            DailySummaryBusiness buss = new DailySummaryBusiness();
            buss.SaveDailySummary(actionList, workTime);

            CleanForm();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void retalórioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReport formReport = new frmReport();
            formReport.ShowDialog();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 1)
                e.Value = ((TimeSpan)e.Value).ToString(@"hh\:mm\:ss");
        }

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSobre form = new frmSobre();
            form.ShowDialog();
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            if (isDisposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(isDisposing);
        } 
        #endregion
    }
}

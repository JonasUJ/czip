using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace czip
{
    public partial class InfoMonitor : Form
    {
        public InfoMonitor()
        {
            InitializeComponent();
            ConsoleUtil.MessageEvent += WriteMessage;
            ConsoleUtil.InfoEvent += WriteInfo;
            ConsoleUtil.WarningEvent += WriteWarning;
            ConsoleUtil.ErrorEvent += WriteError;
        }

        public void AppendText(string text, Color color)
        {
            rtb_printArea.SelectionStart = rtb_printArea.TextLength;
            rtb_printArea.SelectionLength = 0;
            rtb_printArea.SelectionColor = color;
            rtb_printArea.AppendText(text + "\n");
            rtb_printArea.SelectionColor = rtb_printArea.ForeColor;
        }

        private void WriteMessage(object sender, MessageEventArgs e) 
        {
            if (cb_message.Checked)
                rtb_printArea.AppendText(e.Message + "\n");
        }

        private void WriteInfo(object sender, InfoEventArgs e)
        {
            if (cb_info.Checked)
                AppendText("Info: " + e.Message, Color.DeepSkyBlue);
        }

        private void WriteWarning(object sender, WarningEventArgs e)
        {
            if (cb_warning.Checked)
                AppendText("Warning: " + e.Message, Color.Orange);
        }

        private void WriteError(object sender, ErrorEventArgs e)
        {
            if (cb_error.Checked)
                AppendText("Error: " + e.Message, Color.Red);
        }
    }
}

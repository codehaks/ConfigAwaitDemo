using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var t1 = Thread.CurrentThread.ManagedThreadId;
            await DownloadAsyncWithProgressbar();
            var t2 = Thread.CurrentThread.ManagedThreadId;

        }

        private async Task DownloadAsyncWithProgressbar()
        {
            
            for (int i = 0; i < 11; i++)
            {

                await Task.Delay(300);
                progressBar1.Value = 10 * i;
                var t1 = Thread.CurrentThread.ManagedThreadId;


            }
            MessageBox.Show("Download completed!");
        }

    }
}

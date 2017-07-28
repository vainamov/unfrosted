﻿using System.Linq;
using System.Windows.Forms;
using Unfrosted.Core;
using Unfrosted.Network;
using Unfrosted.Transfering;

namespace Unfrosted.Forms
{
    public partial class MainWindow : Form
    {
        public MainWindow() {
            InitializeComponent();

            this.Closing += OnClosing;

            foreach (ToolStripMenuItem item in mstMain.Items) {
                ((ToolStripDropDownMenu) item.DropDown).ShowImageMargin = false;
                item.Padding = new Padding(4);
            }
            mstMain.Renderer = new ToolStripProfessionalRenderer(new WhiteColorTable());

            for (var i = 50000; i < 50020; i++) {
                tsmiTransfers.DropDownItems.Add($"42% - defectively-min.zip (10.0.115.3) [:{i}]");
            }
            
            tsmiReloadPool.Click += (sender, args) => {
                PoolService.Instance.Reload();
                lsbPoolMembers.Items.Clear();
                lsbPoolMembers.Items.AddRange(PoolService.Instance.Members.Select(m => (object) m).ToArray());
            };

            tsmiNewTransfer.Click += OnTsmiNewTransferClick;

            lsbPoolMembers.DoubleClick += OnLsbPoolMembersDoubleClick;

            button1.Click += OnClick;
        }

        private void OnClick(object sender, System.EventArgs e) {
            TransferManager.Instance.CreateNewTransfer(new Transfer {
                FileName = "vainamo-unfrosted-image.vmdkx",
                FileSizeBytes = 86413L,
                SenderAddress = "127.0.0.1",
                ReceiverAddress = "localhost"
            });
        }

        private void OnLsbPoolMembersDoubleClick(object sender, System.EventArgs e) {
            if (lsbPoolMembers.SelectedItem != null) {
                var dialog = new CreateTransferDialog();
                if (dialog.ShowDialog(lsbPoolMembers.SelectedItem.ToString(), 50043) == DialogResult.OK) {
                    var transfer = dialog.Transfer;
                }
            }
        }

        private void OnTsmiNewTransferClick(object sender, System.EventArgs e) {
            var dialog = new CreateTransferDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                var transfer = dialog.Transfer;
            }
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e) {
            PoolService.Instance.StopService();
            MetaService.Instance.StopService();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unfrosted.Forms;
using Unfrosted.Network;

namespace Unfrosted.Transfering
{
    public class TransferManager
    {
        public static TransferManager Instance { get; set; } = new TransferManager();

        public MainWindow Owner { get; set; }

        public List<Transfer> Transfers { get; set; } = new List<Transfer>();
        public List<TransferController> Controllers { get; set; } = new List<TransferController>();

        public void CreateNewTransfer(Transfer transfer) {
            MetaService.Instance.SendMeta(transfer.ReceiverAddress, Configuration.Instance.MetaPort, transfer);
            Transfers.Add(transfer);
        }

        public void ShowTransferPrompt(Transfer transfer) {
            transfer.Port = PortController.Instance.GetBestPort();

            Owner.Invoke(new Action(() => {
                var accept = MessageBox.Show(Owner, $"{transfer.SenderAddress} wants to share a file with you.\n\n{transfer.FileName} ({transfer.FileSizeBytes / 1024}KB)\n\nDo you want to receive this file?", "unfrosted", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                MetaService.Instance.SendMetaResult(transfer.SenderAddress, Configuration.Instance.MetaPort, transfer, accept);
                if (accept) {
                    PortController.Instance.AssignController(new TransferController(transfer));
                }
            }));
        }

        public void StartTransfer(uint id, int port) {
            var transfer = Transfers.Find(t => t.Id == id);
            transfer.Port = port;
            var controller = new TransferController(transfer);
            transfer.Controller = controller;

            controller.StartTransfer();

            Owner.AddTransfer(transfer);
            Transfers.Remove(transfer);
        }

        public void CancelTransfer(uint id) {
            Transfers.RemoveAll(t => t.Id == id);
        }
    }
}

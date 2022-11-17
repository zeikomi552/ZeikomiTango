using ClosedXML.Excel;
using Microsoft.Win32;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZeikomiTango.Models;

namespace ZeikomiTango.ViewModels
{
    internal class MainWindowVM : ViewModelBase
    {
        #region 単語コレクション[TangoCollection]プロパティ
        /// <summary>
        /// 単語コレクション[TangoCollection]プロパティ用変数
        /// </summary>
        TangoCollectionM _TangoCollection = new TangoCollectionM();
        /// <summary>
        /// 単語コレクション[TangoCollection]プロパティ
        /// </summary>
        public TangoCollectionM TangoCollection
        {
            get
            {
                return _TangoCollection;
            }
            set
            {
                if (_TangoCollection == null || !_TangoCollection.Equals(value))
                {
                    _TangoCollection = value;
                    NotifyPropertyChanged("TangoCollection");
                }
            }
        }
        #endregion



        public override void Init(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }

        public override void Close(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }

        public void ReadExcel()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "単語帳エクセルファイル (*.xlsx)|*.xlsx";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    var workbook = new XLWorkbook(dialog.FileName);
                    var sheet = workbook.Worksheets.ElementAt(0);

                    int row = 2;
                    string value = string.Empty;

                    // 値を確認
                    while ((value = sheet.Cell($"A{row}").Value.ToString()!) != string.Empty)
                    {
                        string question = value;
                        string answer = sheet.Cell($"B{row}").Value.ToString()!;
                        string explain = sheet.Cell($"C{row}").Value.ToString()!;
                        string selection_a = sheet.Cell($"D{row}").Value.ToString()!;
                        string selection_b = sheet.Cell($"E{row}").Value.ToString()!;
                        string selection_c = sheet.Cell($"F{row}").Value.ToString()!;
                        string selection_d = sheet.Cell($"G{row}").Value.ToString()!;
                        this.TangoCollection.TangoList.Items!.Add(new TangoM(question, explain, selection_a, selection_b, selection_c, selection_d, answer));

                        row++;
                    }

                    this.TangoCollection.SelectFirst();
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
    }
}

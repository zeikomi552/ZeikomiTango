using ClosedXML.Excel;
using Microsoft.Win32;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using MVVMCore.Common.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                var ctrl = sender as Grid;
                var wnd = VisualTreeHelperWrapper.GetWindow<MainWindow>(sender) as MainWindow;

                EnableDragDrop(wnd!.DisplayArea);
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


      　private void EnableDragDrop(FrameworkElement control)
        {
            //ドラッグ＆ドロップを受け付けられるようにする
            control.AllowDrop = true;

            //ドラッグが開始された時のイベント処理（マウスカーソルをドラッグ中のアイコンに変更）
            control.PreviewDragOver += (s, e) =>
            {
                //ファイルがドラッグされたとき、カーソルをドラッグ中のアイコンに変更し、そうでない場合は何もしない。
                e.Effects = (e.Data.GetDataPresent(DataFormats.FileDrop)) ? DragDropEffects.Copy : e.Effects = DragDropEffects.None;
                e.Handled = true;
            };

            //ドラッグ＆ドロップが完了した時の処理（ファイル名を取得し、ファイルの中身をTextプロパティに代入）
            control.PreviewDrop += (s, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop)) // ドロップされたものがファイルかどうか確認する。
                {
                    string[] paths = ((string[])e.Data.GetData(DataFormats.FileDrop));
                    //--------------------------------------------------------------------
                    // ここに、ドラッグ＆ドロップ受付時の処理を記述する
                    //--------------------------------------------------------------------

                    ReadExcel(paths.FirstOrDefault()!);
                }
            };

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
                    ReadExcel(dialog.FileName); // ファイルの読み込み
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        private void ReadExcel(string path)
        {
            string ext = System.IO.Path.GetExtension(path); //extには".jpg"が代入されます。

            if (!string.IsNullOrEmpty(path) || path.ToLower().Equals(".xlsx"))
            {
                var workbook = new XLWorkbook(path);
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
                    this.TangoCollection.Add(new TangoM(question, explain, selection_a, selection_b, selection_c, selection_d, answer));

                    row++;
                }

                this.TangoCollection.SelectFirst();
            }
        }
    }
}

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
using ZeikomiTango.Views;

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

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        #endregion

        #region 閉じる処理
        /// <summary>
        /// 閉じる処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        #endregion

        #region ドラッグ&ドロップでエクセルファイルの読み込み
        /// <summary>
        /// ドラッグ&ドロップでエクセルファイルの読み込み
        /// </summary>
        /// <param name="control"></param>
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

                    this.TangoCollection.ReadExcel(paths.FirstOrDefault()!);
                }
            };

        }
        #endregion

        #region エクセルの読み込み処理
        /// <summary>
        /// エクセルの読み込み処理
        /// </summary>
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
                    this.TangoCollection.ReadExcel(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion

        #region キー入力処理の受付
        /// <summary>
        /// キー入力処理の受付
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void KeyDown(object sender, EventArgs e)
        {
            try
            {
                var key_eve = e as KeyEventArgs;

                if (key_eve != null)
                {
                    if (key_eve.Key == Key.Right)
                    {
                        key_eve.Handled = true;
                        this.TangoCollection.ChangeDisplay(true);
                    }
                    else if (key_eve.Key == Key.Left)
                    {
                        key_eve.Handled = true;
                        this.TangoCollection.ChangeDisplay(false);
                    }
                    else
                    {
                        ;
                    }
                }
            }
            catch (Exception ev)
            {
                ShowMessage.ShowErrorOK(ev.Message, "Error");
            }
        }
        #endregion

        #region 編集画面を開く
        /// <summary>
        /// 編集画面を開く
        /// </summary>
        public void OpenEditWindow()
        {
            try
            {
                var wnd = new EditWindowV();
                var vm = wnd.DataContext as EditWindowVM;

                if (vm != null)
                {
                    this.TangoCollection.IsAuto = false;
                    vm.TangoCollection = this.TangoCollection;

                    if (wnd.ShowDialog() == true)
                    {

                    }
                    this.TangoCollection = vm.TangoCollection;
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
        #endregion
    }
}

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
using ZeikomiTango.Models;

namespace ZeikomiTango.ViewModels
{
    internal class EditWindowVM : ViewModelBase
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

        public void NewCreate()
        {
            try
            {
                this.TangoCollection.TangoList.Items.Add(new TangoM());
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }

        public void Save()
        {
            try
            {
                // ダイアログのインスタンスを生成
                var dialog = new SaveFileDialog();

                // ファイルの種類を設定
                dialog.Filter = "エクセルファイル (*.xlsx)|*.xlsx";

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    this.TangoCollection.SaveExcel(dialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ShowErrorOK(ex.Message, "Error");
            }
        }
    }
}

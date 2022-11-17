using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeikomiTango.Models
{
    public enum DisplayType
    {
        Question,
        Answer
    }



    public class TangoCollectionM : ModelBase
    {

        DisplayType _DisplayType = DisplayType.Question;

        #region 表示[Display]プロパティ
        /// <summary>
        /// 表示[Display]プロパティ用変数
        /// </summary>
        string _Display = string.Empty;
        /// <summary>
        /// 表示[Display]プロパティ
        /// </summary>
        public string Display
        {
            get
            {
                return _Display;
            }
            set
            {
                if (_Display == null || !_Display.Equals(value))
                {
                    _Display = value;
                    NotifyPropertyChanged("Display");
                }
            }
        }
        #endregion

        #region 自動フラグ[IsAuto]プロパティ
        /// <summary>
        /// 自動フラグ[IsAuto]プロパティ用変数
        /// </summary>
        bool _IsAuto = false;
        /// <summary>
        /// 自動フラグ[IsAuto]プロパティ
        /// </summary>
        public bool IsAuto
        {
            get
            {
                return _IsAuto;
            }
            set
            {
                if (!_IsAuto.Equals(value))
                {
                    _IsAuto = value;
                    NotifyPropertyChanged("IsAuto");
                }
            }
        }
        #endregion

        #region タイマーインターバル(秒)[Interval]プロパティ
        /// <summary>
        /// タイマーインターバル(秒)[Interval]プロパティ用変数
        /// </summary>
        int _Interval = 5;
        /// <summary>
        /// タイマーインターバル(秒)[Interval]プロパティ
        /// </summary>
        public int Interval
        {
            get
            {
                return _Interval;
            }
            set
            {
                if (!_Interval.Equals(value))
                {
                    _Interval = value;
                    NotifyPropertyChanged("Interval");
                }
            }
        }
        #endregion


        /// <summary>
        /// 表示タイプの切り替え
        /// </summary>
        public void ChangeDisplayType()
        {
            this._DisplayType = this._DisplayType == DisplayType.Question ? DisplayType.Answer : DisplayType.Question;
        }

        /// <summary>
        /// 表示切替
        /// </summary>
        /// <param name="next_f">true:次へ false:前へ</param>
        public void ChangeDisplay(bool next_f)
        {
            int index = GetIndex();

            if (next_f) // 次へ移動
            {
                if (this.TangoList.Items.Count > 0 && this.TangoList.SelectedItem != null)
                {
                    if (index >= 0 && (this.TangoList.Items.Count - 1 > index || this._DisplayType == DisplayType.Question) )
                    {
                        ChangeDisplayType();    // 表示タイプの切替
                        if (this._DisplayType == DisplayType.Question)
                        {
                            this.TangoList.SelectedItem = this.TangoList.Items.ElementAt(index + 1);    // 要素の切替
                            this.Display = this.TangoList.SelectedItem.DisplayQuestion;
                        }
                        else
                        {
                            this.Display = this.TangoList.SelectedItem.DisplayAnswer;
                        }
                    }
                }
            }
            else
            {
                if (this.TangoList.Items.Count > 0 && this.TangoList.SelectedItem != null)
                {
                    if (index > 0 || _DisplayType == DisplayType.Answer)
                    {
                        ChangeDisplayType();    // 表示タイプの切替
                        if (this._DisplayType == DisplayType.Answer)
                        {
                            this.TangoList.SelectedItem = this.TangoList.Items.ElementAt(index - 1);
                            this.Display = this.TangoList.SelectedItem.DisplayAnswer;
                        }
                        else
                        {
                            this.Display = this.TangoList.SelectedItem.DisplayQuestion;
                        }
                    }
                }
            }
        }

        #region 単語リスト[TangoList]プロパティ
        /// <summary>
        /// 単語リスト[TangoList]プロパティ用変数
        /// </summary>
        ModelList<TangoM> _TangoList = new ModelList<TangoM>();
        /// <summary>
        /// 単語リスト[TangoList]プロパティ
        /// </summary>
        public ModelList<TangoM> TangoList
        {
            get
            {
                return _TangoList;
            }
            set
            {
                if (_TangoList == null || !_TangoList.Equals(value))
                {
                    _TangoList = value;
                    NotifyPropertyChanged("TangoList");
                }
            }
        }
        #endregion

        /// <summary>
        /// 最初の項目を選択する
        /// </summary>
        public void SelectFirst()
        {
            if (this.TangoList.Items.Count > 0)
            {
                this.TangoList.SelectedItem = this.TangoList.Items.First();
                this._DisplayType = DisplayType.Question;   // 表示タイプを問題文に変更
                this.Display = this.TangoList.SelectedItem.DisplayQuestion;
            }
        }

        /// <summary>
        /// 最後の項目を選択する
        /// </summary>
        public void SelectLast()
        {
            if (this.TangoList.Items.Count > 0)
            {
                this.TangoList.SelectedItem = this.TangoList.Items.Last();
                this._DisplayType = DisplayType.Question;   // 表示タイプを問題文に変更
                this.Display = this.TangoList.SelectedItem.DisplayQuestion;
            }
        }

        /// <summary>
        /// 次のアイテムを選択する
        /// </summary>
        public void SelectNext()
        {
            ChangeDisplay(true);
        }

        /// <summary>
        /// 前のアイテムを選択する
        /// </summary>
        public void SelectPrev()
        {
            ChangeDisplay(false);
        }

        /// <summary>
        /// インデックスの取得処理
        /// </summary>
        /// <returns>インデックス</returns>
        public int GetIndex()
        {
            if (this.TangoList.SelectedItem != null)
            {
                return this.TangoList.Items.IndexOf(this.TangoList.SelectedItem);
            }
            else
            {
                return -1;
            }
        }

        public void Auto()
        {
            Task.Run(() =>
            {
                while (this.IsAuto)
                {
                    System.Threading.Thread.Sleep(this.Interval * 1000);
                    this.SelectNext();
                }
            });
        }
    }
}

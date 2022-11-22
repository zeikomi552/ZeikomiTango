using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Presentation;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ZeikomiTango.Models
{
    public enum DisplayType
    {
        Question,
        Answer
    }

    public class TangoCollectionM : ModelBase
    {
        #region 表示タイプ
        /// <summary>
        /// 表示タイプ
        /// </summary>
        DisplayType _DisplayType = DisplayType.Question;
        #endregion

        #region タイマー用インスタンス
        /// <summary>
        /// タイマー用インスタンス
        /// </summary>
        private DispatcherTimer _Timer;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TangoCollectionM()
        {
            // タイマのインスタンスを生成
            _Timer = new DispatcherTimer();                         // 優先度はDispatcherPriority.Background
            _Timer.Interval = new TimeSpan(0, 0, this.Interval);    // インターバルを設定
            _Timer.Tick += new EventHandler(TimerMethod!);          // タイマメソッドを設定
        }
        #endregion

        #region 表示[Display]プロパティ
        /// <summary>
        /// 表示[Display]プロパティ用変数
        /// </summary>
        string _Display = "ファイルをドロップ";
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
                if (!_Interval.Equals(value) && value > 0)
                {
                    _Interval = value;
                    this._Timer.Interval = new TimeSpan(0, 0, value);
                    NotifyPropertyChanged("Interval");
                }
            }
        }
        #endregion

        #region 繰り返し[IsRepeat]プロパティ
        /// <summary>
        /// 繰り返し[IsRepeat]プロパティ用変数
        /// </summary>
        bool _IsRepeat = false;
        /// <summary>
        /// 繰り返し[IsRepeat]プロパティ
        /// </summary>
        public bool IsRepeat
        {
            get
            {
                return _IsRepeat;
            }
            set
            {
                if (!_IsRepeat.Equals(value))
                {
                    _IsRepeat = value;
                    NotifyPropertyChanged("IsRepeat");
                }
            }
        }
        #endregion

        #region ランダム[IsRandom]プロパティ
        /// <summary>
        /// ランダム[IsRandom]プロパティ用変数
        /// </summary>
        bool _IsRandom = false;
        /// <summary>
        /// ランダム[IsRandom]プロパティ
        /// </summary>
        public bool IsRandom
        {
            get
            {
                return _IsRandom;
            }
            set
            {
                if (!_IsRandom.Equals(value))
                {
                    _IsRandom = value;
                    NotifyPropertyChanged("IsRandom");
                }
            }
        }
        #endregion

        #region フォントサイズ[FontSize]プロパティ
        /// <summary>
        /// フォントサイズ[FontSize]プロパティ用変数
        /// </summary>
        int _FontSize = 25;
        /// <summary>
        /// フォントサイズ[FontSize]プロパティ
        /// </summary>
        public int FontSize
        {
            get
            {
                return _FontSize;
            }
            set
            {
                if (!_FontSize.Equals(value) && _FontSize > 0)
                {
                    _FontSize = value;
                    NotifyPropertyChanged("FontSize");
                }
            }
        }
        #endregion

        #region 表示タイプの切り替え
        /// <summary>
        /// 表示タイプの切り替え
        /// </summary>
        public void ChangeDisplayType()
        {
            this._DisplayType = this._DisplayType == DisplayType.Question ? DisplayType.Answer : DisplayType.Question;
        }
        #endregion

        #region 表示切替
        /// <summary>
        /// 表示切替
        /// </summary>
        /// <param name="next_f">true:次へ false:前へ</param>
        public void ChangeDisplay(bool next_f)
        {

            // ファイルが読み込まれている状態
            if (this.TangoList.Items.Count > 0 && this.TangoList.SelectedItem != null)
            {
                // インデックスの取得
                int index = GetIndex();

                if (next_f) // 次へ移動
                {
                    ChangeDisplayType();    // 表示タイプの切替
                    if (this._DisplayType == DisplayType.Question)  // 質問表示状態
                    {
                        int next_index = GetNextIndex(index);
                        if (next_index < 0)
                            return;

                        this.TangoList.SelectedItem = this.TangoList.Items.ElementAt(next_index);    // 要素の切替
                        this.Display = this.TangoList.SelectedItem.DisplayQuestion;                 // 質問のセット
                    }
                    else
                    {
                        this.Display = this.TangoList.SelectedItem.DisplayAnswer;                   // 回答のセット
                    }
                }
                else
                {
                    ChangeDisplayType();    // 表示タイプの切替
                    if (this._DisplayType == DisplayType.Answer)    // 回答表示状態
                    {
                        int next_index = GetPrevIndex(index);
                        if (next_index < 0)
                            return;

                        this.TangoList.SelectedItem = this.TangoList.Items.ElementAt(next_index);    // 要素の切替
                        this.Display = this.TangoList.SelectedItem.DisplayAnswer;                   // 回答のセット
                    }
                    else
                    {
                        this.Display = this.TangoList.SelectedItem.DisplayQuestion;                 // 質問のセット
                    }
                }
            }
        }
        #endregion

        public void SelectItem(int index)
        {
            this.TangoList.SelectedItem = this.TangoList.Items.ElementAt(index);    // 要素の切替
            this.Display = this.TangoList.SelectedItem.DisplayQuestion;                 // 質問のセット
            this._DisplayType = DisplayType.Question;
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

        #region 最初の項目を選択する
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
        #endregion

        #region 最後の項目を選択する
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
        #endregion

        Random _Rand = new Random();
        #region 次のアイテムを選択する
        /// <summary>
        /// 次のアイテムを選択する
        /// </summary>
        public void SelectNext()
        {
            if (this.IsRandom)
            {
                if(this._DisplayType == DisplayType.Answer)
                {
                    var index = _Rand.Next(0, this.TangoList.Items.Count);
                    SelectItem(index);  // アイテムの選択
                }
                else
                {
                    ChangeDisplay(true);
                }
            }
            else
            {
                ChangeDisplay(true);
            }
        }
        #endregion

        #region 前のアイテムを選択する
        /// <summary>
        /// 前のアイテムを選択する
        /// </summary>
        public void SelectPrev()
        {
            ChangeDisplay(false);
        }
        #endregion

        #region インデックスの取得処理
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
        #endregion

        public int GetNextIndex(int current_index)
        {
            int count = this.TangoList.Count;

            while (true)
            {
                current_index++;    // 一つインデックスをずらす

                if (this.IsRepeat && current_index >= count)
                {
                    current_index = 0;
                }
                else if (current_index >= count)
                {
                    break;
                }

                if (this.TangoList.ElementAt(current_index).IsDisplay)
                {
                    return current_index;
                }
            }

            return -1;
        }

        public int GetPrevIndex(int current_index)
        {
            int count = this.TangoList.Count;

            while (true)
            {
                current_index--;    // 一つインデックスをずらす

                if (this.IsRepeat && current_index < 0)
                {
                    current_index = count - 1;
                }
                else if (current_index < 0)
                {
                    break;
                }

                if (this.TangoList.ElementAt(current_index).IsDisplay)
                {
                    return current_index;
                }
            }

            return -1;
        }

        #region タイマーメソッド
        /// <summary>
        /// タイマーメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerMethod(object sender, EventArgs e)
        {
            this.SelectNext();
        }
        #endregion

        #region 自動実行
        /// <summary>
        /// 自動実行
        /// </summary>
        public void Auto()
        {
            if (this.IsAuto)
            {
                _Timer.Start(); // タイマー開始
            }
            else
            {
                _Timer.Stop();  // タイマー終了
            }
        }
        #endregion

        #region タイマーを停止
        /// <summary>
        /// タイマーを停止
        /// </summary>
        public void StopTimer()
        {
            _Timer.Stop();
        }
        #endregion
    }
}

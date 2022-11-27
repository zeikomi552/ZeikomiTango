using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Presentation;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using MVVMCore.Common.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
        Random _Rand = new Random();

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

        #region ファイルパス[FilePath]プロパティ
        /// <summary>
        /// ファイルパス[FilePath]プロパティ用変数
        /// </summary>
        string _FilePath = string.Empty;
        /// <summary>
        /// ファイルパス[FilePath]プロパティ
        /// </summary>
        public string FilePath
        {
            get
            {
                return _FilePath;
            }
            set
            {
                if (_FilePath == null || !_FilePath.Equals(value))
                {
                    _FilePath = value;
                    NotifyPropertyChanged("FilePath");
                }
            }
        }
        #endregion

        #region 選択要素
        /// <summary>
        /// 選択要素
        /// </summary>
        public TangoM SelectedItem
        {
            get
            {
                return this.TangoList.SelectedItem;
            }
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
        int _Interval = 4;
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

                NotifyPropertyChanged("CurrentIndex");
                NotifyPropertyChanged("CurrentPage");
            }
        }
        #endregion

        #region アイテムの選択処理
        /// <summary>
        /// アイテムの選択処理
        /// </summary>
        /// <param name="index">インデックス</param>
        public void SelectItem(int index)
        {
            this.TangoList.SelectedItem = this.TangoList.Items.ElementAt(index);    // 要素の切替
            this.Display = this.TangoList.SelectedItem.DisplayQuestion;                 // 質問のセット
            this._DisplayType = DisplayType.Question;
            NotifyPropertyChanged("CurrentIndex");
            NotifyPropertyChanged("CurrentPage");
        }
        #endregion

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
                NotifyPropertyChanged("CurrentIndex");
                NotifyPropertyChanged("CurrentPage");
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
                NotifyPropertyChanged("CurrentIndex");
                NotifyPropertyChanged("CurrentPage");
            }
        }
        #endregion

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

        #region 現在のインデックス
        /// <summary>
        /// 現在のインデックス
        /// </summary>
        public int CurrentIndex
        {
            get
            {
                return GetIndex();
            }
        }
        #endregion

        #region 現在のページ番号
        /// <summary>
        /// 現在のページ番号
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return GetIndex() + 1;
            }
        }
        #endregion

        #region リストのサイズ
        /// <summary>
        /// リストのサイズ
        /// </summary>
        public int Count
        {
            get
            {
                return this.TangoList.Count;
            }
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

        #region 次のインデックスを取得
        /// <summary>
        /// 次のインデックスを取得
        /// </summary>
        /// <param name="current_index">現在のインデックス</param>
        /// <returns>次のインデックス</returns>
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
        #endregion

        #region 直前のインデックス取得
        /// <summary>
        /// 直前のインデックス取得
        /// </summary>
        /// <param name="current_index">現在のインデックス</param>
        /// <returns>直前のインデックス</returns>
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
        #endregion

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

        #region 要素を追加する
        /// <summary>
        /// 要素を追加する
        /// </summary>
        /// <param name="item">追加要素</param>
        public void Add(TangoM item)
        {
            this.TangoList.Items.Add(item);
            NotifyPropertyChanged("Count");
        }
        #endregion

        #region 要素の削除
        /// <summary>
        /// 要素の削除
        /// </summary>
        public void Clear()
        {
            this.TangoList.Items.Clear();
            NotifyPropertyChanged("Count");
        }
        #endregion

        #region エクセルファイルの読み込み処理
        /// <summary>
        /// エクセルファイルの読み込み処理
        /// </summary>
        /// <param name="execl_file_path">エクセルファイルパス</param>
        public void ReadExcel(string execl_file_path)
        {
            string ext = System.IO.Path.GetExtension(execl_file_path);

            if (!string.IsNullOrEmpty(execl_file_path) || ext.ToLower().Equals(".xlsx"))
            {
                this.FilePath = execl_file_path;
                FileStream fs = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                using (var workbook = new XLWorkbook(fs, XLEventTracking.Disabled))
                {
                    var sheet = workbook.Worksheets.ElementAt(0);

                    int row = 2;
                    string value = string.Empty;
                    this.Clear();

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
                        this.Add(new TangoM(question, explain, selection_a, selection_b, selection_c, selection_d, answer));

                        row++;
                    }

                    this.SelectFirst();
                }
            }
        }
        #endregion

        #region エクセルファイルの保存処理
        /// <summary>
        /// エクセルファイルの保存処理
        /// </summary>
        /// <param name="execl_file_path">エクセルファイルパス</param>
        public void SaveExcel(string execl_file_path)
        {
            string ext = System.IO.Path.GetExtension(execl_file_path);

            if (!string.IsNullOrEmpty(execl_file_path) || ext.ToLower().Equals(".xlsx"))
            {
                this.FilePath = execl_file_path;

                using (var workbook = new XLWorkbook(XLEventTracking.Disabled))
                {
                    var sheet = workbook.Worksheets.Add("単語帳");

                    int row = 2;
                    string value = string.Empty;

                    sheet.Cell($"A1").Value = "Question";
                    sheet.Cell($"B1").Value = "Answer";
                    sheet.Cell($"C1").Value = "Explanation";
                    sheet.Cell($"D1").Value = "A";
                    sheet.Cell($"E1").Value = "B";
                    sheet.Cell($"F1").Value = "C";
                    sheet.Cell($"G1").Value = "D";
                    foreach (var tango in this.TangoList.Items)
                    {
                        sheet.Cell($"A{row}").Value = tango.Querstion.ToString();
                        sheet.Cell($"B{row}").Value = tango.Answer.ToString();
                        sheet.Cell($"C{row}").Value = tango.Explanation.ToString();
                        sheet.Cell($"D{row}").Value = tango.Selections[0].ToString();
                        sheet.Cell($"E{row}").Value = tango.Selections[1].ToString();
                        sheet.Cell($"F{row}").Value = tango.Selections[2].ToString();
                        sheet.Cell($"G{row}").Value = tango.Selections[3].ToString();
                        row++;
                    }

                    workbook.SaveAs(execl_file_path);
                }
            }
        }
        #endregion
    }
}

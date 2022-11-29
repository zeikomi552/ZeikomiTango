using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Presentation;
using MaterialDesignColors.Recommended;
using Microsoft.Win32;
using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using MVVMCore.Common.Wrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZeikomiTango.Models;
using ZeikomiTango.Views;

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

        #region インストールされている音声リスト[VoiceList]プロパティ
        /// <summary>
        /// インストールされている音声リスト[VoiceList]プロパティ用変数
        /// </summary>
        ModelList<InstalledVoice> _VoiceList = new ModelList<InstalledVoice>();
        /// <summary>
        /// インストールされている音声リスト[VoiceList]プロパティ
        /// </summary>
        public ModelList<InstalledVoice> VoiceList
        {
            get
            {
                return _VoiceList;
            }
            set
            {
                if (_VoiceList == null || !_VoiceList.Equals(value))
                {
                    _VoiceList = value;
                    NotifyPropertyChanged("VoiceList");
                }
            }
        }
        #endregion



        #region 音声発生中[IsVoice]プロパティ
        /// <summary>
        /// 音声発生中[IsVoice]プロパティ用変数
        /// </summary>
        bool _IsVoice = false;
        #endregion

        #region 音声再生命令中[IsPressVoice]プロパティ
        /// <summary>
        /// 音声再生命令中[IsPressVoice]プロパティ用変数
        /// </summary>
        bool _IsPressVoice = false;
        /// <summary>
        /// 音声再生命令中[IsPressVoice]プロパティ
        /// </summary>
        public bool IsPressVoice
        {
            get
            {
                return _IsPressVoice;
            }
            set
            {
                if (!_IsPressVoice.Equals(value))
                {
                    _IsPressVoice = value;
                    NotifyPropertyChanged("IsPressVoice");
                }
            }
        }
        #endregion



        #region SpeachRate[Rate]プロパティ
        /// <summary>
        /// SpeachRate[Rate]プロパティ用変数
        /// </summary>
        int _Rate = 0;
        /// <summary>
        /// SpeachRate[Rate]プロパティ
        /// </summary>
        public int Rate
        {
            get
            {
                return _Rate;
            }
            set
            {
                if (!_Rate.Equals(value))
                {
                    _Rate = value;
                    NotifyPropertyChanged("Rate");
                }
            }
        }
        #endregion

        #region フレーズ要素[PhraseItems]プロパティ
        /// <summary>
        /// フレーズ要素[PhraseItems]プロパティ用変数
        /// </summary>
        ModelList<PhraseM> _PhraseItems = new ModelList<PhraseM>();
        /// <summary>
        /// フレーズ要素[PhraseItems]プロパティ
        /// </summary>
        public ModelList<PhraseM> PhraseItems
        {
            get
            {
                return _PhraseItems;
            }
            set
            {
                if (_PhraseItems == null || !_PhraseItems.Equals(value))
                {
                    _PhraseItems = value;
                    NotifyPropertyChanged("PhraseItems");
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
                var wnd = sender as EditWindowV;
                if(wnd !=null)
                {
                    wnd.WebView2Ctrl.EnsureCoreWebView2Async(null);
                }

                var synthesizer = new SpeechSynthesizer();
                var installedVoices = synthesizer.GetInstalledVoices();
                this.VoiceList.Items.Clear();

                foreach (var voice in installedVoices)
                {
                    this.VoiceList.Items.Add(voice);
                }

                // nullチェック
                if (this.VoiceList.Items.FirstOrDefault() != null)
                {
                    this.VoiceList.SelectedItem = this.VoiceList.Items.FirstOrDefault()!;
                }


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

        #region 新規作成
        /// <summary>
        /// 新規作成
        /// </summary>
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
        #endregion

        #region 保存処理
        /// <summary>
        /// 保存処理
        /// </summary>
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
                var wnd = sender as EditWindowV;

                if (wnd != null)
                {
                    var key_eve = e as KeyEventArgs;
                    var deepl_url_base = "https://www.deepl.com/ja/translator#en/ja/{0}";
                    var googletranslate_url_base = "https://translate.google.co.jp/?sl=en&tl=ja&text={0}&op=translate";

                    if (key_eve!.KeyboardDevice.IsKeyDown(Key.LeftAlt) || key_eve!.KeyboardDevice.IsKeyDown(Key.RightAlt))
                    {
                        switch (key_eve.SystemKey)
                        {
                            case Key.Down:
                                {
                                    int index = this.TangoCollection.TangoList.IndexOf(this.TangoCollection.SelectedItem);

                                    if (this.TangoCollection.TangoList.Count > index + 1)
                                    {
                                        this.TangoCollection.TangoList.SelectedItem = this.TangoCollection.TangoList.ElementAt(index + 1);
                                    }
                                    else
                                    {
                                        this.TangoCollection.Add(new TangoM());
                                    }
                                    break;
                                }
                            case Key.Up:
                                {
                                    int index = this.TangoCollection.TangoList.IndexOf(this.TangoCollection.SelectedItem);

                                    if (index - 1 >= 0)
                                    {
                                        this.TangoCollection.TangoList.SelectedItem = this.TangoCollection.TangoList.ElementAt(index - 1);
                                    }
                                    break;
                                }
                            case Key.Enter:
                                {
                                    wnd!.question_txt.Focus();
                                    break;
                                }
                            case Key.D1:
                                {
                                    if (wnd != null)
                                    {
                                        string url = string.Format(googletranslate_url_base, this.TangoCollection.TangoList.SelectedItem.Selections[0]);
                                        if (wnd.WebView2Ctrl != null && wnd.WebView2Ctrl.CoreWebView2 != null)
                                        {
                                            wnd.WebView2Ctrl.CoreWebView2.Navigate(url);
                                        }
                                    }
                                    break;
                                }
                            case Key.D2:
                                {
                                    string url = string.Format(googletranslate_url_base, this.TangoCollection.TangoList.SelectedItem.Selections[1]);
                                    if (wnd.WebView2Ctrl != null && wnd.WebView2Ctrl.CoreWebView2 != null)
                                    {
                                        wnd.WebView2Ctrl.CoreWebView2.Navigate(url);
                                    }
                                    break;
                                }
                            case Key.D3:
                                {
                                    string url = string.Format(googletranslate_url_base, this.TangoCollection.TangoList.SelectedItem.Selections[2]);
                                    if (wnd.WebView2Ctrl != null && wnd.WebView2Ctrl.CoreWebView2 != null)
                                    {
                                        wnd.WebView2Ctrl.CoreWebView2.Navigate(url);
                                    }
                                    break;
                                }
                            case Key.D4:
                                {
                                    string url = string.Format(googletranslate_url_base, this.TangoCollection.TangoList.SelectedItem.Selections[3]);
                                    if (wnd.WebView2Ctrl != null && wnd.WebView2Ctrl.CoreWebView2 != null)
                                    {
                                        wnd.WebView2Ctrl.CoreWebView2.Navigate(url);
                                    }
                                    break;
                                }
                            case Key.D5:
                                {
                                    string url = string.Format(deepl_url_base, this.PhraseItems.SelectedItem.Phrase);
                                    if (wnd.WebView2Ctrl != null && wnd.WebView2Ctrl.CoreWebView2 != null)
                                    {
                                        wnd.WebView2Ctrl.CoreWebView2.Navigate(url);
                                    }
                                    break;
                                }
                            case Key.D6:
                                {
                                    string url = string.Format(googletranslate_url_base, this.PhraseItems.SelectedItem.Words.SelectedItem.Word);
                                    if (wnd.WebView2Ctrl != null && wnd.WebView2Ctrl.CoreWebView2 != null)
                                    {
                                        wnd.WebView2Ctrl.CoreWebView2.Navigate(url);
                                    }
                                    break;
                                }
                            case Key.D0:
                                {
                                    string url = string.Format(deepl_url_base, this.TangoCollection.TangoList.SelectedItem.Querstion);
                                    if (wnd.WebView2Ctrl != null && wnd.WebView2Ctrl.CoreWebView2 != null)
                                    {
                                        wnd.WebView2Ctrl.CoreWebView2.Navigate(url);
                                    }
                                    break;
                                }
                        }
                    }
                    if (key_eve!.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || key_eve!.KeyboardDevice.IsKeyDown(Key.RightCtrl))
                    {
                        switch (key_eve.Key)
                        {
                            case Key.S:
                                {
                                    if (File.Exists(this.TangoCollection.FilePath))
                                    {
                                        this.TangoCollection.SaveExcel(this.TangoCollection.FilePath);
                                    }
                                    else
                                    {
                                        Save();
                                    }

                                    break;
                                }

                        }
                    }
                }
            }
            catch (Exception ev)
            {
                ShowMessage.ShowErrorOK(ev.Message, "Error");
            }
        }
        #endregion

        #region Questionの選択変更処理
        /// <summary>
        /// Questionの選択変更処理
        /// </summary>
        public void QuestionChanged()
        {
            try
            {
                if (this.TangoCollection.SelectedItem != null && this.TangoCollection.SelectedItem.Querstion != null)
                {
                    // フレーズに分解する
                    var list = this.TangoCollection.SelectedItem.Querstion.Replace("\r", "").Split("\n");

                    // フレーズリストをクリア
                    this.PhraseItems.Items.Clear();

                    // リスト数文まわす
                    foreach (var tmp in list)
                    {
                        // フレーズリストに追加
                        this.PhraseItems.Items.Add(new PhraseM()
                        {
                            Phrase = tmp
                        }
                        );
                    }
                }
                
            }
            catch
            {

            }
        }
        #endregion

        #region URLを開く
        /// <summary>
        /// URLを開く
        /// </summary>
        /// <param name="url">開くURL</param>
        private void OpenURL(string url)
        {
            try
            {
                ProcessStartInfo pi = new ProcessStartInfo()
                {
                    FileName = url,
                    UseShellExecute = true,
                };

                Process.Start(pi);
            }
            catch
            {
            }
        }
        #endregion

        #region フレーズを音声再生する
        /// <summary>
        /// フレーズを音声再生する
        /// </summary>
        public void PhraseVoice()
        {
            try
            {
                // nullチェック
                if(this.PhraseItems.SelectedItem != null)
                {
                    PhraseVoice(this.PhraseItems.SelectedItem.Phrase);  // フレーズ再生
                }
            }
            catch
            {
            }
        }
        #endregion

        #region 合成音声を連続で再生する
        /// <summary>
        /// 合成音声を連続で再生する
        /// </summary>
        public void PhraseVoiceContinue()
        {
            try
            {
                Task.Run(() =>
                {
                    int index = 0;
                    while (true)
                    {
                        if (index < this.PhraseItems.Count)
                        {
                            var tmp = this.PhraseItems.ElementAt(index);

                            this.PhraseItems.SelectedItem = tmp;
                            if (this.IsPressVoice)
                            {
                                PhraseVoice(tmp.Phrase);    // フレーズ再生
                            }
                            else
                            {
                                break;
                            }
                            index++;
                        }
                        else
                        {
                            index = 0;
                        }
                    }
                });
            }
            catch
            {
            }
        }
        #endregion

        #region フレーズを音声再生する
        /// <summary>
        /// フレーズを音声再生する
        /// </summary>
        /// <param name="phrase">フレーズ</param>
        private void PhraseVoice(string phrase)
        {
            try
            {
                // 音声が再生中でない場合のみ実行
                if (!this._IsVoice)
                {
                    this._IsVoice = true;
                    var synthesizer = new SpeechSynthesizer();
                    var tmp = synthesizer.GetInstalledVoices();

                    synthesizer.SetOutputToDefaultAudioDevice();

                    synthesizer.SelectVoice(this.VoiceList.SelectedItem.VoiceInfo.Name);
                    synthesizer.Rate = this.Rate;
                    synthesizer.Speak(phrase);
                    this._IsVoice = false;
                }
            }
            catch
            {
            }
        }
        #endregion

        #region フレーズのダブルクリック
        /// <summary>
        /// フレーズのダブルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PhraseDoubleClick(object sender, EventArgs e)
        {
            try
            {
                // ウィンドウを取得
                var wnd = VisualTreeHelperWrapper.GetWindow<EditWindowV>(sender) as EditWindowV;

                // nullチェック
                if (wnd != null)
                {
                    var deepl_url_base = "https://www.deepl.com/ja/translator#en/ja/{0}";   // DeepLのURL

                    // nullチェック
                    if (this.PhraseItems.SelectedItem != null)
                    {
                        string url = string.Format(deepl_url_base, this.PhraseItems.SelectedItem.Phrase);   // URL作成

                        // nullチェック
                        if (wnd.WebView2Ctrl != null && wnd.WebView2Ctrl.CoreWebView2 != null)
                        {
                            // URLを開く
                            wnd.WebView2Ctrl.CoreWebView2.Navigate(url);
                        }
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

        #region 単語のダブルクリック
        /// <summary>
        /// 単語のダブルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WordDoubleClick(object sender, EventArgs e)
        {
            try
            {
                // ウィンドウを取得
                var wnd = VisualTreeHelperWrapper.GetWindow<EditWindowV>(sender) as EditWindowV;

                // nullチェック
                if (wnd != null)
                {
                    var googletranslate_url_base = "https://translate.google.co.jp/?sl=en&tl=ja&text={0}&op=translate";

                    // nullチェック
                    if (this.PhraseItems.SelectedItem != null && this.PhraseItems.SelectedItem.Words.SelectedItem != null)
                    {
                        string url = string.Format(googletranslate_url_base, this.PhraseItems.SelectedItem.Words.SelectedItem.Word);   // URL作成

                        // nullチェック
                        if (wnd.WebView2Ctrl != null && wnd.WebView2Ctrl.CoreWebView2 != null)
                        {
                            // URLを開く
                            wnd.WebView2Ctrl.CoreWebView2.Navigate(url);
                        }
                    }
                }
            }
            catch
            {
            }
        }
        #endregion

        public void SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                // ウィンドウを取得
                var wnd = VisualTreeHelperWrapper.GetWindow<EditWindowV>(sender) as EditWindowV;

                if (wnd != null)
                {
                    wnd.phrase_dg.ScrollIntoView(wnd.phrase_dg.Items[wnd.phrase_dg.SelectedIndex]); //scroll to last
                    wnd.phrase_dg.UpdateLayout();
                }
            }
            catch
            {
                
            }
        }
    }
}

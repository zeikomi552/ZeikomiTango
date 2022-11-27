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

                var key_eve = e as KeyEventArgs;

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
                                OpenURL($"https://translate.google.co.jp/?hl=ja&sl=en&tl=ja&text={this.TangoCollection.TangoList.SelectedItem.Selections[0]}%0A&op=translate");
                                break;
                            }
                        case Key.D2:
                            {
                                OpenURL($"https://translate.google.co.jp/?hl=ja&sl=en&tl=ja&text={this.TangoCollection.TangoList.SelectedItem.Selections[1]}%0A&op=translate");
                                break;
                            }
                        case Key.D3:
                            {
                                OpenURL($"https://translate.google.co.jp/?hl=ja&sl=en&tl=ja&text={this.TangoCollection.TangoList.SelectedItem.Selections[2]}%0A&op=translate");
                                break;
                            }
                        case Key.D4:
                            {
                                OpenURL($"https://translate.google.co.jp/?hl=ja&sl=en&tl=ja&text={this.TangoCollection.TangoList.SelectedItem.Selections[3]}%0A&op=translate");
                                break;
                            }
                        case Key.V:
                            {
                                StartVoice();
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
            catch (Exception ev)
            {
                ShowMessage.ShowErrorOK(ev.Message, "Error");
            }
        }
        #endregion

        public void StartVoice()
        {
            try
            {
                var synthesizer = new SpeechSynthesizer();
                synthesizer.SetOutputToDefaultAudioDevice();
                synthesizer.SelectVoice("Microsoft Zira Desktop");
                synthesizer.Rate = this.Rate;
                synthesizer.Speak(this.TangoCollection.TangoList.SelectedItem.Querstion);
                synthesizer.Speak(this.TangoCollection.TangoList.SelectedItem.Selections[0]);
                synthesizer.Speak(this.TangoCollection.TangoList.SelectedItem.Selections[1]);
                synthesizer.Speak(this.TangoCollection.TangoList.SelectedItem.Selections[2]);
                synthesizer.Speak(this.TangoCollection.TangoList.SelectedItem.Selections[3]);
            }
            catch
            {

            }
        }

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
    }
}

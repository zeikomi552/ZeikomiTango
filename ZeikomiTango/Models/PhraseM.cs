using MVVMCore.BaseClass;
using MVVMCore.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeikomiTango.Models
{
    public class PhraseM : ModelBase
    {
        #region フレーズ[Phrase]プロパティ
        /// <summary>
        /// フレーズ[Phrase]プロパティ用変数
        /// </summary>
        string _Phrase = string.Empty;
        /// <summary>
        /// フレーズ[Phrase]プロパティ
        /// </summary>
        public string Phrase
        {
            get
            {
                return _Phrase;
            }
            set
            {
                if (_Phrase == null || !_Phrase.Equals(value))
                {
                    _Phrase = value;
                    NotifyPropertyChanged("Phrase");
                    SelectionChanged();
                }
            }
        }
        #endregion

        #region 単語[Words]プロパティ
        /// <summary>
        /// 単語[Words]プロパティ用変数
        /// </summary>
        ModelList<WordM> _Words = new ModelList<WordM>();
        /// <summary>
        /// 単語[Words]プロパティ
        /// </summary>
        public ModelList<WordM> Words
        {
            get
            {
                return _Words;
            }
            set
            {
                if (_Words == null || !_Words.Equals(value))
                {
                    _Words = value;
                    NotifyPropertyChanged("Words");
                }
            }
        }
        #endregion

        #region 選択の変更
        /// <summary>
        /// 選択の変更
        /// </summary>
        public void SelectionChanged()
        {
            this.Words.Items.Clear();
            var tmp = this.Phrase.Split(" ");
            foreach (var word in tmp)
            {
                this.Words.Items.Add(new WordM()
                {
                    Word = word
                }
                );
            }
        }
        #endregion

    }
}

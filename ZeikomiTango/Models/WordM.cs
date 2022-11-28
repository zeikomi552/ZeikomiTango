using MVVMCore.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeikomiTango.Models
{
    public class WordM : ModelBase
    {
        #region 単語[Word]プロパティ
        /// <summary>
        /// 単語[Word]プロパティ用変数
        /// </summary>
        string _Word = string.Empty;
        /// <summary>
        /// 単語[Word]プロパティ
        /// </summary>
        public string Word
        {
            get
            {
                return _Word;
            }
            set
            {
                if (_Word == null || !_Word.Equals(value))
                {
                    _Word = value;
                    NotifyPropertyChanged("Word");
                }
            }
        }
        #endregion


    }
}

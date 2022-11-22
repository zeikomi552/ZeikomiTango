using MVVMCore.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeikomiTango.Models
{
    public class TangoM : ModelBase
    {
        public TangoM()
        {

        }

        public TangoM(string question, string explain, string select_a, string select_b, string select_c, string select_d, string answer)
        {
            this.Querstion = question;
            this.Explanation = explain;
            this.Selections.Clear();
            this.Selections.Add(select_a);
            this.Selections.Add(select_b);
            this.Selections.Add(select_c);
            this.Selections.Add(select_d);
            this.Answer = answer;
        }

        #region 設問[Querstion]プロパティ
        /// <summary>
        /// 設問[Querstion]プロパティ用変数
        /// </summary>
        string _Querstion = string.Empty;
        /// <summary>
        /// 設問[Querstion]プロパティ
        /// </summary>
        public string Querstion
        {
            get
            {
                return _Querstion;
            }
            set
            {
                if (_Querstion == null || !_Querstion.Equals(value))
                {
                    _Querstion = value;
                    NotifyPropertyChanged("Querstion");
                }
            }
        }
        #endregion

        #region 選択肢[Selections]プロパティ
        /// <summary>
        /// 選択肢[Selections]プロパティ用変数
        /// </summary>
        List<string> _Selections = new List<string>();
        /// <summary>
        /// 選択肢[Selections]プロパティ
        /// </summary>
        public List<string> Selections
        {
            get
            {
                return _Selections;
            }
            set
            {
                if (_Selections == null || !_Selections.Equals(value))
                {
                    _Selections = value;
                    NotifyPropertyChanged("Selections");
                }
            }
        }
        #endregion

        #region 答え[Answer]プロパティ
        /// <summary>
        /// 答え[Answer]プロパティ用変数
        /// </summary>
        string _Answer = string.Empty;
        /// <summary>
        /// 答え[Answer]プロパティ
        /// </summary>
        public string Answer
        {
            get
            {
                return _Answer;
            }
            set
            {
                if (_Answer == null || !_Answer.Equals(value))
                {
                    _Answer = value;
                    NotifyPropertyChanged("Answer");
                }
            }
        }
        #endregion

        #region 解説[Explanation]プロパティ
        /// <summary>
        /// 解説[Explanation]プロパティ用変数
        /// </summary>
        string _Explanation = string.Empty;
        /// <summary>
        /// 解説[Explanation]プロパティ
        /// </summary>
        public string Explanation
        {
            get
            {
                return _Explanation;
            }
            set
            {
                if (_Explanation == null || !_Explanation.Equals(value))
                {
                    _Explanation = value;
                    NotifyPropertyChanged("Explanation");
                }
            }
        }
        #endregion

        #region 画面に表示するかどうか[IsDisplay]プロパティ
        /// <summary>
        /// 画面に表示するかどうか[IsDisplay]プロパティ用変数
        /// </summary>
        bool _IsDisplay = true;
        /// <summary>
        /// 画面に表示するかどうか[IsDisplay]プロパティ
        /// </summary>
        public bool IsDisplay
        {
            get
            {
                return _IsDisplay;
            }
            set
            {
                if (!_IsDisplay.Equals(value))
                {
                    _IsDisplay = value;
                    NotifyPropertyChanged("IsDisplay");
                }
            }
        }
        #endregion

        /// <summary>
        /// 値のセット処理
        /// </summary>
        /// <param name="question">設問</param>
        /// <param name="select_a">選択肢A</param>
        /// <param name="select_b">選択肢B</param>
        /// <param name="select_c">選択肢C</param>
        /// <param name="select_d">選択肢D</param>
        /// <param name="answer">回答</param>
        public void SetValue(string question, string select_a, string select_b, string select_c, string select_d, string answer)
        {
            this.Querstion = question;
            this.Selections.Clear();
            this.Selections.Add(select_a);
            this.Selections.Add(select_b);
            this.Selections.Add(select_c);
            this.Selections.Add(select_d);
            this.Answer = answer;
        }

        public string DisplayQuestion
        {
            get
            {
                return this.Querstion + "\r\n\r\n" 
                    + "A:" + this.Selections.ElementAt(0)
                    + "\r\nB:" + this.Selections.ElementAt(1)
                    + "\r\nC:" + this.Selections.ElementAt(2)
                    + "\r\nD:" + this.Selections.ElementAt(3);
            }
        }

        public string DisplayAnswer
        {
            get
            {
                string answer = this.Answer;
                string answer_tmp = string.Empty;

                switch (answer.ToUpper())
                {
                    case "A":
                    default:
                        {
                            answer_tmp = this.Selections.ElementAt(0);
                            break;
                        }
                    case "B":
                        {
                            answer_tmp = this.Selections.ElementAt(1);
                            break;
                        }
                    case "C":
                        {
                            answer_tmp = this.Selections.ElementAt(2);
                            break;
                        }
                    case "D":
                        {
                            answer_tmp = this.Selections.ElementAt(3);
                            break;
                        }

                }

                return this.Answer + ":" + answer_tmp + "\r\n\r\n" + this.Explanation;
            }
        }
    }
}

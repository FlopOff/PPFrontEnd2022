using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerfectPolicyFrontEnd.Models.QuizModels;

namespace PerfectPolicyFrontEnd.Models.QuestionModels
{
    public class Question
    {
        public int QuestionID { get; set; }
        public string QuestionTopic { get; set; }
        public string QuestionText { get; set; }
        public string QuestionImage { get; set; }

        public string Creator { get; set; }

        public Quiz quiz { get; set; }
        //public ICollection<Option> Options { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Service
{
    using System;
    using System.Collections.Generic;
    
    public partial class Answer
    {
        public int AnswerId { get; set; }
        public int QId { get; set; }
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public string IpAddress { get; set; }
        public System.DateTime CreateTime { get; set; }
    
        public virtual Option Option { get; set; }
        public virtual Question Question { get; set; }
        public virtual Questionnaire Questionnaire { get; set; }
    }
}

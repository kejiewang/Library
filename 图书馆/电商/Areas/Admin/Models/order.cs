//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace 电商.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class order
    {
        public int orderId { get; set; }
        public string time { get; set; }
        public int num { get; set; }
        public int isFinish { get; set; }
        public int book_id { get; set; }
        public int publish_id { get; set; }
        public string endtime { get; set; }
    
        public virtual book book { get; set; }
        public virtual publish publish { get; set; }
    }
}

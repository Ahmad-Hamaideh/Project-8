//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_8_MVC_Batool
{
    using System;
    using System.Collections.Generic;
    
    public partial class Delay
    {
        public int ID_Delay { get; set; }
        public string Description { get; set; }
        public string User_ID { get; set; }
        public Nullable<int> Semester { get; set; }
        public Nullable<int> Delay_Hour { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}

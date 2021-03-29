//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StoreFront2.DATA.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Jewelery
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int MaterialID { get; set; }
        public Nullable<int> FitID { get; set; }
        public int InvID { get; set; }
        public Nullable<int> TypeID { get; set; }
        public int SupplierID { get; set; }
        public int UnitsSold { get; set; }
        public Nullable<System.DateTime> ReleaseDate { get; set; }
        public string ProductImage { get; set; }
        public string Description { get; set; }
        public Nullable<bool> SoldAsPair { get; set; }
        public Nullable<decimal> Price { get; set; }
    
        public virtual Fit Fit { get; set; }
        public virtual InvStatu InvStatu { get; set; }
        public virtual Material Material { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Type Type { get; set; }
    }
}

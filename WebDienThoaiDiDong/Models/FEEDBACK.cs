//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebDienThoaiDiDong.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FEEDBACK
    {
        public int MaFB { get; set; }
        public string ChuDeFB { get; set; }
        public string ThongtinFB { get; set; }
        public Nullable<int> MaSP { get; set; }
        public System.DateTime Ngaydanhgia { get; set; }
        public string TenKH { get; set; }
        public string TenSP { get; set; }
    
        public virtual SANPHAM SANPHAM { get; set; }
    }
}

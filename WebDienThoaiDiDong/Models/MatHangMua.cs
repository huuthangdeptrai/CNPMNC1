using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDienThoaiDiDong.Models;

namespace WebDienThoaiDiDong.Models
{
    
    public class MatHangMua
    {
        WebDienThoaiEntities database = new WebDienThoaiEntities();
        public int Masp { get; set; }
        public int Malsp { get; set; }
        public string Tensp { get; set; }
        public string Hinhminhhoa { get; set; }
        public int Soluong { get; set; }
        public double Dongia { get; set; }
        public string Mausac { get; set; }
        public double ThanhTien ()
        {
            return Dongia * Soluong;
        }
        public MatHangMua (int masp , int malsp)
        {
            this.Masp = masp;
            this.Malsp = malsp;
            var sp = database.SANPHAMs.Single(s => s.MaSP == this.Masp);
            var lsp = database.LOAISANPHAMs.Single(s => s.MaLSP == this.Malsp);
            this.Tensp = sp.TenSP;
            this.Hinhminhhoa = lsp.HinhMinhHoa;
            this.Mausac = lsp.MauSac;
            this.Dongia = double.Parse(lsp.DonGia.ToString());
            this.Soluong = 1;
        }
        
    }
}
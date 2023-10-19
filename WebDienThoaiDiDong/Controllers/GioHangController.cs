using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDienThoaiDiDong.Models;
namespace WebDienThoaiDiDong.Controllers
{
    public class GioHangController : Controller
    {
        WebDienThoaiEntities database = new WebDienThoaiEntities();
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
        public List<MatHangMua> LayGioHang()
        {
            List<MatHangMua> gioHang = Session["GioHang"] as List<MatHangMua>;
            //Nếu giỏ hàng chưa tồn tại thì tạo mới và đưa vào Session
            if (gioHang == null)
            {
                gioHang = new List<MatHangMua>();
                Session["GioHang"] = gioHang;
            }
            return gioHang;
        }
        public ActionResult ThemSanPhamVaoGio(int Masp, int Malsp)
        {

            List<MatHangMua> gioHang = LayGioHang();

            MatHangMua sanPham = gioHang.FirstOrDefault(s => s.Malsp == Malsp);
            if (sanPham == null) //Sản phẩm chưa có trong giỏ
            {
                sanPham = new MatHangMua(Masp, Malsp);
                gioHang.Add(sanPham);
            }
            else
            {
                sanPham.Soluong++;
            }    
            // Lấy giỏ hàng hiện tại
            return RedirectToAction("Details", "SanPham", new { id = Masp });
        }
        private int TinhTongSL()
        {
            int TongSL = 0;
            List<MatHangMua> giohang = LayGioHang();
            if (giohang != null)
                TongSL = giohang.Sum(sp => sp.Soluong);
            return TongSL;
        }


        private double TinhTongTien()
        {
            double TongTien = 0;
            List<MatHangMua> giohang = LayGioHang();
            if (giohang != null)
                TongTien = giohang.Sum(sp => sp.ThanhTien());
            return TongTien;
        }
        public ActionResult HienThiGioHang()
        {
            List<MatHangMua> giohang = LayGioHang();
            //Nếu giỏ hàng trống thì trả về trang ban đầu
            if (giohang == null || giohang.Count == 0)
            {
                return RedirectToAction("Index", "SanPham");
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(giohang); //Trả về View hiển thị thông tin giỏ hàng
        }
        public ActionResult XoaMatHang(int Malsp)
        {
            List<MatHangMua> gioHang = LayGioHang();
            //Lấy sản phẩm trong giỏ hàng
            var sanpham = gioHang.FirstOrDefault(s => s.Malsp == Malsp);
            if (sanpham != null)
            {
                gioHang.RemoveAll(s => s.Malsp == Malsp);
                return RedirectToAction("HienThiGioHang"); //Quay về trang giỏ hàng
            }
            if (gioHang.Count == 0) //Quay về trang chủ nếu giỏ hàng không có gì
                return RedirectToAction("Index", "SanPham");
            return RedirectToAction("HienThiGioHang");
        }
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null) //Chưa đăng nhập
                return RedirectToAction("DangNhap", "NguoiDung");

            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang == null || gioHang.Count == 0) //Chưa có giỏ hàng hoặc chưa có sp
                return RedirectToAction("Index", "SanPham");
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang); //Trả về View hiển thị thông tin giỏ hàng
        }
        public ActionResult DongYDatHang(KHACHHANG model)
        {

            KHACHHANG khach = Session["TaiKhoan"] as KHACHHANG;
            List<MatHangMua> gioHang = LayGioHang();
            DONMUASP DonHang = new DONMUASP();
            DonHang.MaKH = khach.MaKH;
            DonHang.NgayDat = DateTime.Now;
            DonHang.TriGia = (decimal)TinhTongTien();
            DonHang.TenNguoiMua = khach.TenKH;
            DonHang.Payments = String.Empty;
            ViewBag.TongSL = TinhTongSL();
            database.DONMUASPs.Add(DonHang);
            database.SaveChanges();

            foreach (var phong in gioHang)
            {
                CHITIETDONMUA chitiet = new CHITIETDONMUA();
                chitiet.MaDonMua = DonHang.MaDonMua;
                chitiet.MaLSP = phong.Malsp;
                chitiet.DonGia = (decimal?)phong.Dongia;
                chitiet.AnhMinhHoa = phong.Hinhminhhoa;
                chitiet.HinhMinhHoa = phong.Hinhminhhoa;
                chitiet.SoLuong = phong.Soluong;
                chitiet.MauSac = phong.Mausac;
                chitiet.NgayDat = DonHang.NgayDat;
                database.CHITIETDONMUAs.Add(chitiet);
            }
            database.SaveChanges();

            Session["GioHang"] = null;
            return RedirectToAction("FilterResults");
            // Chuyển hướng đến trang thành công hoặc trang cảm ơn
        }
    }
}
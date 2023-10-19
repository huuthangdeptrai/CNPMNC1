using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDienThoaiDiDong.Models;

namespace WebDienThoaiDiDong.Controllers
{
    public class SanPhamController : Controller
    {
        WebDienThoaiEntities database = new WebDienThoaiEntities();
        // GET: SanPham
        public ActionResult LayDanhMuc()
        {
            var DanhSachDanhMuc = database.DANHMUCs.ToList();
            return PartialView(DanhSachDanhMuc);            
        }
        private List<SANPHAM> LaySPMoi(int soluong)
        {
            // Sắp xếp sách theo ngày cập nhật giảm dần, lấy đúng số lượng sách cần
            // Chuyển qua dạng danh sách kết quả đạt được
            return database.SANPHAMs.OrderByDescending(mb =>
           mb.MaSP).Take(soluong).ToList();
        }
        // GET: BookStore
        public ActionResult Index()
        {
            // Giả sử cần lấy 5 quyển sách mới cập nhật
            var dsSPMoi = LaySPMoi(8);
            return View(dsSPMoi);
        }
        public ActionResult Details(int id)
        {
            var sp = database.SANPHAMs.FirstOrDefault(a => a.MaSP == id);
            if (sp != null)
            {
                var sanpham = database.LOAISANPHAMs.Where(p => p.MaSP == sp.MaSP).ToList();
                sp.LOAISANPHAMs = sanpham;
            }
            return View(sp);
        }
        public ActionResult FilterResults(bool? Samsung, bool? Iphone, bool? Xiaomi, bool? Vivo, bool? Oppo, bool? Honor)
        {
            List<SANPHAM> locsp = GetFilteredDataFromDatabase(Samsung,Iphone,Xiaomi,Vivo,Oppo,Honor);
            return View(locsp);
        }
        private List<SANPHAM> GetFilteredDataFromDatabase(bool? Samsung, bool? Iphone, bool? Xiaomi, bool? Vivo, bool? Oppo, bool? Honor)
        {
            using (var context = new WebDienThoaiEntities())
            {
                IQueryable<SANPHAM> query = context.SANPHAMs;
                if (Samsung == true)
                {
                    query = query.Where(n => n.HangDienThoai == "Samsung");
                }
                if (Iphone == true)
                {
                    query = query.Where(n => n.HangDienThoai == "Iphone");
                }
                if (Xiaomi == true)
                {
                    query = query.Where(n => n.HangDienThoai == "Xiaomi");
                }
                if (Vivo == true)
                {
                    query = query.Where(n => n.HangDienThoai == "Vivo");
                }
                if (Oppo == true)
                {
                    query = query.Where(n => n.HangDienThoai == "Oppo");
                }
                if (Honor == true)
                {
                    query = query.Where(n => n.HangDienThoai == "Honor");
                }
                List<SANPHAM> results = query.ToList();
                return results;
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["TaiKhoan"] == null) //Chưa đăng nhập
                return RedirectToAction("DangNhap", "NguoiDung");
            ViewBag.MaSP = new SelectList(database.SANPHAMs, "MaSP", "TenSP");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaFB,ChuDeFB,ThongtinFB,MaSP,Ngaydanhgia,TenKH,TenSP")]
        FEEDBACK matbang)
        {

            if (ModelState.IsValid)
            {
                var ks = database.SANPHAMs.FirstOrDefault(p => p.MaSP == matbang.MaSP);

                KHACHHANG khach = Session["TaiKhoan"] as KHACHHANG;
                matbang.TenKH = khach.TenKH;
                matbang.TenSP = ks.TenSP;
                matbang.Ngaydanhgia = DateTime.Now;

                database.FEEDBACKs.Add(matbang);
                database.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaSP = new SelectList(database.SANPHAMs, "MaSP", "TenSP", matbang.MaSP);
            return View(matbang);
        }
    }
}
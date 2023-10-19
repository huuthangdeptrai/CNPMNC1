using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDienThoaiDiDong.Models;

namespace WebDienThoaiDiDong.Controllers
{
    public class NguoiDungController : Controller
    {
        WebDienThoaiEntities database = new WebDienThoaiEntities();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.TenKH))
                    ModelState.AddModelError(string.Empty, "Tên không được để trống");
                if (string.IsNullOrEmpty(kh.HoKH))
                    ModelState.AddModelError(string.Empty, "Họ không được để trống");

                if (string.IsNullOrEmpty(kh.Email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(kh.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (string.IsNullOrEmpty(kh.Xacnhanmatkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu xác nhận không được để trống");
                if (kh.Matkhau != kh.Xacnhanmatkhau)
                {
                    ModelState.AddModelError("Xacnhanmatkhau", "Xác nhận mật khẩu không chính xác");
                    TempData["ErrorMessage"] = "Mật khẩu xác nhận phải trùng khớp, vui lòng nhập lại";
                    return View(kh);
                }
                //Kiểm tra xem có người nào đã đăng kí với tên đăng nhập này hay chưa
                var khachhang = database.KHACHHANGs.FirstOrDefault(k => k.Email == kh.Email);
                if (khachhang != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng kí tên này");

                if (ModelState.IsValid)
                {
                    database.KHACHHANGs.Add(kh);
                    database.SaveChanges();
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("DangNhap");
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.Email))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(kh.Matkhau))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (ModelState.IsValid)
                {
                    //Tìm khách hàng có tên đăng nhập và password hợp lệ trong CSDL
                    var khach = database.KHACHHANGs.FirstOrDefault(k => k.Email ==
                   kh.Email && k.Matkhau == kh.Matkhau);
                    if (khach != null)
                    {
                        TempData["SuccessMessage"] = "Đăng nhập thành công";
                        //Lưu vào session
                        Session["TaiKhoan"] = khach;
                    }
                    else
                        TempData["SuccessMessage"] = "Tài khoản hoặc mật khẩu không chính xác";
                }
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Abandon();
            return RedirectToAction("DangNhap", "NguoiDung");
        }
    }
}
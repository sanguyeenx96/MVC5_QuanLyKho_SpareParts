using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebQuanLySpare.Models;

namespace WebQuanLySpare.Controllers
{
    public class dathangsController : Controller
    {
        private WebSparePartEntities db = new WebSparePartEntities();
        public ActionResult lichsu()
        {
            var list = db.dathangs.Where(x => x.trangthai == "Hàng đã về").ToList().OrderByDescending(x => x.ngaydathang);
            var dem = list.Count();
            ViewBag.soluonghangve = dem;
            return View(list);
        }
        // GET: dathangs
        public ActionResult Index()
        {
            var danhsach = from a in db.danhsachlinhkiens select a;
            List<danhsachlinhkien> danhsachlinhkien = danhsach.ToList();
            ViewData["danhsachlinhkien"] = danhsachlinhkien;

            var list = db.dathangs.Where(x => x.trangthai == "Đang đặt hàng").ToList().OrderByDescending(x=>x.ngaydathang);
            var dem = list.Count();
            ViewBag.soluong = dem;
            return View(list);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(dathang dat)
        {
            dathang dh = new dathang();
            dh.tenlinhkien = dat.tenlinhkien;
            dh.ngaydathang = DateTime.Now.ToString("dd/MM/yyyy");
            dh.malinhkien = dat.malinhkien;
            dh.maker = dat.maker;
            dh.soluong = dat.soluong;
            dh.donvi = dat.donvi;
            dh.dongia = dat.dongia;
            dh.thanhtien = dh.soluong * dh.dongia;
            dh.ghichu = dat.ghichu;
            dh.trangthai = "Đang đặt hàng";
            db.dathangs.Add(dh);
            db.SaveChanges();
            ViewBag.msg = "thành công";
            return RedirectToAction("index");
        }

        // GET: dathangs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dathang dathang = db.dathangs.Find(id);
            if (dathang == null)
            {
                return HttpNotFound();
            }
            return View(dathang);
        }

        // GET: dathangs/Create
        public ActionResult Create(int? id)
        {
            var danhsach = from a in db.danhsachlinhkiens select a;
            List<danhsachlinhkien> thongtinlinhkien = danhsach.Where(x=>x.id == id).ToList();
            ViewData["thongtinlinhkien"] = thongtinlinhkien;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(dathang dat)
        {
            dathang dh = new dathang();
            dh.tenlinhkien = dat.tenlinhkien;
            dh.ngaydathang = DateTime.Now.ToString("dd/MM/yyyy");
            dh.malinhkien = dat.malinhkien;
            dh.maker = dat.maker;
            dh.soluong = dat.soluong;
            dh.donvi = dat.donvi;
            dh.dongia = dat.dongia;
            dh.thanhtien = dh.soluong * dh.dongia;
            dh.ghichu = dat.ghichu;
            dh.trangthai = "Đang đặt hàng";
            db.dathangs.Add(dh);
            db.SaveChanges();
            ViewBag.msg = "Đã nhập câu hỏi thành công";
            return RedirectToAction("index");
        }

        // GET: dathangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dathang dathang = db.dathangs.Find(id);
            if (dathang == null)
            {
                return HttpNotFound();
            }
            return View(dathang);
        }

        // POST: dathangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ngaydathang,ngayhangve,tenlinhkien,malinhkien,maker,soluong,donvi,dongia,thanhtien,ghichu,trangthai")] dathang dathang)
        {
            if (ModelState.IsValid)
            {
                var linhkien = db.danhsachlinhkiens.Where(s => s.malinhkien == dathang.malinhkien).FirstOrDefault();
                if (linhkien != null)
                {
                    linhkien.tonkho = linhkien.tonkho + dathang.soluong;
                }

                db.Entry(dathang).State = EntityState.Modified;
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            return View(dathang);
        }

        // GET: dathangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dathang dathang = db.dathangs.Find(id);
            if (dathang == null)
            {
                return HttpNotFound();
            }
            return View(dathang);
        }

        // POST: dathangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dathang dathang = db.dathangs.Find(id);
            db.dathangs.Remove(dathang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

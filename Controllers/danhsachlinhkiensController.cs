using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using WebQuanLySpare.Models;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace WebQuanLySpare.Controllers
{
    public class danhsachlinhkiensController : Controller
    {
        private WebSparePartEntities db = new WebSparePartEntities();

        // GET: danhsachlinhkiens
        public ActionResult Index(string tenmodel)
        {
            var dathang = from a in db.dathangs select a;
            List<dathang> list_dangdathang = dathang.Where(x => x.trangthai == "Đang đặt hàng").ToList();
            var tongsodathang = dathang.Where(x => x.trangthai == "Đang đặt hàng").Count();
            TempData["list_dangdathang"] = list_dangdathang;
            TempData["tongsodathang"] = tongsodathang;


            var danhsach = from a in db.danhsachlinhkiens select a;
            var tongsolinhkien = danhsach.Count();
            TempData["tongsolinhkien"] = tongsolinhkien;
            ViewBag.tenmodel = new SelectList(db.models, "tenmodel", "tenmodel"); // danh sách 

            var linhkiencandat = danhsach.Where(s => s.tonkho < 5).Count();
            TempData["linhkiencandat"] = linhkiencandat;
            List<danhsachlinhkien> list_linhkiencandat = danhsach.Where(s => s.tonkho < 5).ToList();
            TempData["list_linhkiencandat"] = list_linhkiencandat;

            if (!String.IsNullOrEmpty(tenmodel)) // kiểm tra chuỗi tìm kiếm có rỗng/null hay không
            {
                danhsach = danhsach.Where(s => s.model.Contains(tenmodel));
                TempData["danhsach"] = danhsach;
                TempData["tenmodel"] = tenmodel;

                tongsolinhkien = danhsach.Count();
                TempData["tongsolinhkien"] = tongsolinhkien;

                linhkiencandat = danhsach.Where(s => s.tonkho < 5).Count();
                TempData["linhkiencandat"] = linhkiencandat;
                list_linhkiencandat = danhsach.Where(s => s.tonkho < 5).ToList();
                TempData["list_linhkiencandat"] = list_linhkiencandat;


            }
            return View(danhsach.OrderByDescending(x=>x.tonkho));
        }

        // GET: danhsachlinhkiens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            danhsachlinhkien danhsachlinhkien = db.danhsachlinhkiens.Find(id);
            if (danhsachlinhkien == null)
            {
                return HttpNotFound();
            }
            return View(danhsachlinhkien);
        }

        // GET: danhsachlinhkiens/Create
        public ActionResult Create()
        {
            var danhsach = from a in db.danhsachlinhkiens select a;
            ViewBag.tenmodel = new SelectList(db.models, "tenmodel", "tenmodel"); // danh sách 
            return View();
        }

        // POST: danhsachlinhkiens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(danhsachlinhkien ds)
        {
            danhsachlinhkien dsm = new danhsachlinhkien();
            dsm.model = ds.model;
            dsm.tenlinhkien = ds.tenlinhkien;
            dsm.malinhkien = ds.malinhkien;
            dsm.maker = ds.maker;
            dsm.donvi = ds.donvi;
            dsm.dongia = ds.dongia;
            dsm.tonkho = ds.tonkho;
            dsm.ghichu = ds.ghichu;
            db.danhsachlinhkiens.Add(dsm);
            db.SaveChanges();
            ViewBag.msg = "Đã nhập câu hỏi thành công";
            return RedirectToAction("index");
        }

        // GET: danhsachlinhkiens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            danhsachlinhkien danhsachlinhkien = db.danhsachlinhkiens.Find(id);
            if (danhsachlinhkien == null)
            {
                return HttpNotFound();
            }
            return View(danhsachlinhkien);
        }

        // POST: danhsachlinhkiens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,model,tenlinhkien,malinhkien,hinhanh,maker,donvi,dongia,tonkho,ghichu")] danhsachlinhkien danhsachlinhkien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danhsachlinhkien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danhsachlinhkien);
        }

        // GET: danhsachlinhkiens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            danhsachlinhkien danhsachlinhkien = db.danhsachlinhkiens.Find(id);
            if (danhsachlinhkien == null)
            {
                return HttpNotFound();
            }
            return View(danhsachlinhkien);
        }

        // POST: danhsachlinhkiens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            danhsachlinhkien danhsachlinhkien = db.danhsachlinhkiens.Find(id);
            db.danhsachlinhkiens.Remove(danhsachlinhkien);
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

        public ActionResult ImportExcel()
        {   
            ViewBag.tenmodel = new SelectList(db.models, "tenmodel", "tenmodel"); // danh sách 
            return View();
        }
        public ActionResult Upload(FormCollection formCollection, danhsachlinhkien q)
        {
            ViewBag.tenmodel = new SelectList(db.models, "tenmodel", "tenmodel"); // danh sách 
            var danhsachlinhkien = new List<danhsachlinhkien>();
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var linhkienmoi = new danhsachlinhkien();
                            linhkienmoi.model = q.model.ToString();
                            if(workSheet.Cells[rowIterator, 2].Value != null)
                            {
                                linhkienmoi.tenlinhkien = workSheet.Cells[rowIterator, 2].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 2].Value == null)
                            {
                                linhkienmoi.tenlinhkien = null;
                            }
                            if (workSheet.Cells[rowIterator, 3].Value != null)
                            {
                                linhkienmoi.malinhkien = workSheet.Cells[rowIterator, 3].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 3].Value == null)
                            {
                                linhkienmoi.malinhkien = null;
                            }
                            if (workSheet.Cells[rowIterator, 4].Value != null)
                            {
                                linhkienmoi.maker = workSheet.Cells[rowIterator, 4].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 4].Value == null)
                            {
                                linhkienmoi.maker = null;
                            }
                            if (workSheet.Cells[rowIterator, 7].Value != null)
                            {
                                linhkienmoi.donvi = workSheet.Cells[rowIterator, 7].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 7].Value == null)
                            {
                                linhkienmoi.donvi = null;
                            }
                            if (workSheet.Cells[rowIterator, 5].Value != null)
                            {
                                linhkienmoi.dongia = Convert.ToInt32(workSheet.Cells[rowIterator, 5].Value.ToString());
                            }
                            if (workSheet.Cells[rowIterator, 5].Value == null)
                            {
                                linhkienmoi.dongia = null;
                            }
                            if (workSheet.Cells[rowIterator, 6].Value != null)
                            {
                                linhkienmoi.tonkho = Convert.ToInt32(workSheet.Cells[rowIterator, 6].Value.ToString());
                            }
                            if (workSheet.Cells[rowIterator, 6].Value == null)
                            {
                                linhkienmoi.tonkho = null;
                            }
                            if (workSheet.Cells[rowIterator, 8].Value != null)
                            {
                                linhkienmoi.ghichu = workSheet.Cells[rowIterator, 8].Value.ToString();
                            }
                            if (workSheet.Cells[rowIterator, 8].Value == null)
                            {
                                linhkienmoi.ghichu = null;
                            }


                            danhsachlinhkien.Add(linhkienmoi);

                        }
                    }
                }
            }
            using (WebSparePartEntities excelImportDBEntities = new WebSparePartEntities())
            {
                foreach (var item in danhsachlinhkien)
                {
                    excelImportDBEntities.danhsachlinhkiens.Add(item);
                }
                excelImportDBEntities.SaveChanges();
            }
            ViewBag.msg = "Đã nhập excel thành công";
            return View("ImportExcel");
        }
    }
}

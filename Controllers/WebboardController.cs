﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebboardMVC.Models.db;
using WebboardMVC.ViewModels;

namespace WebboardMVC.Controllers
{
    [Authorize(Roles ="Admin,Staff,Member")]
    public class WebboardController : Controller
    {
        private readonly thaivbWebboardContext _db;

        public WebboardController(thaivbWebboardContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var ks = _db.Kratoos.OrderByDescending(k => k.RecordDate).Include(c => c.Category).Where(u => u.IsShow == true);
            if (ks==null) 
            {
                return NotFound();
            }
            return View(await ks.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["CategoriesLists"] = new SelectList(_db.Categories,"CategoryId","CategoryName");
            return View();  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Kratoo data) 
        {
           
                data.RecordDate = DateTime.Now;
                data.ViewCount = 1;
                data.ReplyCount = 0;
                data.UserIp = HttpContext.Connection.RemoteIpAddress.ToString();
                data.IsShow = true;
                data.UserName = User.Identity.Name;

                _db.Kratoos.Add(data);
               await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            
            //ViewData["CategoriesLists"] = new SelectList(_db.Categories, "CategoryId", "CategoryName",data.CategoryId);
            //return View(data);
        }

        public async Task<IActionResult> kratoosByCategoryId(int id) 
        {
            var ks = _db.Kratoos.OrderByDescending(k => k.RecordDate)
                .Include(c => c.Category)
                .Where(u => u.IsShow == true)
                .Where(i=> i.CategoryId==id);
            if (ks == null)
            {
                return NotFound();
            }
            return View("Index",await ks.ToListAsync());
        }

        public async Task<IActionResult> Search(string q) 
        {
            var ks = _db.Kratoos
                .OrderByDescending(k => k.RecordDate)
                .Include(c => c.Category)
                .Where(u => u.IsShow == true)
                .Where(i=>i.KratooTopic.Contains(q));
            if (ks == null)
            {
                return NotFound();
            }
            return View("Index",await ks.ToListAsync());
        }

        public async Task<IActionResult> kratooComments(int id)
        {
            var kc =await _db.Kratoos
                .Include(c => c.Category)
                .Where(i => i.IsShow == true)
                .FirstOrDefaultAsync(k=>k.Kid==id);

            if (kc == null) 
            {
                return NotFound();
            }
            if(id != kc.Kid) 
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var viewcount = kc.ViewCount;
                    viewcount++;
                    kc.ViewCount = viewcount;
                    _db.Update(kc);
                    await _db.SaveChangesAsync();
                }
                catch (DBConcurrencyException)
                {
                    var result = _db.Kratoos.Any(k => k.Kid ==id);
                    if (result==false)
                    {
                        return NotFound();
                    }
                }
            }
            IQueryable<Comment>cs = _db.Comments
                .OrderBy(c=>c.CommentNo)
                .Where(i=>i.IsShow == true)
                .Where(j=>j.Kid==id);

            var viewmodel = new KratooCommentViewModel()
            {
                Kratoo = kc,
                CommentList= cs
            };
            ViewData["KratooCommentViewModel"] = viewmodel;
            return View("KratooComments");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> kratooComments(Comment data) 
        {
           var cs = await _db.Comments
                .OrderByDescending(i=> i.CommentNo)
                .Where(c=>c.Kid==data.Kid)
                .FirstOrDefaultAsync();
            int CurrentNO;
            if (cs!=null) 
            {
                CurrentNO =cs.CommentNo;
                CurrentNO++;
            }
            else 
            {
                CurrentNO = 1;
            }
            data.CommentNo = CurrentNO;
            data.ReplyDate = DateTime.Now;
            data.UserIp = HttpContext.Connection.RemoteIpAddress.ToString();
            data.IsShow = true;
            data.UserName = User.Identity.Name;
            _db.Add(data);

            var ks= await _db.Kratoos.Where(k=>k.Kid==data.Kid).FirstOrDefaultAsync();
            var replycount = ks.ReplyCount;
            replycount++;
            ks.ReplyCount = replycount;
            _db.Update(ks);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(kratooComments), new { id = data.Kid });

        }
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Delete()
        {
            var ks = _db.Kratoos.OrderByDescending(k => k.RecordDate).Include(c => c.Category).Where(u => u.IsShow == true);
            if (ks == null)
            {
                return NotFound();
            }
            return View(await ks.ToListAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
          
            var kratoo = await _db.Kratoos.FindAsync(id);
            if (kratoo == null)
            {
                return NotFound();
            }
            var comments = _db.Comments.Where(c => c.Kid == id);
            _db.Comments.RemoveRange(comments);

            // ลบ Kratoo
            _db.Kratoos.Remove(kratoo);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult>DeletekratooComments(int id)
        {
            var kc = await _db.Kratoos
                .Include(c => c.Category)
                .Where(i => i.IsShow == true)
                .FirstOrDefaultAsync(k => k.Kid == id);

            if (kc == null)
            {
                return NotFound();
            }
            if (id != kc.Kid)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var viewcount = kc.ViewCount;
                    viewcount++;
                    kc.ViewCount = viewcount;
                    _db.Update(kc);
                    await _db.SaveChangesAsync();
                }
                catch (DBConcurrencyException)
                {
                    var result = _db.Kratoos.Any(k => k.Kid == id);
                    if (result == false)
                    {
                        return NotFound();
                    }
                }
            }
            IQueryable<Comment> cs = _db.Comments
                .OrderBy(c => c.CommentNo)
                .Where(i => i.IsShow == true)
                .Where(j => j.Kid == id);

            var viewmodel = new KratooCommentViewModel()
            {
                Kratoo = kc,
                CommentList = cs
            };
            ViewData["KratooCommentViewModel"] = viewmodel;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int commentId, int kid)
        {
            var comment = await _db.Comments
                .FirstOrDefaultAsync(c => c.CommentNo == commentId && c.Kid == kid);
            if (comment == null || comment.UserName != User.Identity.Name)
            {
                return NotFound();
            }

            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(kratooComments), new { id = kid });
        }


        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var kratoo = await _db.Kratoos.FindAsync(id);
            if (kratoo == null || kratoo.UserName != User.Identity.Name)
            {
                return NotFound();
            }

            ViewData["CategoriesLists"] = new SelectList(_db.Categories, "CategoryId", "CategoryName", kratoo.CategoryId);
            return View(kratoo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Kratoo data)
        {
            if (id != data.Kid || data.UserName != User.Identity.Name)
            {
                return NotFound();
            }
            var existingKratoo = await _db.Kratoos.FindAsync(id);
            if (existingKratoo == null)
            {
                return NotFound();
            }
            existingKratoo.KratooTopic = data.KratooTopic;
            existingKratoo.KratooDetails = data.KratooDetails;
            existingKratoo.CategoryId = data.CategoryId;
        
            try
            {
                _db.Update(existingKratoo);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.Kratoos.Any(k => k.Kid == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }


    }
}

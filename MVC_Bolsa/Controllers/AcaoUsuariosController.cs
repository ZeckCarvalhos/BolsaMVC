using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Bolsa.Models;

namespace MVC_Bolsa.Controllers
{
    public class AcaoUsuariosController : Controller
    {
        private  MVC_BolsaContext _context;

        public AcaoUsuariosController(MVC_BolsaContext context)
        {
            _context = context;
        }

        // GET: AcaoUsuarios
        public async Task<IActionResult> Index()
        {
            var mVC_BolsaContext = _context.AcaoUsuario.Include(a => a.IdAcao).Include(a => a.IdUsuario);
            return View(await mVC_BolsaContext.ToListAsync());
        }

        // GET: AcaoUsuarios/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acaoUsuario = await _context.AcaoUsuario
                .Include(a => a.IdAcao)
                .Include(a => a.IdUsuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (acaoUsuario == null)
            {
                return NotFound();
            }

            return View(acaoUsuario);
        }

        // GET: AcaoUsuarios/Create
        public IActionResult Create()
        {
            ViewData["IdAcaoForeignKey"] = new SelectList(_context.Acao, "Id", "Nome");
            ViewData["IdUsuarioForeignKey"] = new SelectList(_context.Usuario, "Id", "Nome");
            return View();
        }

        // POST: AcaoUsuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUsuarioForeignKey,IdAcaoForeignKey,Quantidade")] AcaoUsuario acaoUsuario)
        {
            if (ModelState.IsValid)
            {
                var usuario = _context.Usuario.First(u => u.Id == acaoUsuario.IdUsuarioForeignKey);
                var acao = _context.Acao.First(a => a.Id == acaoUsuario.IdAcaoForeignKey);
                var valorTotal = acao.Preco * acaoUsuario.Quantidade;
                if (valorTotal > usuario.Saldo)
                {
                    //não pode realizar a compra
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    acaoUsuario.ValorTotal = valorTotal;
                    usuario.Saldo = usuario.Saldo - valorTotal;
                }
                _context.Add(acaoUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAcaoForeignKey"] = new SelectList(_context.Acao, "Id", "Nome", acaoUsuario.IdAcaoForeignKey);
            ViewData["IdUsuarioForeignKey"] = new SelectList(_context.Usuario, "Id", "Nome", acaoUsuario.IdUsuarioForeignKey);
            return View(acaoUsuario);
        }

        // GET: AcaoUsuarios/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acaoUsuario = await _context.AcaoUsuario.FindAsync(id);
            if (acaoUsuario == null)
            {
                return NotFound();
            }
            ViewData["IdAcaoForeignKey"] = new SelectList(_context.Acao, "Id", "Nome", acaoUsuario.IdAcaoForeignKey);
            ViewData["IdUsuarioForeignKey"] = new SelectList(_context.Usuario, "Id", "Id", acaoUsuario.IdUsuarioForeignKey);
            return View(acaoUsuario);
        }

        // POST: AcaoUsuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,IdUsuarioForeignKey,IdAcaoForeignKey,Quantidade")] AcaoUsuario acaoUsuario)
        {
            if (id != acaoUsuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(acaoUsuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcaoUsuarioExists(acaoUsuario.Id))
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
            ViewData["IdAcaoForeignKey"] = new SelectList(_context.Acao, "Id", "Nome", acaoUsuario.IdAcaoForeignKey);
            ViewData["IdUsuarioForeignKey"] = new SelectList(_context.Usuario, "Id", "Id", acaoUsuario.IdUsuarioForeignKey);
            return View(acaoUsuario);
        }

        // GET: AcaoUsuarios/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acaoUsuario = await _context.AcaoUsuario
                .Include(a => a.IdAcao)
                .Include(a => a.IdUsuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (acaoUsuario == null)
            {
                return NotFound();
            }

            return View(acaoUsuario);
        }

        // POST: AcaoUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var acaoUsuario = await _context.AcaoUsuario.FindAsync(id);
            _context.AcaoUsuario.Remove(acaoUsuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcaoUsuarioExists(long id)
        {
            return _context.AcaoUsuario.Any(e => e.Id == id);
        }
    }
}

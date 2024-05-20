using Imagens.Data;
using Imagens.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace Imagens.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly AppContext _context;

        public ProdutoController(AppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _context.Produtos.ToListAsync();
            return View(produtos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto, IList<IFormFile> imagens)
        {
            if (imagens.Count > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imagens.FirstOrDefault().CopyTo(memoryStream);
                    produto.Foto = memoryStream.ToArray();
                }
            }

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, Produto produto, IList<IFormFile> imagens)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produtoExistente = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (imagens.Count > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imagens.FirstOrDefault().CopyTo(memoryStream);
                    produto.Foto = memoryStream.ToArray();
                }
            }
            else
            {
                produto.Foto = produtoExistente.Foto;
            }

            if (ModelState.IsValid)
            {
                _context.Update(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(produto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

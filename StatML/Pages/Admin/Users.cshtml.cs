using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StatML.Models;

namespace StatML.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly AppDbContext _context;
        public const int PageSize = 10;
        public SelectList Roles { get; set; }
        public SelectList Players { get; set; }
        [BindProperty]
        public Login Login { get; set; }
        
        public UsersModel(AppDbContext context)
        {
            _context = context;
        }

        public PaginatedList<Login> Logins { get; set; }
        public async Task OnGet(int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            Logins = await PaginatedList<Login>.CreateAsync(_context.Logins.Include(x => x.RoleEntity).OrderBy(x => x.Id.ToString().ToLower()), currentPage, PageSize);

            var roles = _context.Roles.ToList();

            Roles = new SelectList(roles, "Id", "Name");

            var players = _context.Players.OrderByDescending(x => x.IsGeneral).OrderBy(x => x.PName.ToLower()).ToList();
            
            Players = new SelectList(players, "Id", "PName");
        }

        public async Task<IActionResult> OnPostSave (int? id, int? selectedPlayerId, int? selectedRoleId, int? pageIndex)
        {
            Console.WriteLine($"OnPostSave called: id={id}, selectedPlayerId={selectedPlayerId}, selectedRoleId={selectedRoleId}, pageIndex={pageIndex}");

            if (!id.HasValue)
            {
                Console.WriteLine("Invalid id, returning to page");
                return Page();
            }

            int currentPage = pageIndex ?? 1;

            if (id == null)
            {
                return NotFound();
            }

            int roleId = 0;
            

            var login = await _context.Logins.FindAsync(id);

            login.RoleId = (int)selectedRoleId;

            if (login != null)
            {
                Console.WriteLine($"Found login {login.Id}: RoleId={login.RoleId}, PlayerId={login.PlayerId}");
                if (selectedPlayerId.HasValue) login.PlayerId = selectedPlayerId.Value;
                if (selectedRoleId.HasValue) login.RoleId = selectedRoleId.Value;

                try
                {
                    //_context.Logins.Update(login);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при удалении: " + ex.Message);
                    
                    Logins = await PaginatedList<Login>.CreateAsync(_context.Logins.Include(x => x.RoleEntity).OrderBy(x => x.Id.ToString().ToUpper()), currentPage, PageSize);

                    var roles = _context.Roles.ToList();

                    Roles = new SelectList(roles, "Id", "Name");
                    return Page();
                }
            }

            return RedirectToPage("/Admin/Users");
        }

        public async Task<IActionResult> OnPostDelete(int? id, int? pageIndex)
        {
            int currentPage = pageIndex ?? 1;

            if (id == null)
            {
                return NotFound();
            }

            var login = _context.Logins.Find(id);

            if (login != null)
            {
                try
                {
                    _context.Logins.Remove(login);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при удалении: " + ex.Message);
                    
                    Logins = await PaginatedList<Login>.CreateAsync(_context.Logins.Include(x => x.RoleEntity).Include(x => x.PlayerEntity)
                        .OrderBy(x => x.Id.ToString().ToUpper()), currentPage, PageSize);

                    var roles = _context.Roles.ToList();

                    Roles = new SelectList(roles, "Id", "Name");
                    return Page();
                }
            }

            return RedirectToPage("/Players/Management");
        }
    }
}

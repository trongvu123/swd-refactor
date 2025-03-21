using Microsoft.AspNetCore.Mvc;
using SonicStore.Repository.Entity;

namespace SonicStore.Areas.SonicStore.Dtos;

public class CompositeViewModel
{
    [BindProperty]
    public Account AccountModel { get; set; }

    [BindProperty]
    public User UserModel { get; set; }

   
}

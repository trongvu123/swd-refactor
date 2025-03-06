using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SonicStore.Areas.SonicStore.Dtos;
using SonicStore.Repository.Entity;

namespace SonicStore.Areas.SonicStore.Controllers.AddressManage;

[Authorize(Roles = "customer")]
[Area("SonicStore")]
public class AddressController : Controller
{
    private readonly SonicStoreContext _context;

    public AddressController(SonicStoreContext context)
    {
        _context = context;
    }
    [HttpGet("change-adress")]
    public async Task<IActionResult> CartAdressScreen()
    {
        return View();
    }
    [HttpGet("loadDataAddress")]
    public async Task<JsonResult> loadAddress()
    {
        var userJson = HttpContext.Session.GetString("user");
        var userSession = JsonConvert.DeserializeObject<User>(userJson);
        var addressDefault = await _context.UserAddresses.Where(u => u.User.Id == userSession.Id && u.Status == true).Select(u => u.User_Address).FirstOrDefaultAsync();
        var listAddressUser = await _context.UserAddresses.Where(u => u.User.Id == userSession.Id).Include(u => u.User).Select(u => new
        {
            u.Id,
            u.User.FullName,
            u.User_Address,
            u.User.Phone,
            u.Status

        }).ToListAsync();
        return Json(new
        {
            data = listAddressUser,
            status = true
        });
    }
    [HttpPost("save-address")]
    public async Task<JsonResult> saveAddress(string strAddress)
    {
        bool status = true;
        var userJson = HttpContext.Session.GetString("user");
        var userSession = JsonConvert.DeserializeObject<User>(userJson);
        var address = JsonConvert.DeserializeObject<AddressInput>(strAddress);
        string fullAddress = $"{address.xa}, {address.huyen}, {address.tinh}";
        var userAddress = await _context.UserAddresses.FindAsync(address.id);
        var addressExist = await _context.UserAddresses.Where(u => u.Id != address.id && u.User_Address.Equals(fullAddress) && u.UserId == userSession.Id).FirstOrDefaultAsync();
        if (userAddress.Status == true)
        {
            if (address.check == false)
            {
                status = true;
            }
            else
            {
                if (addressExist != null)
                {
                    status = false;
                }
                else
                {
                    if (address.check == true)
                    {
                        foreach (var item in _context.UserAddresses)
                        {
                            if (item.Id != address.id && item.Status == true)
                            {
                                item.Status = false;
                                _context.Update(item);
                                break;
                            }
                        }
                        userAddress.User_Address = fullAddress;
                        userAddress.Status = true;
                        _context.Update(userAddress);

                    }
                    else
                    {
                        userAddress.User_Address = fullAddress;
                        _context.Update(userAddress);
                    }
                    await _context.SaveChangesAsync();
                }
            }
        }
        return Json(new { status = status });
    }
    [HttpPost("change-default")]
    public async Task<JsonResult> ChangeDefaultAdress(int id)
    {
        var userId = 1;
        var address = await _context.UserAddresses.FindAsync(id);
        address.Status = true;
        foreach (var item in _context.UserAddresses)
        {
            if (item.Id != id)
            {
                item.Status = false;
            }
        }
        await _context.SaveChangesAsync();
        return Json(new { status = true });
    }
    [HttpPost("delete-address")]
    public async Task<JsonResult> DeleteAddress(int delete)
    {
        var addressUser = await _context.UserAddresses.FindAsync(delete);
        if (addressUser != null)
        {
            _context.UserAddresses.Remove(addressUser);
        }
        await _context.SaveChangesAsync();
        return Json(new { status = true });
    }
    [HttpPost("add-address")]
    public async Task<JsonResult> AddNewAddress(string strAddress)
    {
        bool status = true;
        try
        {
            var userJson = HttpContext.Session.GetString("user");
            var userSession = JsonConvert.DeserializeObject<User>(userJson);
            var address = JsonConvert.DeserializeObject<AddressInputAdding>(strAddress);
            string fullAddress = $"{address.xa}, {address.huyen}, {address.tinh}";
            var addressExist = await _context.UserAddresses.Where(u => u.User_Address.Equals(fullAddress) && u.UserId == userSession.Id).FirstOrDefaultAsync();
            if (addressExist == null)
            {
                if (address.check == true)
                {
                    foreach (var item in _context.UserAddresses)
                    {
                        if (item.Status == true)
                        {
                            item.Status = false;
                            _context.Update(item);
                            break;
                        }
                    }
                    var newAddress = new UserAddress
                    {
                        User_Address = fullAddress,
                        UserId = userSession.Id,
                        Status = true
                    };
                    _context.UserAddresses.Add(newAddress);
                }
                else
                {
                    var newAddress = new UserAddress
                    {
                        User_Address = fullAddress,
                        UserId = userSession.Id,
                        Status = false
                    };
                    _context.UserAddresses.Add(newAddress);
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                status = false;
            }

        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
        }
        return Json(new { status = status });
    }
}

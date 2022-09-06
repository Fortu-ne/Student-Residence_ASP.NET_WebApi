using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using JulyProject.Helper;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using WepApiWithToken.Interface;
using WepApiWithToken.Model;

namespace WepApiWithToken.Service
{
    //public interface IUserService
    //{
    //    string GetMyName();
    //    bool UserExist(string user);

    //    User GetUser(string name);
    //}


    //public class UserService : IUserService
    //{
    //    private readonly IHttpContextAccessor _httpContextAccessor;
    //    private readonly AppDbContext context;

    //    public UserService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
    //    {
    //        _httpContextAccessor = httpContextAccessor;
    //        this.context = context;
    //    }

    //    public string GetMyName()
    //    {
    //        var result = string.Empty;
    //        if (_httpContextAccessor.HttpContext != null)
    //        {
    //            result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
    //        }
    //        return result;
    //    }

    //    public User GetUser(string email)
    //    {
    //        return context.Users.FirstOrDefault(r => r.Email == email);
    //    }

    //    public bool UserExist(string user)
    //    {
    //        return context.Users.Any(r => r.Email == user);
    //    }
    //}

    
}

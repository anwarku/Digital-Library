using Backend.DTOs;
using Backend.Models;

namespace Backend.Services.Interfaces
{
    public interface IMemberService
    {
        List<Member> GetAllMembers();
        Member GetMemberById(int id);
        MemberCheckDto GetMemberCheckById(int id);
        void Add(AddMemberDto addMemberDto, IFormFile imageFile);
        void Delete(int id);
        bool Exist(int id);
    }
}

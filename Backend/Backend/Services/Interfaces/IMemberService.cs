using Backend.DTOs;
using Backend.Models;

namespace Backend.Services.Interfaces
{
    public interface IMemberService
    {
        List<Member> GetAllMembers();
        Member GetMemberById(int id);
        void Add(AddMemberDto addMemberDto);
        void Delete(int id);
        bool Exist(int id);
    }
}

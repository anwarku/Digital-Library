using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services.Interfaces;

namespace Backend.Services
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContext _context;

        public MemberService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Member> GetAllMembers()
        {
            return _context.Members.ToList();
        }

        public Member GetMemberById(int id)
        {
            return _context.Members.FirstOrDefault(m => m.Id == id);
        }

        public void Add(AddMemberDto addMemberDto)
        {
            var member = new Member
            {
                Name = addMemberDto.Name,
                Gender = addMemberDto.Gender,
                Phone = addMemberDto.Phone,
                Job = addMemberDto.Job
            };

            _context.Members.Add(member);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existMember = GetMemberById(id);
            if (existMember != null)
            {
                _context.Members.Remove(existMember);
                _context.SaveChanges();
            }
        }

        public bool Exist(int id)
        {
            return _context.Members.Any(m => m.Id == id);
        }
    }
}

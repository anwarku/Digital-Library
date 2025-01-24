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

        public MemberCheckDto GetMemberCheckById(int id)
        {
            var member = _context.Members.Find(id);

            // Mengecek apakah member ada dalam database
            if (member == null)
            {
                throw new Exception("Member is not found");
            }

            var checkTransaction = _context.Transactions.Any(t => t.MemberId == member.Id && t.Status == "Borrowed");

            // Mengecek jika member sudah ada transaksi yang belum dikembalikan
            if (checkTransaction) 
            {
                throw new Exception("Members still have borrowed books that have not been returned");
            }

            // Jika valid
            MemberCheckDto validMember = _context.Members
                .Where(m => m.Id == member.Id)
                .Select(member => new MemberCheckDto
                { Id = member.Id, Name = member.Name, Phone = member.Phone })
                .First();

            return validMember;
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

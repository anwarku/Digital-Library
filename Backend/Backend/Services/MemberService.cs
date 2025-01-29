using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services.Interfaces;

namespace Backend.Services
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public MemberService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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

        public void Add(AddMemberDto addMemberDto, IFormFile imageFile)
        {
            try
            {
                var member = new Member
                {
                    Name = addMemberDto.Name,
                    Gender = addMemberDto.Gender,
                    Phone = addMemberDto.Phone,
                    Job = addMemberDto.Job
                };

                if (imageFile != null)
                {
                    string fileNameImage = SaveImageLocal(imageFile);
                    member.Image = fileNameImage;
                }

                _context.Members.Add(member);
                _context.SaveChanges();
            }
            catch (Exception ex) 
            {
                throw new Exception($"Failed to save : {ex.Message}");
            }
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

        private string SaveImageLocal(IFormFile imageFile)
        {
            try
            {
                var extFile = Path.GetExtension(imageFile.FileName);
                var fileName = Guid.NewGuid() + extFile;
                var wwwRootPath = Path.Combine(_environment.WebRootPath, "images");
                var imagePath = Path.Combine(wwwRootPath, fileName);

                // Validasi file ekstensi
                var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
                if (!allowedExtensions.Contains(extFile))
                {
                    throw new Exception("File is not supported!");
                }

                // Validasi file ukuran
                if (imageFile.Length > MegaToByte(2))
                {
                    throw new Exception("Size image so large!");
                }

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                return fileName;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        private long MegaToByte(double megaSize)
        {
            return (long)(megaSize * 1048576);
        }
    }
}

using Backend.DTOs;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/members")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public ActionResult<List<Member>> GetAllBooks()
        {
            var members = _memberService.GetAllMembers();

            return Ok(members);
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<Member> GetMemberById(int id)
        {
            var member = _memberService.GetMemberById(id);
            if (member == null)
            {
                return NotFound(new
                {
                    Message = "Member is not found!"
                });
            }
            return Ok(member);
        }

        [HttpGet]
        [Route("check/{id:int}")]
        public ActionResult<MemberCheckDto> GetMemberCheck(int id)
        {
            try
            {
                var memberCheck = _memberService.GetMemberCheckById(id);

                return Ok(memberCheck);
            }
            catch (Exception ex) 
            {
                return BadRequest(new {Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<Member> StoreMember([FromForm] AddMemberDto addMemberDto, IFormFile imageFile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _memberService.Add(addMemberDto, imageFile);
                return Created();
            }
            catch (Exception ex) 
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteMember(int id)
        {
            if (!_memberService.Exist(id))
            {
                return NotFound();
            }
            _memberService.Delete(id);
            return NoContent();
        }
    }
}

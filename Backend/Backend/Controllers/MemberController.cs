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
                return NotFound();
            }
            return Ok(member);
        }

        [HttpPost]
        public ActionResult<Member> StoreMember([FromBody] AddMemberDto addMemberDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _memberService.Add(addMemberDto);
            return Created();
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

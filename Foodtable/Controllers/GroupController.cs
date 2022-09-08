using Foodtable.Models;
using Microsoft.AspNetCore.Mvc;

namespace Foodtable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class GroupController : ControllerBase
    {

        private readonly DataContext _context;

        public GroupController(DataContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Put User in a Group
        /// </summary>
        /// <response code="200">User succesfully added in Group</response>
        /// <response code="404">Group or User not Found</response>
        [HttpPut("{groupId}/User/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Group>> PutUserInGroup(int groupId, int userId)
        {
            var dbGroup = await _context.Groups
                .Include(u => u.Users)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);

            var dbUser = await _context.Users.FindAsync(userId);

            if (dbGroup == null || dbUser == null) { return NotFound("Sorry invalid paramaters, pleaser provide a valid GroupID or valid UserId"); }
            else if (!dbGroup.Users.Contains(dbUser))
            {
                dbGroup.Users.Add(dbUser);
                await _context.SaveChangesAsync();
                return Ok(dbGroup);
            }
            return BadRequest("User already in group.");
        }




        /// <summary>
        /// Delete a User from a Group
        /// </summary>
        /// <response code="202">User succesfully deleted from Group</response>
        /// <response code="404">Group or User not Found</response>
        [HttpDelete("{groupId}/User/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Group>> DeleteUserFromGroup(int groupId, int userId)
        {
            var dbGroup = await _context.Groups
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);

            var dbUser = await _context.Users.FindAsync(userId);
            if (dbGroup == null || dbUser == null) { return NotFound("Sorry invalid paramaters, pleaser provide a valid GroupID or valid UserId"); }
            else if (dbGroup.Users.Contains(dbUser))
            {
                dbGroup.Users.Remove(dbUser);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound("User doesn't exists in Group");
        }


        /// <summary>
        /// Delete a Group
        /// </summary>
        /// <response code="204">Group succesfully deleted</response>
        /// <response code="404">Group not Found</response>
        [HttpDelete("{groupId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Group>> DeleteGroup(int groupId)
        {
            var dbGroup = await _context.Groups.FindAsync(groupId);

            if (dbGroup == null) { return NotFound("Sorry invalid paramaters, pleaser provide a valid GroupID"); }

            _context.Groups.Remove(dbGroup);
            await _context.SaveChangesAsync();
            return NoContent();
        }



        /// <summary>
        /// Get All User from a Group
        /// </summary>
        /// <response code="200">All User succesfully received from Group</response>
        /// <response code="404">Group not Found</response>
        [HttpGet("{groupId}/Users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Group>> GetAllUserFromGroup(int groupId)
        {
            var dbGroup = await _context.Groups
                 .Where(g => g.GroupId == groupId)
                 .Include(u => u.Users)
                 .ToListAsync();

            if (dbGroup.Find(g => g.GroupId == groupId) == null) { return NotFound("Sorry invalid paramaters, please provide a valid GroupID and valid UserId"); }
            return Ok(dbGroup);
        }


        /// <summary>
        /// Count All Users from a Group
        /// </summary>
        /// <response code="200">All Users counted succesfully</response>
        /// <response code="404">Group not Found</response>
        [HttpGet("{groupId}/CountUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Group>> CountAllUsersFromAGroup(int groupId)
        {
            var dbGroup = _context.Groups
                 .Where(g => g.GroupId == groupId)
                 .SelectMany(u => u.Users)
                 .Count();

            if (_context.Groups.Find(groupId) == null) { return NotFound("Sorry invalid paramater, please provide a valid GroupID"); }
            return Ok(dbGroup);
        }



        /// <summary>
        /// Get All Existing Groups
        /// </summary>
        /// <response code="200">All Groups succesfully received</response>
        /// <response code="404">No Groups existing</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Group>>> GetAllExistingGroups()
        {
            var dbGroups = await _context.Groups.ToListAsync();
            if (dbGroups.Count == 0) { return NotFound("Sorry no Groups existing"); }
            return Ok(dbGroups);
        }



        /// <summary>
        /// Get a Active Group
        /// </summary>
        /// <response code="200">Group succesfully received</response>
        /// <response code="404">Group not Found</response>
        [HttpGet("{groupId}/Active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Group>>> GetActiveGroup(int groupId)
        {
            var dbGroups = await _context.Groups
                .Where(v => v.ValidUntil > DateTime.Now || v.ValidUntil == null)
                .FirstOrDefaultAsync(g => g.GroupId == groupId);

            if (dbGroups == null) { return NotFound("Sorry invalid Parameters, GroupId not Found"); }
            return Ok(dbGroups);
        }




        /// <summary>
        /// Create a Group
        /// </summary>
        /// <response code="201">Group succesfully created </response>
        /// <response code="400">Can't create Group</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Group>>> CreateANewGroup(GroupDTO createGroup)
        {

            if (createGroup.GroupName.Length > 70) { return BadRequest("GroupName is too long [MAX 70 Characters]"); }
            Random random = new Random();

            var newCreatedGroup = new Group()
            {
                GroupName = createGroup.GroupName,
                GroupType = createGroup.GroupType,
                EndOfOrderTime = createGroup.EndOfOrderTime,
                Created = DateTime.Now
            };

            if (newCreatedGroup.GroupType == GroupType.OneDay){ newCreatedGroup.ValidUntil = newCreatedGroup.Created.AddDays(1);}

            int randomGroupId;
            Group? checkIfGroupIdIsExisting;
            do
            {
                randomGroupId = random.Next(999_999_99, 100_000_000_0);  //Group number should have 9 digits 
                checkIfGroupIdIsExisting = await _context.Groups.FindAsync(randomGroupId);

            } while (checkIfGroupIdIsExisting != null);

            newCreatedGroup.GroupId = randomGroupId;
            var dbGroup = _context.Groups.Add(newCreatedGroup);

            await _context.SaveChangesAsync();
            return StatusCode(201, dbGroup.Entity);
        }


        /// <summary>
        /// Update a Group
        /// </summary>
        /// <response code="200">Group succesfully updatet </response>
        /// <response code="404">Group not found</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Group>>> UpdateAGroup(GroupDTO updateGroup)
        {
            var dbGroup = await _context.Groups.FindAsync(updateGroup.GroupId);
            if (dbGroup == null)
            {
                return NotFound("Sorry Group not found please use the Correct GroupId");
            }
            else if (updateGroup.GroupName.Length > 70)
            {
                return BadRequest("Group Name is to long [MAX 70 Characters]");
            }

            dbGroup.GroupName = updateGroup.GroupName;
            dbGroup.GroupType = updateGroup.GroupType;
            dbGroup.EndOfOrderTime = updateGroup.EndOfOrderTime;

            await _context.SaveChangesAsync();
            return Ok(dbGroup);
        }



        /// <summary>
        /// Get Group Order and whole UserOrders at the Date
        /// </summary>
        /// <response code="200">Order received succesfully</response>
        /// <response code="404">Can't find Order</response>
        [HttpGet("{groupId}/Order{orderId}/{orderDate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Order>>> GetWholeOrderFromOneDay(int groupId, int orderId, DateTime orderDate)
        {
            var dbGroup = _context.Groups.Find(groupId);
            if (dbGroup == null) { return NotFound("Sorry Invalid Parameter, GroupId doesn't exist"); }

            var dbGroupOrder = await _context.Groups
                .Where(g => g.GroupId == groupId)
                .Include(o => o.Orders
                .Where(o => o.OrderDate == orderDate)
                .Where(o => o.OrderId == orderId))
                .ThenInclude(o => o.UserOrders
                .Where(u => u.OrderId == orderId))
                .ToListAsync();


            var dbOrder = _context.Orders.Find(orderId);
            if (dbOrder == null) { return NotFound("Invalid Parameter, OrderId doesn't exist"); }

            return Ok(dbGroupOrder);
        }



        /// <summary>
        /// Count All User Orders from a Group at the Date
        /// </summary>
        /// <response code="200">All users counted succesfully</response>
        /// <response code="404">Group not Found</response>
        [HttpGet("{groupId}/Order/{orderId}/CountUserOrders/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Group>> CountAllUsersFromAGroupAtTheDate(int groupId, int orderId, DateTime date)
        {
            var dbCountedGroup = _context.Groups
                .Where(g => g.GroupId == groupId)
                .Select(o => o.Orders
                .Where(o => o.OrderId == orderId)
                .Where(d => d.OrderDate == date)
                .SelectMany(u => u.UserOrders
                .Where(o => o.OrderId == orderId))
                .Count());

            var checkIfOrderExists = _context.Orders
                .Where(g => g.GroupId == groupId)
                .Where(o => o.OrderId == orderId)
                .FirstOrDefault(d => d.OrderDate == date);

            if (checkIfOrderExists == null) { return NotFound("Sorry invalid paramater, please provide a valid GroupID, OrderId or Date"); }
            return Ok(dbCountedGroup);
        }

    }
}

using Foodtable.Models;
using Microsoft.AspNetCore.Mvc;

namespace Foodtable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get a User
        /// </summary>
        /// <response code="200">User succesfully reiceved</response>
        /// <response code="404">User not Found</response>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUser(int userId)
        {
            var dbUser = await _context.Users
                .Where(u => u.UserId == userId)
                .ToListAsync();

            if (dbUser == null) { return NotFound("Sorry User not found please use the Correct UserId"); }
            return Ok(dbUser);
        }


        /// <summary>
        /// Get All Existing Users
        /// </summary>
        /// <response code="200">All Users succesfully reiceved</response>
        /// <response code="404">No Users exisitng</response>
        [HttpGet("AllDbUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<User>>> Get()
        {
            var dbUsers = await _context.Users.ToListAsync();
            if (dbUsers.Count == 0) { return NotFound("Sorry no Users existing"); }
            return Ok(dbUsers);
        }




        /// <summary>
        /// Create a new User
        /// </summary>
        /// <response code="201">User succesfully created</response>
        /// <response code="400">Can't create User</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<User>>> AddUser(UserDTO createUser)
        {
            if (createUser.FirstName.Length > 25 || createUser.LastName.Length > 25)
            {
                return BadRequest("FirstName or LastName is too long [MAX 25 Characters] per Name");
            }

            var newCreatedUser = new User()
            {
                UserId = 0,
                FirstName = createUser.FirstName,
                LastName = createUser.LastName,
            };

            newCreatedUser.UserId = 0;
            var dbUser = _context.Users.Add(newCreatedUser);
            await _context.SaveChangesAsync();
            return StatusCode(201, dbUser.Entity);
        }




        /// <summary>
        /// Update a User
        /// </summary>
        /// <response code="200">User succesfully updatet</response>
        /// <response code="404">User not Found</response>
        /// <response code="400">Can't update User</response>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<User>>> UpdateUser(UserDTO updateUser, int userId)
        {

            var dbUser = await _context.Users.FindAsync(userId);

            if (dbUser == null) { return NotFound("Sorry User not found please use the Correct UserId"); }
            else if (updateUser.FirstName.Length > 50 || updateUser.LastName.Length > 50)
            {
                return BadRequest("FirstName or LastName is to long [MAX 25 Characters] per Name");
            }

            dbUser.FirstName = updateUser.FirstName;
            dbUser.LastName = updateUser.LastName;

            await _context.SaveChangesAsync();
            return Ok(dbUser);
        }




        /// <summary>
        /// Delete a User
        /// </summary>
        /// <response code="204">User succesfully deleted</response>
        /// <response code="404">User not Found</response>
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<User>>> DeleteUser(int userId)
        {
            var dbUser = await _context.Users.FindAsync(userId);
            if (dbUser == null) { return NotFound("Sorry User not found please use the Correct UserId"); }

            _context.Users.Remove(dbUser);
            await _context.SaveChangesAsync();
            return NoContent();
        }




        /// <summary>
        /// Get All Groups Where User is Joined
        /// </summary>
        /// <response code="200">All Groups from User succesfully received</response>
        /// <response code="404">User Groups not Found</response>
        [HttpGet("{userId}/AllGroups")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Group>> GetAllUserGroups(int userId)
        {
            var dbUserGroups = await _context.Users
                    .Where(u => u.UserId == userId)
                    .Include(g => g.Groups)
                    .ToListAsync();

            if (dbUserGroups.Find(u => u.UserId == userId) == null) { return NotFound("Sorry Invalid Parameters, please enter a Correct UserId"); }
            return Ok(dbUserGroups);
        }




        /// <summary>
        /// Get All Active Groups Where User is Joined
        /// </summary>
        /// <response code="200">All Active Groups from User succesfully received</response>
        /// <response code="404">Can't find Active Groups</response>
        [HttpGet("{userId}/AllActiveGroups")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Group>> GetAllActiveUserGroups(int userId)
        {
            var dbActiveGroups = _context.Users
                .Where(u => u.UserId == userId)
                .Include(g => g.Groups
                .Where(v => v.ValidUntil > DateTime.Now || v.ValidUntil == null))
                .ToList();

            if (dbActiveGroups.Find(u => u.UserId == userId) == null) { return NotFound("Sorry Invalid Parameters, please enter a Correct UserId"); }
            return Ok(dbActiveGroups);
        }




        /// <summary>
        /// Get all Orders from User
        /// </summary>
        /// <response code="200">UserOrder received succesfully</response>
        /// <response code="404">Can't find User Order</response>
        [HttpGet("{userId}/AllUserOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<UserOrder>>> GetAllUserOrdersFromUser(int userId)
        {
            var userOrders = await _context.UserOrders
                .Where(u => u.UserId == userId)
                .ToListAsync();

            if (userOrders.Find(u => u.UserId == userId) == null) { return NotFound("Sorry Invalid Parameters, please enter a Correct UserId"); }
            return Ok(userOrders);
        }
    }
}


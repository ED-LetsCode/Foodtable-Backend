using Foodtable.Models;
using Microsoft.AspNetCore.Mvc;

namespace Foodtable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Create a new Order 
        /// </summary>
        /// <response code="201">Order created succesfully</response>
        /// <response code="404">Can't find GroupId or UserId </response>
        /// <response code="400">Can't create Order</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Order>>> CreateOrder(OrderDTO createOrder)
        {

            if (createOrder.RestaurantName.Length > 50) { return BadRequest("RestaurantName is to long [MAX 50 Characters]"); }
            else if (createOrder.EatingSelection.Length > 100) { return BadRequest("EatingSelection is to long [MAX 100 Characters]"); }

            var dbGroup = _context.Groups
                .Include(g => g.Users)
                .FirstOrDefault(g => g.GroupId == createOrder.GroupId);

            if (dbGroup == null) { return NotFound("Sorry Invalid Parameters, please use a Corecct GroupId"); }
            else if (!dbGroup.Users.Exists(u => u.UserId == createOrder.UserId)) { return NotFound("User not attached to Group, use a valid UserId"); }

            var checkIfOrderExists = await _context.Orders
                .Where(g => g.GroupId == createOrder.GroupId)
                .Where(d => d.OrderDate == createOrder.OrderDate)
                .FirstOrDefaultAsync();

            if (checkIfOrderExists != null) { return BadRequest("Can't Create Order because at this Day already a Order exists"); }

            var newCreatedOrder = new Order()
            {
                OrderId = 0,
                GroupId = createOrder.GroupId,
                EatingSelection = createOrder.EatingSelection,
                RestaurantName = createOrder.RestaurantName,
                OrderDate = createOrder.OrderDate
            };

            var addedDbOrder = _context.Orders.Add(newCreatedOrder);
            await _context.SaveChangesAsync();
            return StatusCode(201, addedDbOrder.Entity);
        }



        /// <summary>
        /// Delete a Order
        /// </summary>
        /// <response code="204">Order deleted succesfully</response>
        /// <response code="404">Can't delete Order</response>
        [HttpDelete("{orderId}/Group/{groupId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Order>>> DeleteAOrder(int groupId, int orderId)
        {
            var dbGroup = _context.Groups
                .Include(g => g.Users)
                .FirstOrDefault(g => g.GroupId == groupId);

            if (dbGroup == null) { return NotFound("Sorry Invalid Parameter, please use a Corect GroupId"); }
            var dbGroupOrder = _context.Orders.Where(o => o.OrderId == orderId).FirstOrDefault();

            if (dbGroupOrder == null) { return NotFound("Sorry Invalid Parameter, OrderId doesn't exist"); }
            _context.Orders.Remove(dbGroupOrder);

            await _context.SaveChangesAsync();
            return NoContent();
        }


        /// <summary>
        /// Get All existing Orders
        /// </summary>
        /// <response code="200">Order received succesfully</response>
        /// <response code="404">Can't find Orders</response>
        [HttpGet("AllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<UserOrder>>> GetAllExistingOrders()
        {
            var dbUserOrder = await _context.Orders.ToListAsync();
            if (dbUserOrder == null) { return NotFound("Sorry no Orders existing"); }
            return Ok(dbUserOrder);
        }



        /// <summary>
        /// Update a Order
        /// </summary>
        /// <response code="200">Order updated succesfully</response>
        /// <response code="404">Can't find Order</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut]
        public async Task<ActionResult<List<Order>>> UpdateOrder(OrderDTO updateOrder)
        {
            if (updateOrder.RestaurantName.Length > 50) { return BadRequest("RestaurantName is to long [MAX 50 Characters]"); }
            else if (updateOrder.EatingSelection.Length > 100) { return BadRequest("EatingSelection is to long [MAX 100 Characters]"); }

            var dbGroup = _context.Groups
                .Include(g => g.Users)
                .FirstOrDefault(g => g.GroupId == updateOrder.GroupId);

            if (dbGroup == null) { return NotFound("Sorry Invalid Parameter, please use a Corect GroupId"); }

            var dbGroupOrder = _context.Orders.Where(o => o.OrderId == updateOrder.OrderId).FirstOrDefault();
            if (dbGroupOrder == null) { return NotFound("Invalid Parameter, OrderId doesn't exist"); }

            dbGroupOrder.EatingSelection = updateOrder.EatingSelection;
            dbGroupOrder.RestaurantName = updateOrder.RestaurantName;

            await _context.SaveChangesAsync();
            return Ok(dbGroupOrder);
        }




        /// <summary>
        /// Get only a Order
        /// </summary>
        /// <response code="200">Order received succesfully</response>
        /// <response code="404">Can't find Order</response>
        [HttpGet("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Order>>> GetAOrder(int orderId)
        {
            var dbGroupOrder = await _context.Orders.FindAsync(orderId);
            if (dbGroupOrder == null) { return NotFound("Invalid Parameter, OrderId doesn't exist"); }
            return Ok(dbGroupOrder);
        }



        //  User Orders Table
        //      ____
        //      |  |
        //      |  |
        //     _|  |_
        //     \    /
        //      \  /
        //       \/





        /// <summary>
        /// Update User Order for a Order
        /// </summary>
        /// <response code="200">User Order updatet succesfully</response>
        /// <response code="400">Can't update User Order</response>
        /// <response code="404">Can't find User Order</response>
        [HttpPut("UserOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<UserOrder>>> UpdateUserOrder(UserOrderDTO updateUserOrder)
        {
            if (updateUserOrder.ProductName.Length > 100) { return BadRequest("ProductName is to long [MAX 100 Characters]"); }

            var dbUserOrder = await _context.UserOrders
                        .Where(u => u.UserId == updateUserOrder.UserId)
                        .Where(o => o.OrderId == updateUserOrder.OrderId)
                        .Where(u => u.UserOrderId == updateUserOrder.UserOrderId)
                        .FirstOrDefaultAsync();

            if (dbUserOrder == null) { return NotFound("Sorry Invalid Parameters, please use a Corecct GroupId or UserId"); }

            dbUserOrder.ProductName = updateUserOrder.ProductName;
            dbUserOrder.AmountOfProduct = updateUserOrder.AmountOfProduct;

            await _context.SaveChangesAsync();
            return Ok(dbUserOrder);
        }



        /// <summary>
        /// Create UserOrder for a Order
        /// </summary>
        /// <response code="201">User Order created succesfully</response>
        /// <response code="404">Can't find Order</response>
        /// <response code="400">Can't create User Order</response>
        [HttpPost("UserOrder")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<UserOrder>>> CreateUserOrder(UserOrderDTO createUserOrder)
        {
            if (createUserOrder.ProductName.Length > 100) { return BadRequest("ProductName is to long [MAX 100 Characters]"); }

            var dbGroup = await _context.Groups
                        .Include(u => u.Users)
                        .FirstOrDefaultAsync(g => g.GroupId == createUserOrder.GroupId);

            if (dbGroup == null) { return NotFound("Sorry Invalid Parameters, please use a Corect GroupId"); }
            else if (!dbGroup.Users.Exists(u => u.UserId == createUserOrder.UserId)) { return NotFound("User not attached to Group, use a valid UserId"); }

            var dbUserOrder = await _context.Orders
                    .Where(o => o.OrderId == createUserOrder.OrderId)
                    .FirstOrDefaultAsync();

            if (dbUserOrder == null) { return NotFound("Sorry Invalid Parameter please use a Corecct OrderId"); }


            var createdDBUserOrder = new UserOrder()
            {
                UserOrderId = 0,
                OrderId = createUserOrder.OrderId,
                UserId = createUserOrder.UserId,
                AmountOfProduct = createUserOrder.AmountOfProduct,
                ProductName = createUserOrder.ProductName
            };

            var newCreatedDBOrderUser = _context.UserOrders.Add(createdDBUserOrder);
            await _context.SaveChangesAsync();
            return StatusCode(201, newCreatedDBOrderUser.Entity);
        }


        /// <summary>
        /// Delete User Order from a Order
        /// </summary>
        /// <response code="204">Order deleted succesfully</response>
        /// <response code="404">Can't find Order</response>
        [HttpDelete("{orderId}/UserOrder/{userOrderId}/User/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<UserOrder>>> DeleteUserOrder(int orderId, int userOrderId, int userId)
        {
            var dbUserOrder = await _context.UserOrders
                          .Where(u => u.UserId == userId)
                          .Where(o => o.OrderId == orderId)
                          .Where(u => u.UserOrderId == userOrderId)
                          .FirstOrDefaultAsync();

            if (dbUserOrder == null) { return NotFound("Sorry Invalid Parameters, please use a Corect GroupId, UserId or UserOrderId"); }
            _context.UserOrders.Remove(dbUserOrder);
            await _context.SaveChangesAsync();
            return NoContent();
        }



        /// <summary>
        /// Get a User Order from Order
        /// </summary>
        /// <response code="200">Order received succesfully</response>
        /// <response code="404">Can't find Order</response>
        [HttpGet("{orderId}/UserOrder/{userOrderId}/User/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<UserOrder>>> GetUserOrder(int userId, int orderId, int userOrderId)
        {
            var dbUserOrder = await _context.UserOrders
                        .Where(u => u.UserId == userId)
                        .Where(o => o.OrderId == orderId)
                        .Where(u => u.UserOrderId == userOrderId)
                        .FirstOrDefaultAsync();

            if (dbUserOrder == null) { return NotFound("Sorry Invalid Parameters, please use a Corect GroupId, UserId or UserOrderId"); }
            return Ok(dbUserOrder);
        }




        /// <summary>
        /// Get All existing UserOrders
        /// </summary>
        /// <response code="200">Order received succesfully</response>
        /// <response code="404">Can't find Order</response>
        [HttpGet("AllUserOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<UserOrder>>> GetAllExistingUserOrders()
        {
            var dbUserOrder = await _context.UserOrders.ToListAsync();
            if (dbUserOrder == null) { return NotFound("Sorry no UserOrders existing"); }
            return Ok(dbUserOrder);
        }

    }
}


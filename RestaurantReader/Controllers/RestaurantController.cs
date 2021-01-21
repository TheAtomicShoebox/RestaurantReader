using RestaurantReader.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestaurantReader.Controllers
{
    public class RestaurantController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();
        
        // POST
        // api/Restaurant
        [HttpPost]
        public async Task<IHttpActionResult> CreateRestaurant([FromBody] Restaurant model)
        {
            if(model is null)
            {
                return BadRequest("Your request body cannot be empty.");
            }

            if (ModelState.IsValid)
            {
                // Store the model in database
                _context.Restaurants.Add(model);
                int changeCount = await _context.SaveChangesAsync();

                return Ok("Restaurant Created.");
            }

            // If invalid, reject it
            return BadRequest(ModelState);
        }

        // GET ALL
        // api/Restaurant
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Restaurant> restaurants = await _context.Restaurants.ToListAsync();
            return Ok(restaurants);
        }

        // GET BY ID
        // api/Restaurant/{id}
        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);
            
            if(restaurant != null)
            {
                return Ok(restaurant);
            }

            return NotFound();
        }

        // PUT Update
        // api/Restaurant/{id}
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRestaurant([FromUri] int id, [FromBody] Restaurant updatedRestaurant)
        {
            // Check the ids if they match
            if (id != updatedRestaurant?.Id)
                return BadRequest("Ids do not match.");

            // Check the ModelState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find restaurant in DB
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            // If restaurant DNE, do nothing
            if (restaurant is null)
                return NotFound();

            // Update
            restaurant.Name = updatedRestaurant.Name;
            restaurant.Address = updatedRestaurant.Address;

            // Save Changes
            await _context.SaveChangesAsync();

            return Ok("The restaurant was updated!");
        }

        // DELETE
        // api/Restaurant/{id}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRestaurant([FromUri] int id)
        {
            // Find restaurant in DB
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            // If restaurant DNE, do nothing
            if (restaurant is null)
                return NotFound();

            // Delete
            _context.Restaurants.Remove(restaurant);

            if(await _context.SaveChangesAsync() == 1)
                return Ok("The restaurant was deleted.");

            return InternalServerError();
        }
    }
}

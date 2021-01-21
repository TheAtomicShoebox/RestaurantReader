using RestaurantReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestaurantReader.Controllers
{
    public class RatingController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();

        // Create
        [HttpPost]
        public async Task<IHttpActionResult> CreateRating([FromBody] Rating model)
        {
            if (model is null)
                return BadRequest("Your request body cannot be empty.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Restaurant restaurantEntity = await _context.Restaurants.FindAsync(model.RestaurantId);

            if (restaurantEntity is null)
                return BadRequest($"The target restaurant with ID: {model.RestaurantId} does not exist.");

            // Add to ratings table, separate way to do this
            //_context.Ratings.Add(model);

            restaurantEntity.Ratings.Add(model);
            if (await _context.SaveChangesAsync() == 1)
                return Ok($"You rated restaurant {restaurantEntity.Name} successfully!");

            return InternalServerError();
        }

        // Get by Id

        // Get all

        // Get all by restaurant Id

        // Update

        // Delete
    }
}

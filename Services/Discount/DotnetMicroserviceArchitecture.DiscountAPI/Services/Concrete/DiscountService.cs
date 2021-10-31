using Dapper;
using DotnetMicroserviceArchitecture.Core.Dtos;
using DotnetMicroserviceArchitecture.DiscountAPI.Entities;
using DotnetMicroserviceArchitecture.DiscountAPI.Services.Abstract;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DotnetMicroserviceArchitecture.DiscountAPI.Services.Concrete
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;

            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> Add(Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("INSERT INTO discount (userid,rate,code,inserteddate) VALUES (@UserId,@Rate,@Code,@InsertedDate)", discount).ConfigureAwait(false);

            if (status <= 0) return Response<NoContent>.Fail("An error occurred while adding",HttpStatusCode.InternalServerError.GetHashCode());

            return Response<NoContent>.Success(HttpStatusCode.NoContent.GetHashCode());
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var status = await _dbConnection.ExecuteAsync("DELETE FROM discount WHERE id = @Id", new { Id = id }).ConfigureAwait(false);

            if (status <= 0) return Response<NoContent>.Fail("Discount not found", HttpStatusCode.NotFound.GetHashCode());

            return Response<NoContent>.Success(HttpStatusCode.NoContent.GetHashCode());
        }

        public async Task<Response<List<Discount>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Discount>("SELECT * FROM discount").ConfigureAwait(false);

            return Response<List<Discount>>.Success(discounts.ToList(), HttpStatusCode.OK.GetHashCode());
        }

        public async Task<Response<Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discount = (await _dbConnection.QueryAsync<Discount>("SELECT * FROM discount WHERE code=@Code AND userid=@UserId", new { Code = code, UserId = userId }).ConfigureAwait(false)).FirstOrDefault();

            if (discount is null) return Response<Discount>.Fail("Discount not found", HttpStatusCode.NotFound.GetHashCode());

            return Response<Discount>.Success(discount, HttpStatusCode.OK.GetHashCode());
        }

        public async Task<Response<Discount>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Discount>("SELECT * FROM discount WHERE id=@Id", new { id }).ConfigureAwait(false)).FirstOrDefault();

            if (discount is null) return Response<Discount>.Fail("Discount not found", HttpStatusCode.NotFound.GetHashCode());

            return Response<Discount>.Success(discount, HttpStatusCode.OK.GetHashCode());
        }

        public async Task<Response<NoContent>> Update(Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("UPDATE discount set userid=@UserId,rate=@Rate,code=@Code WHERE id = @id", new { Id = discount.Id, UserId = discount.UserId, Rate = discount.Rate, Code = discount.Code }).ConfigureAwait(false);

            if (status <= 0) return Response<NoContent>.Fail("Discount not found", HttpStatusCode.NotFound.GetHashCode());

            return Response<NoContent>.Success(HttpStatusCode.NoContent.GetHashCode());
        }
    }
}

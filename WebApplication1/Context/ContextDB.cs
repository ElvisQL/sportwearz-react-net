using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Context
{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options) : base(options)
        {

        }


    }
}

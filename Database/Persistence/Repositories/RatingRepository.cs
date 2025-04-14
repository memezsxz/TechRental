using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class RatingRepository : Repository<Rating>, IRatingRepository
    {
        public RatingRepository(RentalDBContext context) : base(context)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }

        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            query = query.Cast<Rating>().Select(er => new
            {
                Id = er.Id,
                Rate = er.Rate,
                Note = er.Note,
                Equipment = er.Equipment.Id + " - " + er.Equipment.Name,
                User = er.User.Id + " - " + er.User.FirstName + " " + er.User.LastName,
                IsHidden = er.IsHidden
            });

            return query.Cast<object>();
        }
    }
}

using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class RentalRequestRepository : Repository<RentalRequest>, IRentalRequestRepository
    {
        public RentalRequestRepository(RentalDBContext context) : base(context)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }

        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            query = query.Cast<RentalRequest>().Select(rr => new
            {
                Id = rr.Id,
                Equipment = rr.Equipment != null ? rr.Equipment.Id + " - " + rr.Equipment.Name : "",
                Customer = rr.Customer != null
                    ? rr.Customer.Id + " - " + rr.Customer.FirstName + " " + rr.Customer.LastName
                    : "",
                StartDate = rr.StartDate,
                EndDate = rr.ReturnDate,
                Status = rr.Status != null ? rr.Status.StatusName : ""
            });

            return query.Cast<object>();
        }
    }
}
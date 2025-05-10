using Database.Core.Domain;
using Database.Core.Repositories;
using Microsoft.EntityFrameworkCore;

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


        private IQueryable<RentalRequest> GetWithDetails()
        {
            return RentalDBContext.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Include(r => r.Status)
                .AsQueryable();
        }

        private IQueryable<RentalRequest> GetWithRecordDetails()
        {
            return GetWithDetails()
                .Include(r => r.RentalRecords)
                .AsQueryable();
        }

        public RentalRequest GetWithRecordDetails(int id)
        {
            return GetWithRecordDetails().Where(r => r.Id == id).FirstOrDefault();
        }

        #region Search

        public override Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var d = base.GetEntityColumnsWithTypes();
            if (d.ContainsKey("EquipmentId")) d["EquipmentId"] = "Int32";
            if (d.ContainsKey("CustomerId")) d["CustomerId"] = "Int32";
            return d;
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

        #endregion

    }
}
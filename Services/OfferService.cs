using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CartAppWS.DBFactory;
using CartAppWS.Models;
using CartAppWS.Utilities;

namespace CartAppWS.Services
{
    public class OfferService : IOffer
    {
        private Conexion conexion;

        public OfferService()
        {
            conexion = new Conexion();
        }

        public int Delete(Offer offer)
        {
            string sql = "DELETE FROM Offers WHERE IDOffer = " + offer.IDOffer;
            return conexion.Execute(sql);
        }

        public List<Offer> Get()
        {
            List<Offer> list = new List<Offer>();
            string sql = "SELECT IDOffer, Name, Image, StartDate, EndDate, StartTime, EndTime, Price, " +
                "Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday, Comment, CreatedUser, " +
                "CreatedDate, ModifiedDate, Status, ModifiedUser " +
                "FROM Offers " + 
                "WHERE Status <> " + (int) Constants.Status.ELIMINADO;
            DataTable dt = conexion.Query(sql);
            foreach(DataRow row in dt.Rows)
            {
                list.Add(GetOffer(row));
            }
            return list;
        }

        public List<Offer> GetAvailable()
        {
            List<Offer> list = new List<Offer>();
            string sql = "SELECT IDOffer, Name, Image, StartDate, EndDate, StartTime, EndTime, Price, " +
                "Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday, Comment, CreatedUser, " +
                "CreatedDate, ModifiedDate, Status, ModifiedUser " +
                "FROM Offers " +
                "WHERE Status = " + (int)Constants.Status.ACTIVO + " " +
                "AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE,StartDate) AND CONVERT(DATE, EndDate) " +
                "AND CONVERT(TIME, GETDATE()) BETWEEN CONVERT(TIME, StartTime) AND CONVERT(TIME, EndTime) " +
                $"AND {GetDay()} = 'True'";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(GetOffer(row));
            }
            return list;
        }

        public String GetDay()
        {
            String[] dias = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            return dias[(int)System.DateTime.Now.DayOfWeek];
        }
        private Offer GetOffer(DataRow row)
        {
            int id =  Convert.ToInt32(row["IDOffer"].ToString());
            string name = row["Name"].ToString();
            string image = row["Image"].ToString();
            DateTime startDate =  Fecha.toDateTimeUTC(row["StartDate"]);
            DateTime endDate = Fecha.toDateTimeUTC(row["EndDate"]);
            DateTime startTime = Fecha.toDateTimeUTC(row["StartTime"]);
            DateTime endTime = Fecha.toDateTimeUTC(row["EndTime"]);
            decimal price = Convert.ToDecimal(row["Price"]);
            bool monday = Convert.ToBoolean(row["Monday"]);
            bool tuesday = Convert.ToBoolean(row["Tuesday"]);
            bool wednesday = Convert.ToBoolean(row["Wednesday"]);
            bool thursday = Convert.ToBoolean(row["Thursday"]);
            bool friday = Convert.ToBoolean(row["Friday"]);
            bool saturday = Convert.ToBoolean(row["Saturday"]);
            bool sunday = Convert.ToBoolean(row["Sunday"]);
            string comment = row["Comment"].ToString();
            int createdUser = Convert.ToInt32(row["CreatedUser"]);
            DateTime createdDate = Fecha.toDateTimeUTC(row["CreatedDate"]);
            DateTime modifiedDate = Fecha.toDateTimeUTC(row["CreatedDate"]);
            int modifiedUser = Convert.ToInt32(row["ModifiedUser"]); ;
            int status = Convert.ToInt32(row["Status"]); 
            return new Offer(id, name, image, startDate, endDate, startTime, endTime, price, monday, tuesday, wednesday, thursday, friday, saturday, sunday, comment, createdUser, createdDate, modifiedUser, modifiedDate, status, GetProducts(id));
        }

        private List<OffersProducts> GetProducts(int IDOffer)
        {
            List<OffersProducts> list = new List<OffersProducts>();
            String sql = "SELECT op.IDOfferProduct, op.IDOffer, op.IDProduct, op.Quantity, p.Name, p.Image " +
                "FROM offersproducts op JOIN products p ON op.IDProduct = p.IDProduct AND op.IDOffer = " + IDOffer;
            DataTable dt = conexion.Query(sql);
            foreach(DataRow row in dt.Rows)
            {
                int idOfferProduct = Convert.ToInt32(row["IDOfferProduct"]) ;
                int idOffer = Convert.ToInt32(row["IDOffer"]);
                int idProduct = Convert.ToInt32(row["IDProduct"]) ;
                decimal quantity = Convert.ToDecimal(row["Quantity"]);
                string name = row["Name"].ToString();
                string image = row["Image"].ToString();
                list.Add(new OffersProducts(idOfferProduct, idOffer, idProduct, quantity, name, image));
            }
            return list;
        }

        public Offer GetById(int id)
        {
            string sql = "SELECT IDOffer, Name, Image, StartDate, EndDate, StartTime, EndTime, Price, Monday, Tuesday, " +
                "Wednesday, Thursday, Friday, Saturday, Sunday, Comment, CreatedUser, CreatedDate, " +
                "ModifiedDate, Status, ModifiedUser   FROM Offers " +
                "WHERE Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "AND IDOffer = " + id;
   
            DataTable dt = conexion.Query(sql);
            if (dt.Rows.Count > 0)
            {
                return (GetOffer(dt.Rows[0]));
            }
            return null;
        }

        public int Save(Offer offer)
        {
            if (offer.IDOffer == 0)
                return Insert(offer);
            else
                return Update(offer);
        }

        private int Insert(Offer offer)
        {
            string sql = "INSERT INTO Offers (Name, Image, StartDate, EndDate, StartTime, EndTime, Price, Monday, " +
                "Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday, Comment, CreatedUser, CreatedDate, " +
                "ModifiedDate, Status, ModifiedUser) " +
                "VALUES (@Name, @Image, @StartDate, @EndDate, @StartTime, @EndTime, @Price, @Monday, " +
                "@Tuesday, @Wednesday, @Thursday, @Friday, @Saturday, @Sunday, @Comment, @CreatedUser, @CreatedDate, " +
                "@ModifiedDate, @Status, @ModifiedUser)";

            string[] parametros = {"@Name", "@Image", "@StartDate", "@EndDate", "@StartTime", "@EndTime", "@Price", "@Monday",
                "@Tuesday", "@Wednesday", "@Thursday", "@Friday", "@Saturday", "@Sunday", "@Comment", "@CreatedUser", "@CreatedDate",
                "@ModifiedDate", "@Status", "@ModifiedUser"};

            object[] valores = {offer.Name, offer.Image, offer.StartDate, offer.EndDate, offer.StartTime, offer.EndTime, offer.Price, offer.Monday,
                offer.Tuesday, offer.Wednesday, offer.Thursday, offer.Friday, offer.Saturday, offer.Sunday, offer.Comment, offer.CreatedUser, offer.CreatedDate,
                offer.ModifiedDate, offer.Status, offer.ModifiedUser};

            int i;
            try
            {
                conexion.BeginTrassancion();
                i = conexion.Execute(sql, parametros, valores);
                int id = conexion.LastInsertID();
                foreach(OffersProducts op in offer.Products)
                {
                    op.IDOffer = id;
                    i += InsertOffersProducts(op);
                }
                conexion.Commit();
            }catch (Exception)
            {
                conexion.Rollback();
                throw;
            }
            return i;

        }

        private int Update(Offer offer)
        {
            string sql = "UPDATE Offers SET Name = @Name, Image = @Image, StartDate = @StartDate, EndDate = @EndDate, StartTime = @StartTime, " +
                "EndTime = @EndTime, Price = @Price, Monday = @Monday, Tuesday = @Tuesday, Wednesday = @Wednesday, Thursday = @Thursday, " +
                "Friday = @Friday, Saturday = @Saturday, Sunday = @Sunday, Comment = @Comment, CreatedUser = @CreatedUser, CreatedDate = @CreatedDate, " +
                "ModifiedDate = @ModifiedDate, Status = @Status, ModifiedUser = @ModifiedUser WHERE IDOffer = @IDOffer";
            
            string[] parametros = {"@Name", "@Image", "@StartDate", "@EndDate", "@StartTime", "@EndTime", "@Price", "@Monday",
                "@Tuesday", "@Wednesday", "@Thursday", "@Friday", "@Saturday", "@Sunday", "@Comment", "@CreatedUser", "@CreatedDate",
                "@ModifiedDate", "@Status", "@ModifiedUser", "@IDOffer"};

            object[] valores = {offer.Name, offer.Image, offer.StartDate, offer.EndDate, offer.StartTime, offer.EndTime, offer.Price, offer.Monday,
                offer.Tuesday, offer.Wednesday, offer.Thursday, offer.Friday, offer.Saturday, offer.Sunday, offer.Comment, offer.CreatedUser, offer.CreatedDate,
                offer.ModifiedDate, offer.Status, offer.ModifiedUser, offer.IDOffer};
            
            
            
            int i;
            try
            {
                conexion.BeginTrassancion();
                i = conexion.Execute(sql, parametros, valores);
                i += DeleteProducts(offer);
                foreach (OffersProducts offersProducts in offer.Products)
                {
                    offersProducts.IDOffer = offer.IDOffer;
                    if (offersProducts.IDOfferProduct == 0)
                        i += InsertOffersProducts(offersProducts);
                    else
                        i += UpdateOffersProducts(offersProducts);
                }
                conexion.Commit();
            }catch(Exception)
            {
                conexion.Rollback();
                throw;
            }
            return i;
        }

        private int DeleteProducts(Offer offer)
        {

            if (offer.Products.Count > 0)
            {
                String sql = "DELETE FROM OffersProducts WHERE IDOffer = " + offer.IDOffer + " AND Not IDProduct in (";
                foreach (OffersProducts offersProducts in offer.Products)
                    sql += offersProducts.IDProduct + ",";
                sql = sql.Substring(0, sql.Length - 1) + ")";
                return conexion.Execute(sql);
            }
            return 0;
        }

                private int InsertOffersProducts(OffersProducts offersProducts)
        {
            string sql2 = "INSERT INTO OffersProducts (IDOffer, IDProduct, Quantity) VALUES (@IDOffer, @IDProduct, @Quantity)";

            string[] parametros = {"@IDOffer", "@IDProduct", "@Quantity"};

            object[] valores = {offersProducts.IDOffer, offersProducts.IDProduct, offersProducts.Quantity};

            return conexion.Execute(sql2, parametros, valores);
	    }

        private int UpdateOffersProducts(OffersProducts offersProducts)
        {
            string sql2 = "UPDATE OffersProducts SET IDOffer = @IDOffer, IDProduct = @IDProduct, Quantity = @Quantity WHERE IDOfferProduct = @IDOfferProduct";
            
            string[] parametros = {"@IDOffer", "@IDProduct", "@Quantity", "@IDOfferProduct"};

            object[] valores = {offersProducts.IDOffer, offersProducts.IDProduct, offersProducts.Quantity, offersProducts.IDOfferProduct};

            return conexion.Execute(sql2, parametros, valores);
        }
    }
}

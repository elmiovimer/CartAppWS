using CartAppWS.DBFactory;
using CartAppWS.Models;
using CartAppWS.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Services
{
    public class EventService : IEvent
    {
        private readonly Conexion conexion;

        public EventService()
        {
            conexion = new Conexion();
        }

        public int Delete(Event Event)
        {
            string sql1 = $"DELETE FROM EventsOffers WHERE IDEvent = {Event.IDEvent}";
            string sql2 = $"DELETE FROM EventsImages WHERE IDEvent = {Event.IDEvent}";
            string sql3 = $"DELETE FROM Events WHERE IDEvent = {Event.IDEvent}";
            try
            {
                conexion.BeginTrassancion();
                int i = conexion.Execute(sql1);
                i += conexion.Execute(sql2);
                i += conexion.Execute(sql3);
                conexion.Commit();
                return i;
            }
            catch (Exception)
            {
                conexion.Rollback();
                throw;
            }
        }

        public List<Event> Get()
        {
            List<Event> list = new List<Event>();
            string sql = "SELECT IDEvent, Name, StartDate, EndDate, StartTime, " +
                "EndTime, Image, Description, CreateUser, CreatedDate, ModifiedUser, " +
                "ModifiedDate, Status " +
                "FROM Events " +
                "WHERE Status <> " + (int)Constants.Status.ELIMINADO;
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetEvent(row));
            return list;
        }

        public List<Event> GetAvailable()
        {
            List<Event> list = new List<Event>();
            string sql = "SELECT IDEvent, Name, StartDate, EndDate, StartTime, " +
                "EndTime, Image, Description, CreateUser, CreatedDate, ModifiedUser, " +
                "ModifiedDate, Status " +
                "FROM Events " +
                "WHERE Status = " + (int)Constants.Status.ACTIVO + " " +
                "AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(DATE,StartDate) AND CONVERT(DATE, EndDate) " +
                "AND CONVERT(TIME, GETDATE()) BETWEEN CONVERT(TIME, StartTime) AND CONVERT(TIME, EndTime)";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetEvent(row));
            return list;
        }

        private Event GetEvent(DataRow row)
        {
            int IDEvent = Convert.ToInt32(row["IDEvent"]);
            return new Event()
            {
                IDEvent = Convert.ToInt32(row["IDEvent"]),
                Name = row["Name"].ToString(),
                StartDate = Fecha.toDateTimeUTC(row["StartDate"]),
                EndDate = Fecha.toDateTimeUTC(row["EndDate"]),
                StartTime = Fecha.toDateTimeUTC(row["StartTime"]),
                EndTime = Fecha.toDateTimeUTC(row["EndTime"]),
                Image = row["Image"].ToString(),
                Description = row["Description"].ToString(),
                CreateUser = Convert.ToInt32(row["CreateUser"]),
                CreatedDate = Fecha.toDateTimeUTC(row["CreatedDate"]),
                ModifiedUser = Convert.ToInt32(row["ModifiedUser"]),
                ModifiedDate = Fecha.toDateTimeUTC(row["ModifiedDate"]),
                Status = Convert.ToInt32(row["Status"]),
                Images = GetImages(IDEvent),
                Offers = GetOffers(IDEvent),
            };
        }

        private List<EventOffer> GetOffers(int iDEvent)
        {
            List<EventOffer> list = new List<EventOffer>();
            string sql = "SELECT a.IDEventOffer, a.IDEvent, a.IDOffer, b.Name " +
                "FROM EventsOffers a " +
                "JOIN Offers b ON(a.IDOffer = b.IDOffer) " +
                $"WHERE a.IDEvent = {iDEvent}";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(new EventOffer()
                {
                    IDEventOffer = Convert.ToInt32(row["IDEventOffer"]),
                    IDEvent = Convert.ToInt32(row["IDEvent"]),
                    IDOffer = Convert.ToInt32(row["IDOffer"]),
                    OfferName = row["OfferName"].ToString(),
                });
            return list;

        }

        private List<EventImage> GetImages(int iDEvent)
        {
            List<EventImage> list = new List<EventImage>();
            string sql = $"SELECT IDImage, IDEvent, Image FROM EventsImages WHERE IDEvent = {iDEvent}";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(new EventImage()
                {
                    IDImage = Convert.ToInt32(row["IDImage"]),
                    IDEvent = Convert.ToInt32(row["IDEvent"]),
                    Image = row["Image"].ToString(),
                });
            return list;
        }

        public Event GetById(int id)
        {
            string sql = "SELECT IDEvent, Name, StartDate, EndDate, StartTime, " +
                "EndTime, Image, Description, CreateUser, CreatedDate, ModifiedUser, " +
                "ModifiedDate, Status " +
                "FROM Events " +
                "WHERE Status <> " + (int)Constants.Status.ELIMINADO + " " +
                $"AND IDEvent = {id}";
            DataTable dt = conexion.Query(sql);
            if (dt.Rows.Count > 0)
                return GetEvent(dt.Rows[0]);
            return null;
        }

        public int Save(Event Event)
        {
            if (Event.IDEvent == 0)
                return Insert(Event);
            else
                return Update(Event);
        }

        private int Insert(Event e)
        {
            string sql = "INSERT INTO Events (Name, StartDate, EndDate, StartTime, EndTime, Image, Description, CreateUser, CreatedDate, ModifiedUser, ModifiedDate, Status) " +
                "VALUES (@Name, @StartDate, @EndDate, @StartTime, @EndTime, @Image, @Description, @CreateUser, @CreatedDate, @ModifiedUser, @ModifiedDate, @Status)";
            string[] parametros = { "@Name", "@StartDate", "@EndDate", "@StartTime", "@EndTime", "@Image", "@Description", "@CreateUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status" };
            object[] valores = { e.Name, e.StartDate, e.EndDate, e.StartTime, e.EndTime, e.Image, e.Description, e.CreateUser, e.CreatedDate, e.ModifiedUser, e.ModifiedDate, e.Status };
            try
            {
                conexion.BeginTrassancion();
                int i = conexion.Execute(sql, parametros, valores);
                int id = conexion.LastInsertID();
                foreach(EventOffer offer in e.Offers)
                {
                    offer.IDEvent = id;
                    i += InsertOffer(offer);
                }
                foreach (EventImage img in e.Images)
                {
                    img.IDEvent = id;
                    i += InsertImage(img);
                }
                conexion.Commit();
                return i;
            }
            catch (Exception)
            {
                conexion.Rollback();
                throw;
            }

        }

        private int InsertOffer(EventOffer offer)
        {
            string sql = "INSERT INTO EventsOffers(IDEvent, IDOffer) " +
                "VALUES(@IDEvent, @IDOffer)";
            string[] parametros = { "@IDEvent", "@IDOffer" };
            object[] valores = { offer.IDEvent, offer.IDOffer };
            return conexion.Execute(sql, parametros, valores);
        }

        private int InsertImage(EventImage img)
        {
            string sql = "INSERT INTO EventsImages(IDEvent, Image) " +
                "VALUES(@IDEvent, @Image)";
            string[] parametros = { "@IDEvent", "@Image" };
            object[] valores = { img.IDEvent, img.Image };
            return conexion.Execute(sql, parametros, valores);
        }

        private int Update(Event e)
        {
            string sql = "UPDATE Events SET Name = @Name, StartDate = @StartDate, EndDate = @EndDate, StartTime = @StartTime, EndTime = @EndTime, Image = @Image, Description = @Description, CreateUser = @CreateUser, CreatedDate = @CreatedDate, ModifiedUser = @ModifiedUser, ModifiedDate = @ModifiedDate, Status = @Status  " +
                "WHERE IDEvent = @IDEvent";
            string[] parametros = { "@Name", "@StartDate", "@EndDate", "@StartTime", "@EndTime", "@Image", "@Description", "@CreateUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status", "@IDEvent" };
            object[] valores = { e.Name, e.StartDate, e.EndDate, e.StartTime, e.EndTime, e.Image, e.Description, e.CreateUser, e.CreatedDate, e.ModifiedUser, e.ModifiedDate, e.Status, e.IDEvent };
            try
            {
                conexion.BeginTrassancion();
                DeleteImages(e);
                DeleteOffers(e);
                int i = conexion.Execute(sql, parametros, valores);
                foreach (EventOffer offer in e.Offers)
                {
                    offer.IDEvent = e.IDEvent;
                    if (offer.IDEventOffer == 0)
                        i += InsertOffer(offer);
                    else
                        i += UpdateOffer(offer);
                }
                foreach (EventImage img in e.Images)
                {
                    img.IDEvent = e.IDEvent;
                    if (img.IDImage == 0)
                        i += InsertImage(img);
                    else
                        i += UpdateImage(img);
                }
                conexion.Commit();
                return i;
            }
            catch (Exception)
            {
                conexion.Rollback();
                throw;
            }
        }

        private int UpdateOffer(EventOffer offer)
        {
            string sql = "UPDATE EventsOffers SET IDEvent = @IDEvent, IDOffer = @IDOffer " +
                "WHERE IDEventOffer = @IDEventOffer";
            string[] parametros = { "@IDEvent", "@IDOffer", "@IDEventOffer" };
            object[] valores = { offer.IDEvent, offer.IDOffer, offer.IDEventOffer };
            return conexion.Execute(sql, parametros, valores);
        }

        private int UpdateImage(EventImage img)
        {
            string sql = "UPDATE EventsImages SET IDEvent = @IDEvent, Image = @Image " +
                "WHERE IDImage = @IDImage";
            string[] parametros = { "@IDEvent", "@Image", "@IDImage" };
            object[] valores = { img.IDEvent, img.Image, img.IDImage };
            return conexion.Execute(sql, parametros, valores);
        }

        private int DeleteOffers(Event e)
        {
            if (e.Offers.Count > 0)
            {
                String sql = "DELETE FROM EventsOffers WHERE IDEvent = " + e.IDEvent + " AND Not IDEventOffer in (";
                foreach (EventOffer offer in e.Offers)
                    sql += offer.IDEventOffer + ",";

                sql = sql.Substring(0, sql.Length - 1) + ")";
                return conexion.Execute(sql);
            }
            return 0;
        }

        private int DeleteImages(Event e)
        {
            if (e.Images.Count > 0)
            {
                String sql = "DELETE FROM EventsImages WHERE IDEvent = " + e.IDEvent + " AND Not IDImage in (";
                foreach (EventImage img in e.Images)
                    sql += img.IDImage + ",";

                sql = sql.Substring(0, sql.Length - 1) + ")";
                return conexion.Execute(sql);
            }
            return 0;
        }
    }
}

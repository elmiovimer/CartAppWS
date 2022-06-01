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
    public class OrderService : IOrder
    {
        private readonly Conexion conexion;
        private readonly IAddress _address;

        //c.FirstName +' ' + c.LastName as ClientName,

        private readonly string SQLIncial = " SELECT o.IDOrder, o.IDClient, c.FirstName +' ' + c.LastName as ClientName, c.Phone, o.IDAddress, o.Date, o.Total, o.Taxes, " +
            "o.IDPaymentMethod, o.IDOrderType, o.Status, o.TookUserOrder, o.MacAddress, o.Tracking " +
            "FROM Orders o " +
            "join clients c on o.IDClient = c.IDClient ";

        private readonly string SQLTop5Incial = " SELECT TOP 5 o.IDOrder, o.IDClient, c.FirstName +' ' + c.LastName as ClientName, c.Phone, o.IDAddress, o.Date, o.Total, o.Taxes, " +
            "o.IDPaymentMethod, o.IDOrderType, o.Status, o.TookUserOrder, o.MacAddress, o.Tracking " +
            "FROM Orders o " +
            "join clients c on o.IDClient = c.IDClient ";

        public OrderService()
        {
            conexion = new Conexion();
            _address = new AddressService();
        }

       
        public int Delete(Order order)
        {
            string sql1 = $"DELETE FROM OrdersItemTopping WHERE WHERE IDOrderItem IN (SELECT IDOrderItem FROM OrdersItems WHERE IDOrder = {order.IDOrder}) ";
            string sql2 = $"DELETE FROM FROM OrdersItems WHERE IDOrder = {order.IDOrder}";
            string sql3 = $"DELETE FROM FROM OrdersOffers WHERE IDOrder = {order.IDOrder}";
            string sql4 = $"DELETE FROM FROM Orders WHERE IDOrder = FROM  {order.IDOrder}";
            try
            {
                conexion.BeginTrassancion();
                int i = conexion.Execute(sql1);
                i += conexion.Execute(sql2);
                i += conexion.Execute(sql3);
                i += conexion.Execute(sql4);
                conexion.Commit();
                return i;
            }
            catch (Exception)
            {
                conexion.Rollback();
                throw;
            }
        }

        public List<Order> Get()
        {
            List<Order> list = new List<Order>();
            string sql = SQLIncial + 
                "WHERE o.Status <> " + (int) Constants.Status.ELIMINADO;
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetOrder(row));
            return list;
        }

        public dynamic GetAvailableOrders(int id)
        {
            string sql = "SELECT ISNULL(Count(IDOrder), 0) as Quantity, " +
                "ISNULL(Max(IDOrder),0) as LastID " +
                "FROM orders " +
                $"WHERE Status = {(int)Constants.Status.ACTIVO} " +
                $"AND Tracking = {(int) Constants.Tracking.PENDIENTE}" +
                "AND IDOrder > @ID ";
            string[] parametros = { "@ID" };
            Object[] valores = { id };
            DataTable dt = conexion.Query(sql, parametros, valores);
            if (dt.Rows.Count > 0)
                return new 
                {
                    Availables = Convert.ToInt32(dt.Rows[0]["Quantity"]),
                    Last = Convert.ToInt32(dt.Rows[0]["LastID"])
                };
            return 0;
        }

        public List<dynamic> TrackOrders(int[] ids)
        {
            List<dynamic> list = new();
            string sql = "SELECT IDOrder, Tracking " +
                "FROM Orders " +
                "WHERE Status = 1 " +
                "AND IDOrder in (";

            for (int i = 0; i < ids.Length - 1; i++)
                sql += $"{ids[i]}, ";
            sql += $"{ids[ids.Length - 1]})";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(new
                {
                    IDOrder = Convert.ToInt32(row["IDOrder"]),
                    Tracking = Convert.ToInt32(row["Tracking"])
                });
            return list;

        }

        private Order GetOrder(DataRow row)
        {
            int IDOrder = Convert.ToInt32(row["IDOrder"]);
            int IDAddress = Convert.ToInt32(row["IDAddress"]);
            Address address = _address.GetById(IDAddress);
            return new Order()
            {
                IDOrder = Convert.ToInt32(row["IDOrder"]),
                IDClient = Convert.ToInt32(row["IDClient"]),
                ClientName = Convert.ToString(row["ClientName"]),
                Phone = Convert.ToString(row["Phone"]),
                IDAddress = Convert.ToInt32(row["IDAddress"]),
                Address = address,
                Date = Fecha.toDateTimeUTC(row["Date"]),
                Total = Convert.ToDecimal(row["Total"]),
                Taxes = Convert.ToDecimal(row["Taxes"]),
                IDPaymentMethod = Convert.ToInt32(row["IDPaymentMethod"]),
                IDOrderType = Convert.ToInt32(row["IDOrderType"]),
                Status = Convert.ToInt32(row["Status"]),
                TookUserOrder = Convert.ToInt32(row["TookUserOrder"]),
                MacAddress = row["MacAddress"].ToString(),
                Tracking = Convert.ToInt32( row["Tracking"]),
                Items = GetItems(IDOrder),
                Offers = GetOffers(IDOrder),
            };
        }

        
        private List<OrderItem> GetItems(int iDOrder)
        {
            List<OrderItem> list = new List<OrderItem>();
            string sql = "SELECT o.IDOrderItem, o.IDOrder, o.IDProduct, p.Name as ProductName, o.Quantity, " +
                "o.Price, o.Taxes, o.Comment " +
                "FROM OrdersItems o " +
                "JOIN Products p ON(o.IDProduct = p.IDProduct) " +
                $"WHERE IDOrder = {iDOrder}";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetItem(row));
            return list;
        }

        public List<Order> TrackOrder(int tracking)
        {
            List<Order> list = new();
            string sql = SQLIncial +
                 "WHERE o.Status <> " + (int)Constants.Status.ELIMINADO + " " +
                $"AND isnull(Tracking, 0) = @Tracking";
            string[] parametro = { "@Tracking" };
            object[] valores = { tracking };
            DataTable dt = conexion.Query(sql, parametro, valores);
            foreach (DataRow row in dt.Rows)
                list.Add(GetOrder(row));
            return list;
        }

        private OrderItem GetItem(DataRow row)
        {
            int IDOrderItem = Convert.ToInt32(row["IDOrderItem"]);
            return new OrderItem()
            {
                IDOrderItem = Convert.ToInt32(row["IDOrderItem"]),
                IDOrder = Convert.ToInt32(row["IDOrder"]),
                IDProduct = Convert.ToInt32(row["IDProduct"]),
                ProductName = row["ProductName"].ToString(),
                Quantity = Convert.ToDecimal(row["Quantity"]),
                Price = Convert.ToDecimal(row["Price"]),
                Taxes = Convert.ToDecimal(row["Taxes"]),
                Comment = row["Comment"].ToString(),
                toppings = GetItemToppings(IDOrderItem),
            };
        }

        private List<OrderItemTopping> GetItemToppings(int iDOrderItem)
        {
            List<OrderItemTopping> list = new List<OrderItemTopping>();
            string sql = "SELECT i.IDOrdersItemTopping, i.IDOrderItem, i.IDTopping, t.Name as ToppingName, " +
                "tt.IDToppingType, tt.Name as ToppingTypeName, i.Selected " +
                "FROM OrdersItemTopping i " +
                "JOIN Toppings t ON i.IDTopping = t.IDTopping " +
                "JOIN ToppingTypes tt ON t.IDToppingType = tt.IDToppingType " +
                $"WHERE IDOrderItem = {iDOrderItem} " +
                $"ORDER BY tt.Name, t.Name";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(new OrderItemTopping()
                {
                    IDOrdersItemTopping = Convert.ToInt32(row["IDOrdersItemTopping"]),
                    IDOrderItem = Convert.ToInt32(row["IDOrderItem"]),
                    IDTopping = Convert.ToInt32(row["IDTopping"]),
                    ToppingName = row["ToppingName"].ToString(),
                    IDToppingType = Convert.ToInt32(row["IDToppingType"]),
                    ToppingTypeName = row["ToppingTypeName"].ToString(),
                    Selected = Convert.ToBoolean(row["Selected"]),
                });
            return list;
        }

        public Order GetById(int id)
        {
            string sql =SQLIncial +
                "WHERE o.Status <> " + (int)Constants.Status.ELIMINADO + " " +
                "AND IDOrder = " + id;
            DataTable dt = conexion.Query(sql);
            if (dt.Rows.Count > 0)
                return GetOrder(dt.Rows[0]);
            return null;
        }

        public List<Order> GetByClient(int id)
        {
            List<Order> list = new();
            string sql = SQLTop5Incial +
                "WHERE o.Status <> " + (int)Constants.Status.ELIMINADO + " " +
                "AND o.IDClient = " + id + " " +
                "ORDER BY o.Date desc";
            DataTable dt = conexion.Query(sql);
            foreach(DataRow row in dt.Rows)
                list.Add(GetOrder(row));
            return list ;
        }

        private List<OrderOffers> GetOffers(int iDOrder)
        {
            List<OrderOffers> list = new List<OrderOffers>();
            string sql = "SELECT a.IDOrderOffer, a.IDOrder, a.IDOffer,  a.Quantity " +
                "FROM OrdersOffers a " +
                "JOIN Offers b ON(a.IDOffer = b.IDOffer) " +
                $"WHERE a.IDOrder = {iDOrder}";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(new OrderOffers()
                {
                    IDOrderOffer = Convert.ToInt32(row["IDOrderOffer"]),
                    IDOrder = Convert.ToInt32(row["IDOrder"]),
                    IDOffer = Convert.ToInt32(row["IDOffer"]),
                    //OfferName = row["OfferName"].ToString(),
                    Quantity = Convert.ToDecimal(row["Quantity"]),
                });
            return list;
        }

        public int Save(Order order)
        {
            if (order.IDOrder == 0)
                return Insert(order);
            else
                return Update(order);
        }

        private int Insert(Order order)
        {
            string sql = "INSERT INTO Orders (IDClient, IDAddress, Date, Total, Taxes, IDPaymentMethod, IDOrderType, Status, TookUserOrder, MacAddress, Tracking) " +
                "VALUES(@IDClient, @IDAddress, @Date, @Total, @Taxes, @IDPaymentMethod, @IDOrderType, @Status, @TookUserOrder, @MacAddress, @Tracking)";
            string[] parametros = { "@IDClient", "@IDAddress", "@Date", "@Total", "@Taxes", "@IDPaymentMethod", "@IDOrderType", "@Status", "@TookUserOrder", "@MacAddress", "@Tracking" };
            object[] valores = { order.IDClient, order.IDAddress, order.Date, order.Total, order.Taxes, order.IDPaymentMethod, order.IDOrderType, order.Status, order.TookUserOrder, order.MacAddress, order.Tracking };
            try
            {
                conexion.BeginTrassancion();
                int i = conexion.Execute(sql, parametros, valores);
                int id = conexion.LastInsertID();
                foreach(OrderItem item in order.Items)
                {
                    item.IDOrder = id;
                    i += InsertItem(item);
                }
                foreach(OrderOffers offer in order.Offers)
                {
                    offer.IDOrder = id;
                    i += InsertOffer(offer);
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

        private int InsertItem(OrderItem item)
        {
            string sql = "INSERT INTO OrdersItems (IDOrder, IDProduct, Quantity, Price, Taxes, Comment) " +
                "VALUES(@IDOrder, @IDProduct, @Quantity, @Price, @Taxes, @Comment)";
            string[] parametros = { "@IDOrder", "@IDProduct", "@Quantity", "@Price", "@Taxes", "@Comment" };
            object[] valores = { item.IDOrder, item.IDProduct, item.Quantity, item.Price, item.Taxes, item.Comment };
            int i = conexion.Execute(sql, parametros, valores);
            int id = conexion.LastInsertID();
            foreach(OrderItemTopping topping in item.toppings)
            {
                topping.IDOrderItem = id;
                i += InsertItemTopping(topping);
            }
            return i;
        }

        private int InsertItemTopping(OrderItemTopping topping)
        {
            string sql = "INSERT INTO OrdersItemTopping (IDOrderItem, IDTopping, Selected)" +
                "VALUES(@IDOrderItem, @IDTopping, @Selected)";
            string[] parametros = { "@IDOrderItem", "@IDTopping", "@Selected" };
            object[] valores = { topping.IDOrderItem, topping.IDTopping, topping.Selected };
            return conexion.Execute(sql, parametros, valores);
        }

        private int InsertOffer(OrderOffers offer)
        {
            string sql = "INSERT INTO OrdersOffers(IDOrder, IDOffer, Quantity) " +
                "VALUES(@IDOrder, @IDOffer, @Quantity)";
            string[] parametros = { "@IDOrder", "@IDOffer", "@Quantity" };
            object[] valores = { offer.IDOrder, offer.IDOffer, offer.Quantity };
            return conexion.Execute(sql, parametros, valores);
        }

        private int Update(Order order)
        {
            string sql = "UPDATE Orders SET IDClient = @IDClient, IDAddress = @IDAddress, Date = @Date, Total = @Total, Taxes = @Taxes, " +
                "IDPaymentMethod = @IDPaymentMethod, IDOrderType = @IDOrderType, Status = @Status, TookUserOrder = @TookUserOrder, " +
                "MacAddress = @MacAddress, Tracking = @Tracking " +
                "WHERE IDOrder = @IDOrder";
            string[] parametros = { "@IDClient", "@IDAddress", "@Date", "@Total", "@Taxes", "@IDPaymentMethod", "@IDOrderType", "@Status", "@TookUserOrder", "@MacAddress", "@Tracking", "@IDOrder" };
            object[] valores = { order.IDClient, order.IDAddress, order.Date, order.Total, order.Taxes, order.IDPaymentMethod, order.IDOrderType, order.Status, order.TookUserOrder, order.MacAddress, order.Tracking, order.IDOrder };
            try
            {

                conexion.BeginTrassancion();
                DeleteItems(order);
                DeleteOffers(order);
                int i = conexion.Execute(sql, parametros, valores);
                foreach (OrderItem item in order.Items)
                {
                    item.IDOrder = order.IDOrder;
                    if (item.IDOrderItem == 0)
                        i += InsertItem(item);
                    else
                        i += UpdateItem(item);
                }
                foreach (OrderOffers offer in order.Offers)
                {
                    offer.IDOrder = order.IDOrder;
                    if (offer.IDOrderOffer == 0)
                        i += InsertOffer(offer);
                    else
                        i += UpdateOffer(offer);
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

        private int UpdateItem(OrderItem item)
        {
            string sql = "UPDATE OrdersItems SET IDOrder = @IDOrder, IDProduct = @IDProduct, Quantity = @Quantity, Price = @Price, Taxes = @Taxes, Comment = @Comment  " +
                "WHERE IDOrderItem = @IDOrderItem";
            string[] parametros = { "@IDOrder", "@IDProduct", "@Quantity", "@Price", "@Taxes", "@Comment", "@IDOrderItem" };
            object[] valores = { item.IDOrder, item.IDProduct, item.Quantity, item.Price, item.Taxes, item.Comment, item.IDOrderItem };
            int i = conexion.Execute(sql, parametros, valores);
            foreach (OrderItemTopping topping in item.toppings)
            {
                topping.IDOrderItem = item.IDOrderItem;
                if (topping.IDOrdersItemTopping == 0)
                    i += InsertItemTopping(topping);
                else
                    i += UpdateItemTopping(topping);
            }
            return i;
        }

        private int UpdateItemTopping(OrderItemTopping topping)
        {
            string sql = "UPDATE OrdersItemTopping SET IDOrderItem = @IDOrderItem, IDTopping = @IDTopping, Selected = @Selected " +
                "WHERE IDOrdersItemTopping = @IDOrdersItemTopping";
            string[] parametros = { "@IDOrderItem", "@IDTopping", "@Selected", "@IDOrdersItemTopping" };
            object[] valores = { topping.IDOrderItem, topping.IDTopping, topping.Selected, topping.IDOrdersItemTopping };
            return conexion.Execute(sql, parametros, valores);
        }

        private int UpdateOffer(OrderOffers offer)
        {
            string sql = "UDPDATE OrdersOffers SET IDOrder = @IDOrder, IDOffer = @IDOffer, Quantity = @Quantity " +
                "WHERE IDOrderOffer = @IDOrderOffer";
            string[] parametros = { "@IDOrder", "@IDOffer", "@Quantity", "@IDOrderOffer" };
            object[] valores = { offer.IDOrder, offer.IDOffer, offer.Quantity, offer.IDOrderOffer };
            return conexion.Execute(sql, parametros, valores);
        }

        private int DeleteItems(Order order)
        {
            if (order.Items.Count > 0)
            {
                String sql = "SELECT IDOrderItem FROM OrdersItems WHERE IDOrder = " + order.IDOrder + " AND Not IDOrderItem in (";
                foreach (OrderItem item in order.Items)
                    sql += item.IDOrderItem + ",";

                sql = sql.Substring(0, sql.Length - 1) + ")";
                DataTable dt = conexion.Query(sql);
                int i = 0;
                foreach(DataRow row in dt.Rows)
                {
                    i += conexion.Execute("DELETE FROM OrdersItemTopping WHERE IDOrderItem = " + Convert.ToInt32(row["IDOrderItem"]));
                    i += conexion.Execute("DELETE FROM OrdersItems WHERE IDOrderItem = " + Convert.ToInt32(row["IDOrderItem"]));
                }
                return i;
            }
            return 0;
        }

        private int DeleteOffers(Order order)
        {
            if (order.Offers.Count > 0)
            {
                String sql = "DELETE FROM OrdersOffers WHERE IDOrder = " + order.IDOrder + " AND Not IDOrderOffer in (";
                foreach (OrderOffers offer in order.Offers)
                    sql +=  offer.IDOrderOffer + ",";

                sql = sql.Substring(0, sql.Length - 1) + ")";
                return conexion.Execute(sql);
            }
            return 0;
        }
    }
}

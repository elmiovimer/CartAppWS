using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CartAppWS.DBFactory;
using CartAppWS.Models;
using CartAppWS.Utilities;

namespace CartAppWS.Services
{
    public class StateService : IState
    {
        private Conexion conexion;

        public StateService()
        {
            conexion = new Conexion();
        }
        public int Delete(State state)
        {
            string sql = "DELETE FROM States WHERE IDState = " + state.IDState;
            return conexion.Execute(sql);
        }

        public List<State> Get()
        {
            List<State> list = new List<State>();
            string sql = "SELECT IDState, Name, CreatedUser, " +
                "CreatedDate, ModifiedUser, ModifiedDate, Status " +
                "FROM States " +
                "WHERE Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "ORDER BY Name";
            DataTable dt = conexion.Query(sql);
            foreach (DataRow row in dt.Rows)
                list.Add(GetState(row));
            return list;
        }

        private State GetState(DataRow row)
        {
            int idState = Convert.ToInt32(row["IDState"]);
            string name = row["Name"].ToString();
            int createdUser = Convert.ToInt32(row["CreatedUser"]);
            DateTime createDate = Fecha.toDateTimeUTC(row["CreatedDate"]); 
            int modifiedUser = Convert.ToInt32(row["ModifiedUser"]);
            DateTime modifiedDate = Fecha.toDateTimeUTC(row["ModifiedDate"]);
            int status = Convert.ToInt32(row["Status"]);
            return new State(idState, name, createdUser, createDate, modifiedUser, modifiedDate, status);
        }

        public State GetById(int id)
        {
            string sql = "SELECT IDState, Name, CreatedUser, " +
                "CreatedDate, ModifiedUser, ModifiedDate, Status " +
                "FROM States " +
                "WHERE Status <> " + (int) Constants.Status.ELIMINADO + " " +
                "AND IDState = " + id;
            DataTable dt = conexion.Query(sql);
            if(dt.Rows.Count > 0)
                return GetState(dt.Rows[0]);
            return null;
        }

        public int Save(State state)
        {
            if (state.IDState == 0)
                return Insert(state);
            else
                return Update(state);
        }
        
       private int Insert(State state)
        {
            string sql = "INSERT INTO States (Name, CreatedUser, " +
                "CreatedDate, ModifiedUser, ModifiedDate, Status) " +
                "VALUES (@Name, @CreatedUser, " +
                "@CreatedDate, @ModifiedUser, @ModifiedDate, @Status)";

            string[] parametros = {"@Name", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status"};

            object[] valores = {state.Name, state.CreatedUser, state.CreatedDate, state.ModifiedUser, state.ModifiedDate, state.Status};



            return conexion.Execute(sql, parametros, valores);
        }

        private int Update(State state)
        {
            string sql = "UPDATE States SET Name = @Name, CreatedUser = @CreatedUser, " +
                "CreatedDate = @CreatedDate, ModifiedUser = @ModifiedUser, ModifiedDate = @ModifiedDate, Status = @Status " +
                "WHERE IDState = @IDState";


            string[] parametros = {"@Name", "@CreatedUser", "@CreatedDate", "@ModifiedUser", "@ModifiedDate", "@Status", "@IDState"};

            object[] valores = {state.Name, state.CreatedUser, state.CreatedDate, state.ModifiedUser, state.ModifiedDate, state.Status, state.IDState};



            return conexion.Execute(sql, parametros, valores);
        }
    }
}

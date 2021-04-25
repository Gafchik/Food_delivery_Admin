using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Food_delivery_library.About_orders
{
   public class Current_Ch_Orders
    {
        public int Id { get; set; }
        public int Chek_Id { get; set; }
        public int Order_Id { get; set; }
    }

  public  class Current_Ch_Orders_Repository : IRepository<Current_Ch_Orders>
    {
        string connectionString = Resource.ConSTR;
        public void Create(Current_Ch_Orders value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "INSERT INTO  Current_Ch_Order (Chek_Id ,Order_Id)" +
                                 " VALUES(@Chek_Id, @Order_Id)";
                        db.Execute(sqlQuery,
                           new
                           {
                               Chek_Id = value.Chek_Id,
                               Order_Id = value.Order_Id,
                             
                           },
                           transaction);
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public void Delete(Current_Ch_Orders value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "DELETE FROM  Current_Ch_Order WHERE Id = @Id";
                        db.Execute(sqlQuery, new { value.Id }, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public Current_Ch_Orders Get(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Current_Ch_Orders> GetColl()
        {
            List<Current_Ch_Orders> coll = new List<Current_Ch_Orders>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                coll = db.Query<Current_Ch_Orders>("SELECT * FROM Current_Ch_Order").ToList();
            }

            return coll;
        }

        public void Update(Current_Ch_Orders value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "UPDATE  Current_Ch_Order SET Chek_Id = @Chek_Id ,Order_Id =  @Order_Id," +                       
                            " WHERE Id = @Id";
                        db.Execute(sqlQuery,
                           new
                           {
                               Chek_Id = value.Chek_Id,
                               Order_Id = value.Order_Id,
                               Id = value.Id
                           },
                           transaction);
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}

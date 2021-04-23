using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Food_delivery_library
{
   public class Order
    {      
        public int Orders_Id { get; set; }

        public int Orders_Products_Id { get; set; }

        public int Orders_Count { get; set; }

        public int? Orders_Current_Chek_Id { get; set; }

        public int? Orders_Completed_Chek_Id { get; set; }

        public virtual Completed_Сheck Completed_Сheck { get; set; }

        public virtual Current_Сheck Current_Сheck { get; set; }

        public virtual Product product { get; set; }
    }

    public class Orders_Repository : IRepository<Order>
    {
        string connectionString = Resource.ConSTR;
        public void Create(Order value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {                      
                        var sqlQuery = "INSERT INTO [dbo].[Orders]" +
           "([Orders_Products_Id], [Orders_Count], [Orders_Current_Chek_Id], Orders_Completed_Chek_Id)" +
     "VALUES (@Orders_Products_Id,  @Orders_Count,  @Orders_Current_Chek_Id, @Orders_Completed_Chek_Id)";

                        db.Execute(sqlQuery,
                       new
                       {
                           Orders_Products_Id = value.Orders_Products_Id,
                           Orders_Count = value.Orders_Count,
                           Orders_Current_Chek_Id = value.Orders_Current_Chek_Id,
                           Orders_Completed_Chek_Id = value.Orders_Completed_Chek_Id
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

        public void Delete(Order value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "DELETE FROM  [dbo].[Orders] WHERE Orders_Id = @Orders_Id";
                        db.Execute(sqlQuery, new { value.Orders_Id }, transaction);
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

        public Order Get(int Id)
        {
            Order item = null;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                item = db.Query<Order>("SELECT * FROM [dbo].[Orders] WHERE Orders_Id = @Orders_Id", new { Id }).FirstOrDefault();
            }
            return item;
        }

        public IEnumerable<Order> GetColl()
        {
            List<Order> coll = new List<Order>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                coll = db.Query<Order>("SELECT * FROM [dbo].[Orders]").ToList();
            }
            return coll;
        }

        public void Update(Order value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "UPDATE  [dbo].[Completed_Сhecks] SET Orders_Products_Id = @Orders_Products_Id ,Orders_Count =  @Orders_Count," +
                            " Orders_Current_Chek_Id =  @Orders_Current_Chek_Id, Orders_Completed_Chek_Id = @Orders_Completed_Chek_Id)" +
                            " WHERE Orders_Id = @Orders_Id";
                        db.Execute(sqlQuery,
                           new
                           {
                               Orders_Id = value.Orders_Id,
                               Orders_Products_Id = value.Orders_Products_Id,
                               Orders_Count = value.Orders_Count,
                               Orders_Current_Chek_Id = value.Orders_Current_Chek_Id,
                               Orders_Completed_Chek_Id = value.Orders_Completed_Chek_Id
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

using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Food_delivery_library.About_orders
{
    public class Order
    {
        public int Order_Id { get; set; }
        public string Order_Products_Name { get; set; }
        public float Order_Price { get; set; }
        public float Order_Discount { get; set; }
        public float Order_Final_Price { get; set; }
        public int Order_Chek_Id { get; set; }
    }


    public class Current_Orders_Repository : IRepository<Order>
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConSTR"].ConnectionString;
        public void Create(Order value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "INSERT INTO  Current_Orders (Order_Products_Name ,Order_Price, Order_Discount, Order_Final_Price, Order_Chek_Id)" +
                                 " VALUES(@Order_Products_Name, @Order_Price, @Order_Discount, @Order_Final_Price, @Order_Chek_Id)";
                        db.Execute(sqlQuery,
                           new
                           {
                               Order_Products_Name = value.Order_Products_Name,
                               Order_Price = value.Order_Price,
                               Order_Discount = value.Order_Discount,
                               Order_Final_Price = value.Order_Final_Price,
                               Order_Chek_Id = value.Order_Chek_Id
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
                        var sqlQuery = "DELETE FROM  Current_Orders WHERE Order_Id = @Order_Id";
                        db.Execute(sqlQuery, new { value.Order_Id }, transaction);
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
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetColl()
        {
            List<Order> coll = new List<Order>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                coll = db.Query<Order>("SELECT * FROM Current_Orders").ToList();
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
                        var sqlQuery = "UPDATE  Current_Orders SET Order_Products_Name = @Order_Products_Name ,Order_Price =  @Order_Price," +
                            " Order_Discount =  @Order_Discount, Order_Final_Price = @Order_Final_Price, Order_Chek_Id = @Order_Chek_Id" +
                            " WHERE Product_Id = @Product_Id";
                        db.Execute(sqlQuery,
                           new
                           {
                               Order_Products_Name = value.Order_Products_Name,
                               Order_Chek_Id = value.Order_Chek_Id,
                               Order_Price = value.Order_Price,
                               Order_Discount = value.Order_Discount,
                               Order_Final_Price = value.Order_Final_Price,
                               Order_Id = value.Order_Id
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

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Food_delivery_library.About_orders
{
    public class Order
    {
        public Order( )
        { 
            Order_Final_Price = Order_Price - ((Order_Price / 100) * Order_Discount);
        }

        public int Order_Id { get; set; }
        public string Order_Products_Name { get; set; }
        public float Order_Price { get; set; }
        public float Order_Discount { get; set; }
        public float Order_Final_Price { get; set; }
    }

    public class Order_Repository : IRepository<Order>
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
                        var sqlQuery = "INSERT INTO  Orders (Order_Products_Name ,Order_Price, Order_Discount, Order_Final_Price)" +
                                 " VALUES(@Order_Products_Name, @Order_Price, @Order_Discount, @Order_Final_Price)";
                        db.Execute(sqlQuery,
                           new
                           {
                               Order_Products_Name = value.Order_Products_Name,
                               Order_Price = value.Order_Price,
                               Order_Discount = value.Order_Discount,
                               Order_Final_Price = value.Order_Final_Price
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
                        var sqlQuery = "DELETE FROM  Orders WHERE Order_Id = @Order_Id";
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
                coll = db.Query<Order>("SELECT * FROM Orders").ToList();
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
                        var sqlQuery = "UPDATE  Orders SET Order_Products_Name = @Order_Products_Name ,Order_Price =  @Order_Price," +
                            " Order_Discount =  @Order_Discount, Order_Final_Price = @Order_Final_Price" +
                            " WHERE Product_Id = @Product_Id";
                        db.Execute(sqlQuery,
                           new
                           {
                               Order_Products_Name = value.Order_Products_Name,
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

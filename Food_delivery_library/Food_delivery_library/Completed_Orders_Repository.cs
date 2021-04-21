namespace Food_delivery_library
{
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    public partial class Completed_Orders
    {
        public int Completed_Order_Id { get; set; }

        public int Completed_Order_User_Id { get; set; }

        public int Completed_Order_Product_Id { get; set; }

        public int Completed_Order_Count_product { get; set; }

        public DateTime Completed_Order_Date { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }

    public partial class Completed_Orders_Repository : IRepository<Completed_Orders>
    {
        string connectionString = Resource.ConSTR;
        private User_Repository user_Repository = new User_Repository();
        private Products_Repository products_Repository = new Products_Repository();
        public void Create(Completed_Orders value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "INSERT INTO  Completed_Orders (Completed_Order_User_Id ,Completed_Order_Product_Id, Completed_Order_Count_product, Completed_Order_Date)" +
                                 "VALUES(@Completed_Order_User_Id, @Completed_Order_Product_Id, @Completed_Order_Count_product, @Completed_Order_Date);";
                        db.Execute(sqlQuery,
                           new
                           {
                               Completed_Order_User_Id = value.Completed_Order_User_Id,
                               Completed_Order_Product_Id = value.Completed_Order_Product_Id,
                               Completed_Order_Count_product = value.Completed_Order_Count_product,
                               Completed_Order_Date = value.Completed_Order_Date
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

        public void Delete(Completed_Orders value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "DELETE FROM  Completed_Orders WHERE Completed_Order_Id = @Completed_Order_Id";
                        db.Execute(sqlQuery, new { value.Completed_Order_Id }, transaction);
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

        public Completed_Orders Get(int Id)
        {
            Completed_Orders item = null;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        item = db.Query<Completed_Orders>("SELECT * FROM Completed_Orders WHERE Completed_Order_Id = @Completed_Order_Id", new { Id }, transaction).FirstOrDefault();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            item.Product = products_Repository.Get(item.Completed_Order_Product_Id);
            item.User = user_Repository.Get(item.Completed_Order_User_Id);
            return item;
        }

        public IEnumerable<Completed_Orders> GetColl()
        {
            List<Completed_Orders> coll = new List<Completed_Orders>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        coll = db.Query<Completed_Orders>("SELECT * FROM Completed_Orders", transaction).ToList();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            coll.ForEach(i => i.Product = products_Repository.Get(i.Completed_Order_Product_Id));
            coll.ForEach(i => i.User = user_Repository.Get(i.Completed_Order_User_Id));
            return coll;
        }

        public void Update(Completed_Orders value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "UPDATE  Completed_Orders SET Completed_Order_User_Id = @Completed_Order_User_Id ,Completed_Order_Product_Id =  @Completed_Order_Product_Id," +
                            " Completed_Order_Count_product =  @Completed_Order_Count_product, Completed_Order_Date = @Completed_Order_Date)" +
                            " WHERE Completed_Order_Id = @Completed_Order_Id";
                        db.Execute(sqlQuery,
                           new
                           {
                               Completed_Order_User_Id = value.Completed_Order_User_Id,
                               Completed_Order_Product_Id = value.Completed_Order_Product_Id,
                               Completed_Order_Count_product = value.Completed_Order_Count_product,
                               Completed_Order_Date = value.Completed_Order_Date,
                               Completed_Order_Id = value.Completed_Order_Id
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

namespace Food_delivery_library
{
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    public partial class Current_Orders
    {      
        public int Current_Order_Id { get; set; }

        public int Current_Order_User_Id { get; set; }

        public int Current_Order_Product_Id { get; set; }

        public int Current_Order_Count_product { get; set; }
      
        public DateTime Current_Order_Date { get; set; } 

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }

    public partial class Current_Orders_Repository : IRepository<Current_Orders>
    {
        string connectionString = Resource.ConSTR;
        private User_Repository user_Repository = new User_Repository();
        private Products_Repository products_Repository = new Products_Repository();
        public void Create(Current_Orders value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "INSERT INTO  Current_Orders (Current_Order_User_Id ,Current_Order_Product_Id, Current_Order_Count_product, Current_Order_Date)" +
                                 "VALUES(@Current_Order_User_Id, @Current_Order_Product_Id, @Current_Order_Count_product, @Current_Order_Date);";
                        db.Execute(sqlQuery,
                           new
                           {
                               Current_Order_User_Id = value.Current_Order_User_Id,
                               Current_Order_Product_Id = value.Current_Order_Product_Id,
                               Current_Order_Count_product = value.Current_Order_Count_product,
                               Current_Order_Date = value.Current_Order_Date
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

        public void Delete(Current_Orders value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "DELETE FROM  Current_Orders WHERE Current_Order_Id = @Current_Order_Id";
                        db.Execute(sqlQuery, new { value.Current_Order_Id }, transaction);
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

        public Current_Orders Get(int Id)
        {
            Current_Orders item = null;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        item = db.Query<Current_Orders>("SELECT * FROM Current_Orders WHERE Current_Order_Id = @Current_Order_Id", new { Id }, transaction).FirstOrDefault();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            item.Product = products_Repository.Get(item.Current_Order_Product_Id);
            item.User = user_Repository.Get(item.Current_Order_User_Id);
            return item;
        }

        public IEnumerable<Current_Orders> GetColl()
        {
            List<Current_Orders> coll = new List<Current_Orders>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        coll = db.Query<Current_Orders>("SELECT * FROM Current_Orders", transaction).ToList();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            coll.ForEach(i => i.Product = products_Repository.Get(i.Current_Order_Product_Id));
            coll.ForEach(i => i.User = user_Repository.Get(i.Current_Order_User_Id));
            return coll;
        }

        public void Update(Current_Orders value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "UPDATE  Current_Orders SET Current_Order_User_Id = @Current_Order_User_Id ,Current_Order_Product_Id =  @Current_Order_Product_Id," +
                            " Current_Order_Count_product =  @Current_Order_Count_product, Current_Order_Date = @Current_Order_Date)" +
                            " WHERE Current_Order_Id = @Current_Order_Id";
                        db.Execute(sqlQuery,
                           new
                           {
                               Current_Order_User_Id = value.Current_Order_User_Id,
                               Current_Order_Product_Id = value.Current_Order_Product_Id,
                               Current_Order_Count_product = value.Current_Order_Count_product,
                               Current_Order_Date = value.Current_Order_Date,
                               Current_Order_Id = value.Current_Order_Id
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

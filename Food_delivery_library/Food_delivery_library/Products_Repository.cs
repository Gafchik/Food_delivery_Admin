using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Food_delivery_library
{

    public partial class Product
    {       
        public int Product_Id { get; set; }

        public int Product_category_Id { get; set; }
        public string Product_Name { get; set; }

        public double Product_Price { get; set; }

        public double Product_Discount { get; set; }
        public virtual Product_Categories Product_category { get; set; } 
    }

    public partial class Products_Repository : IRepository<Product>
    {
      public  Product_Categories_Repository repository_Categories = new Product_Categories_Repository();
        string connectionString = Resource.ConSTR;

        public void Create(Product value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "INSERT INTO  Products (Product_category_Id ,Product_Name, Product_Price, Product_Discount)" +
                                 "VALUES(@Product_category_Id, @Product_Name, @Product_Price, @Product_Discount); SELECT CAST(SCOPE_IDENTITY() as int)";
                        db.Execute(sqlQuery,
                           new
                           {
                               Product_category_Id = value.Product_category_Id,
                               Product_Name = value.Product_Name,
                               Product_Price = value.Product_Price,
                               Product_Discount = value.Product_Discount
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

        public void Delete(Product value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "DELETE FROM  Products WHERE Product_Id = @Product_Id";
                        db.Execute(sqlQuery, new { value.Product_Id }, transaction);
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

        public Product Get(int Id)
        {
            Product item = null;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        item = db.Query<Product>("SELECT * FROM Products WHERE Product_Id = @Product_Id", new { Id }, transaction).FirstOrDefault();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            item.Product_category =  repository_Categories.Get(item.Product_category_Id);
            return item;
        }

        public IEnumerable<Product> GetColl()
        {
            List<Product> coll = new List<Product>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        coll = db.Query<Product>("SELECT * FROM Products", transaction).ToList();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            coll.ForEach(i => i.Product_category = repository_Categories.Get(i.Product_category_Id));
            return coll;
        }

        public void Update(Product value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "UPDATE  Products SET Product_Name = @Product_Name ,Product_category_Id =  @Product_category_Id," +
                            " Product_Price =  @Product_Price, Product_Discount = @Product_Discount)" +
                            " WHERE Product_Id = @Product_Id";
                        db.Execute(sqlQuery,
                           new
                           {
                               Product_Name = value.Product_Name,
                               Product_category_Id = value.Product_category_Id,
                               Product_Price = value.Product_Price,
                               Product_Discount = value.Product_Discount,
                               Product_Id = value.Product_Id
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

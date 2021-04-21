using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Food_delivery_library
{
    public partial class Product_Categories
    {                  
        public int Product_category_Id { get; set; }     
        public string Product_category_Name { get; set; }            
    }
    public partial class Product_Categories_Repository : IRepository<Product_Categories>
    {
        string connectionString = Resource.ConSTR;

       
        public void Create(Product_Categories value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "INSERT INTO  Product_Categories (Product_category_Name)" +
                                 "VALUES(@Product_category_Name);";
                        db.Execute(sqlQuery,new { Product_category_Name = value.Product_category_Name },  transaction);                                          
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

        public void Delete(Product_Categories value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "DELETE FROM  Product_Categories WHERE Product_category_Id = @Product_category_Id";
                        db.Execute(sqlQuery, new { value.Product_category_Id }, transaction);
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

        public Product_Categories Get(int Id)
        {
            Product_Categories item = null;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                item = db.Query<Product_Categories>("SELECT * FROM Product_Categories WHERE Product_category_Id = @Product_category_Id", new { Id }).FirstOrDefault();
            }
            return item;
        }

        public IEnumerable<Product_Categories> GetColl()
        {
            List<Product_Categories> coll = new List<Product_Categories>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                coll = db.Query<Product_Categories>("SELECT * FROM Product_Categories").ToList();
            }
            return coll;
        }

        public void Update(Product_Categories value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "UPDATE  Product_Categories SET Product_category_Name = @Product_category_Name"  +
                            " WHERE Product_category_Id = @Product_category_Id";
                        db.Execute(sqlQuery,
                           new
                           {
                               Product_category_Name = value.Product_category_Name,
                               Product_category_Id = value.Product_category_Id
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

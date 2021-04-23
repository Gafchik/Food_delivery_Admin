namespace Food_delivery_library
{
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    public partial class Current_�heck
    {

        public int Current_�hecks_Id { get; set; }

        public int Current_Checks_Admin_Id { get; set; }

        public int Current_�hecks_User_Id { get; set; }

        public DateTime Current_�hecks_Date { get; set; }

        public virtual Admin admin { get; set; }

        public virtual User user { get; set; }



    }

    public partial class Current_�hecks_Repository : IRepository<Current_�heck>
    {
        string connectionString = Resource.ConSTR;

        public void Create(Current_�heck value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "INSERT INTO [dbo].[Current_�hecks]" +
           "([Current_Checks_Admin_Id], [Current_�hecks_User_Id], [Current_�hecks_Date])" +
     "VALUES (@Current_Checks_Admin_Id,  @Current_�hecks_User_Id,  @Current_�hecks_Date)";

                        db.Execute(sqlQuery,
                       new
                       {
                           Current_Checks_Admin_Id = value.Current_Checks_Admin_Id,
                           Current_�hecks_User_Id = value.Current_�hecks_User_Id,
                           Current_�hecks_Date = value.Current_�hecks_Date,
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

        public void Delete(Current_�heck value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "DELETE FROM  [dbo].[Current_�hecks] WHERE Current_�hecks_Id = @Current_�hecks_Id";
                        db.Execute(sqlQuery, new { value.Current_�hecks_Id }, transaction);
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

        public Current_�heck Get(int Id)
        {
            Current_�heck item = null;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                item = db.Query<Current_�heck>("SELECT * FROM [dbo].[Current_�hecks] WHERE Current_�hecks_Id = @Current_�hecks_Id", new { Id }).FirstOrDefault();
            }
            return item;
        }

        public IEnumerable<Current_�heck> GetColl()
        {
            List<Current_�heck> coll = new List<Current_�heck>();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                coll = db.Query<Current_�heck>("SELECT * FROM [dbo].[Current_�hecks]").ToList();
            }
            return coll;
        }

        public void Update(Current_�heck value)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sqlQuery = "UPDATE  [dbo].[Current_�hecks] SET Current_Checks_Admin_Id = @Current_Checks_Admin_Id ,Current_�hecks_User_Id =  @Current_�hecks_User_Id," +
                            " Current_�hecks_Date =  @Current_�hecks_Date)" +
                            " WHERE Current_�hecks_Id = @Current_�hecks_Id";
                        db.Execute(sqlQuery,
                           new
                           {
                               Current_�hecks_Id = value.Current_�hecks_Id,
                               Current_Checks_Admin_Id = value.Current_Checks_Admin_Id,
                               Current_�hecks_User_Id = value.Current_�hecks_User_Id,
                               Current_�hecks_Date = value.Current_�hecks_Date,
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

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Linq;

using System.Threading.Tasks;
using Web.Models;
using System.Collections;



//db reader will be used for select queries only
// only complex queries i.e   joins 
//normal select queries  even if from different tables can be handled easily by entity framework
// i.e    _DBContext.GetAll() 
//inserting and updating queries don't need this reader it only run selecting queries
//updating queries and deleting queries are one table queries, so it will be   handled easily by the framework
//for the sake of truth , even the joins can be handled easily by the  framework using C# LINQ , but it will be a good practice to write
//our own queries :) 
//God bless the jungle boys
namespace Web.DBEntities
{


    public interface IDBReader
    {
        object GetData(string query,string data_container);
        // List<object[]> ExecuteReader(string query);
        int ExecuteNonQuery(string query);
    }
    public class DBReader:IDBReader
    {
 
        private FantasyLeagueContext _DbContext;
        private DbConnection connection;
        public DBReader(FantasyLeagueContext FantasyLeagueDbContext)
        {
            _DbContext = FantasyLeagueDbContext;
            connection = FantasyLeagueDbContext.Database.GetDbConnection();
        }

        public DBReader()
        {
        }

        //GetData will return  list of objects array or a dictionary which is equivalent to map
        //use List as a second parameter to tell the function to return a list which
        //will be accessed like a 2d array , i.e list[0][1]
        //use Dictionary to access it like map  where each column name as  a key 
        //have it's data in an array :) 
        //god bless the jungle boys  
        public   object GetData(string query,string container_type)
        {

            List<object[]> DataRecord = new List<object[]>();
              Dictionary<string, List<Object>> dic = new Dictionary<string, List<Object>>();
            try
            {
        

                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = query;
                DbDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    int c = 0;
                    int x = 0;
                   
                    // connection.Close();
                    while (reader.Read())
                    {
                        c++;
                       
                        Object[] row = new Object[reader.FieldCount];
                        
                        for (int i=0; i<reader.FieldCount; i++)
                        {
                            if (!dic.ContainsKey(reader.GetName(i))) { 
                            dic.Add(reader.GetName(i),new List<object>());
                            }
                            dic[reader.GetName(i)].Add(reader.GetValue(i));
                        }
                        x++;
                        
                      
                        reader.GetValues(row);
                        DataRecord.Add(row);
                       // yield return row;
                    }
                    reader.Dispose(); 
                }

                reader.Dispose();
                connection.Close();

            }
            catch (Exception e)
            {
                var message = e.Message;

            }

            connection.Close();
            if(container_type=="List")
            return DataRecord;
            return dic;

          //  return DataRecord;
        }

        public int ExecuteNonQuery(string query)
        {
            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = query;
               
               int x= command.ExecuteNonQuery();
                connection.Close();
                return x;
            }
            catch (Exception ex)
            {
                connection.Close();
                return -1;
            }
        }
    }
}

using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PopulateStrongsDictionary
{
    internal class TableBase
    {
        private MainForm mainForm;

        public TableBase(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }
        protected  bool ClearTable(string tableName, NpgsqlDataSource dataSource)
        {
            mainForm.Trace("Clearing " + tableName, Color.Green);

            bool result = true;
            string cmdText = string.Empty;

            try
            {
                var command = dataSource.CreateCommand();

                bool exists = false;
                command.CommandText = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = '" + tableName + "') AS table_existence;";
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    exists = (bool)reader[0];
                }
                reader.Close();

                if (!exists)
                {
                    mainForm.Trace("Table '" + tableName + "' does not exist!", Color.Red);
                    return false;
                }

                command.CommandText = "DELETE FROM public.\"" + tableName + "\";";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                mainForm.TraceError(MethodBase.GetCurrentMethod().Name, ex.ToString());
                result = false;
            }

            return result;
        }

    }
}

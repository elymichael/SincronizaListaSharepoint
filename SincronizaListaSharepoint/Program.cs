namespace SincronizaListaSharepoint
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;

    class Program
    {
        static void Main(string[] args)
        {
            process();
            //test();
        }

        /// <summary>
        /// Sent to process the records.
        /// </summary>
        private static void process()
        {
            try
            {
                Console.WriteLine("Obteniendo listado de empleados");
                /// Get all Employees.
                DataTable dtEmployeeList = GetEmployeeList();
                if (dtEmployeeList.Rows.Count > 0)
                {
                    // Get Active Directory Information.
                    ADModel model = new ADModel();
                    /// Component to interact with sharepoint.
                    SharepointInteraction interaction = new SharepointInteraction(
                        ConfigurationManager.AppSettings["sharepointsite"],
                        ConfigurationManager.AppSettings["account"],
                        ConfigurationManager.AppSettings["password"]);

                    Console.WriteLine("Borrando lista de empleados");
                    /// Delete all items in the list before add the new records.
                    interaction.deleteItem(ConfigurationManager.AppSettings["listname"]);

                    //// Read All records and add them to the context.
                    foreach (DataRow dr in dtEmployeeList.Rows)
                    {
                        // Username information from active directory.
                        UserInformation info = model.GetUserInformation(ConfigurationManager.AppSettings["domainname"], dr["Username"].ToString());
                        interaction.AddItem(ConfigurationManager.AppSettings["listname"], dr, info);
                    }
                    Console.WriteLine("Guardando lista de empleados");
                    /// Execute the query.
                    interaction.ExecuteQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                EventLog.WriteEntry("Sharepoint Interaction", "Error: " + ex.Message, EventLogEntryType.Error);
            }
        }

        private static void test()
        {
            // Get Active Directory Information.
            ADModel model = new ADModel();
            // Username information from active directory.
            UserInformation info = model.GetUserInformation(ConfigurationManager.AppSettings["domainname"], "enunez@dgcp.gob.do");

        }
        /// <summary>
        /// Get all employees.
        /// </summary>
        /// <returns></returns>
        private static DataTable GetEmployeeList()
        {
            SqlData data = new SqlData(ConfigurationManager.ConnectionStrings["sourceconnection"].ConnectionString);
            return data.GetDatatable();
        }

    }
}

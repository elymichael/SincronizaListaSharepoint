namespace SincronizaListaSharepoint
{
    using System;    
    using System.Data;
    using System.Data.SqlClient;
    
    public class SqlData
    {
        private SqlConnection conn;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionstring">Default connection string to use.</param>
        public SqlData(string connectionstring)
        {
            conn = new SqlConnection(connectionstring);
        }

        /// <summary>
        /// Get list of employee from HR database.
        /// </summary>
        /// <returns>Datatable</returns>
        public DataTable GetDatatable()
        {
            DataTable dt = new DataTable();

            string sqlData = @"SELECT 
                                Title = nombres + ' ' + apellidos, 
                                JobTitle = cargos, 
                                BirthDay = DATEADD(DAY, 1, CASE 
                                    WHEN YEAR(Fecha_Nacimiento) > 2000 THEN Fecha_Nacimiento 
                                    ELSE DATEADD(year, datediff(year, Fecha_Nacimiento, '2000-01-01'),Fecha_Nacimiento) 
                                END),
                                Email = REPLACE(correo,' ',''),
                                Department = Depto_Descripcion,
                                Username = SUBSTRING(REPLACE(correo,' ',''), 0, CHARINDEX('@',REPLACE(correo,' ','')))
                               FROM vw_EmpleadosHistoricos WITH(NOLOCK) 
                               WHERE Estatus = 1 AND periodo = YEAR(GETDATE())";
            try
            {                
                SqlDataAdapter adap = new SqlDataAdapter(sqlData, conn);
                adap.Fill(dt);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return dt;
        }
    }
}

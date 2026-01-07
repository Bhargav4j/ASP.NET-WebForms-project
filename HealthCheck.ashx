<%@ WebHandler Language="C#" Class="HealthCheck" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

public class HealthCheck : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        try
        {
            // Check database connectivity
            string connectionString = ConfigurationManager.ConnectionStrings["filmsConnectionString"]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                context.Response.StatusCode = 503;
                context.Response.Write("{\"status\":\"unhealthy\",\"error\":\"Connection string not configured\"}");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT 1", conn))
                {
                    cmd.ExecuteScalar();
                }
            }

            context.Response.StatusCode = 200;
            context.Response.Write("{\"status\":\"healthy\",\"timestamp\":\"" + DateTime.UtcNow.ToString("o") + "\"}");
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 503;
            context.Response.Write("{\"status\":\"unhealthy\",\"error\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }
}

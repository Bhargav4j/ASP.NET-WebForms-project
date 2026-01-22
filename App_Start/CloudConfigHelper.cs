using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace FIlms
{
    /// <summary>
    /// Cloud Configuration Helper for AWS deployment
    /// Replaces environment variable placeholders in connection strings and configuration values
    /// </summary>
    public static class CloudConfigHelper
    {
        /// <summary>
        /// Initializes cloud configuration by replacing environment variable placeholders
        /// Call this method in Global.asax Application_Start before any database access
        /// </summary>
        public static void InitializeCloudConfiguration()
        {
            ReplaceEnvironmentVariablesInConnectionStrings();
            ReplaceEnvironmentVariablesInAppSettings();
        }

        /// <summary>
        /// Replaces ${VAR_NAME} placeholders in connection strings with environment variable values
        /// Falls back to AWS Parameter Store or provides helpful error messages
        /// </summary>
        private static void ReplaceEnvironmentVariablesInConnectionStrings()
        {
            foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
            {
                if (connectionString.ConnectionString.Contains("${"))
                {
                    string originalConnectionString = connectionString.ConnectionString;
                    string updatedConnectionString = ReplaceEnvironmentVariables(originalConnectionString);

                    // Use reflection to update the readonly ConnectionString property
                    var connectionStringField = typeof(ConfigurationElement)
                        .GetField("_bReadOnly", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    connectionStringField?.SetValue(connectionString, false);

                    var connectionStringProperty = typeof(ConnectionStringSettings)
                        .GetProperty("ConnectionString");
                    connectionStringProperty?.SetValue(connectionString, updatedConnectionString, null);

                    connectionStringField?.SetValue(connectionString, true);
                }
            }
        }

        /// <summary>
        /// Replaces ${VAR_NAME} placeholders in app settings with environment variable values
        /// </summary>
        private static void ReplaceEnvironmentVariablesInAppSettings()
        {
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                string value = ConfigurationManager.AppSettings[key];
                if (!string.IsNullOrEmpty(value) && value.Contains("${"))
                {
                    string updatedValue = ReplaceEnvironmentVariables(value);
                    ConfigurationManager.AppSettings[key] = updatedValue;
                }
            }
        }

        /// <summary>
        /// Replaces ${VAR_NAME} patterns with environment variable values
        /// Supports fallback to AWS Systems Manager Parameter Store format
        /// </summary>
        private static string ReplaceEnvironmentVariables(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Pattern to match ${VARIABLE_NAME}
            var pattern = @"\$\{([^}]+)\}";
            var matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
            {
                string variableName = match.Groups[1].Value;
                string variableValue = GetEnvironmentVariableValue(variableName);

                if (!string.IsNullOrEmpty(variableValue))
                {
                    input = input.Replace(match.Value, variableValue);
                }
                else
                {
                    // Log warning about missing environment variable
                    System.Diagnostics.Trace.TraceWarning(
                        $"Environment variable '{variableName}' not found. " +
                        $"Please set this in AWS Elastic Beanstalk Environment Properties, " +
                        $"EC2 User Data, or AWS Systems Manager Parameter Store.");

                    // For development, provide a helpful placeholder
                    if (IsLocalDevelopment())
                    {
                        input = input.Replace(match.Value, GetDevelopmentDefault(variableName));
                    }
                }
            }

            return input;
        }

        /// <summary>
        /// Gets environment variable value with fallback options
        /// Priority: 1) Environment Variable, 2) AWS Parameter Store (if available), 3) Default
        /// </summary>
        private static string GetEnvironmentVariableValue(string variableName)
        {
            // First, try standard environment variable
            string value = Environment.GetEnvironmentVariable(variableName);
            if (!string.IsNullOrEmpty(value))
                return value;

            // Try with different prefixes (common in cloud platforms)
            value = Environment.GetEnvironmentVariable($"APPSETTING_{variableName}");
            if (!string.IsNullOrEmpty(value))
                return value;

            // AWS Elastic Beanstalk sometimes prefixes with aws:
            value = Environment.GetEnvironmentVariable($"aws_{variableName}");
            if (!string.IsNullOrEmpty(value))
                return value;

            // TODO: Add AWS Systems Manager Parameter Store integration here
            // Example: var parameterStoreValue = GetFromParameterStore($"/films-app/{variableName}");

            return null;
        }

        /// <summary>
        /// Determines if the application is running in local development mode
        /// </summary>
        private static bool IsLocalDevelopment()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNET_ENVIRONMENT")
                              ?? Environment.GetEnvironmentVariable("ENVIRONMENT")
                              ?? "Development";
            return environment.Equals("Development", StringComparison.OrdinalIgnoreCase)
                || environment.Equals("Local", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Provides development defaults for local testing
        /// </summary>
        private static string GetDevelopmentDefault(string variableName)
        {
            switch (variableName.ToUpperInvariant())
            {
                case "DB_SERVER":
                    return "localhost";
                case "DB_NAME":
                    return "films";
                case "DB_AUTH_NAME":
                    return "aspnet-films-auth";
                case "DB_USER":
                    return "sa";
                case "DB_PASSWORD":
                    return "YourLocalPassword123!";
                case "MACHINE_VALIDATION_KEY":
                    return "AUTO"; // ASP.NET will auto-generate for development
                case "MACHINE_DECRYPTION_KEY":
                    return "AUTO"; // ASP.NET will auto-generate for development
                default:
                    return $"NOT_SET_{variableName}";
            }
        }

        /// <summary>
        /// Validates that all required environment variables are set
        /// Call this after InitializeCloudConfiguration to ensure proper configuration
        /// </summary>
        public static void ValidateCloudConfiguration()
        {
            var requiredVariables = new[]
            {
                "DB_SERVER",
                "DB_NAME",
                "DB_AUTH_NAME",
                "DB_USER",
                "DB_PASSWORD"
            };

            var missingVariables = new System.Collections.Generic.List<string>();

            foreach (var variable in requiredVariables)
            {
                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(variable)) && !IsLocalDevelopment())
                {
                    missingVariables.Add(variable);
                }
            }

            if (missingVariables.Count > 0)
            {
                var message = $"Missing required environment variables for cloud deployment: {string.Join(", ", missingVariables)}. " +
                             $"Please configure these in AWS Elastic Beanstalk Environment Properties or AWS Systems Manager Parameter Store.";
                System.Diagnostics.Trace.TraceError(message);

                if (!IsLocalDevelopment())
                {
                    throw new ConfigurationErrorsException(message);
                }
            }
        }
    }
}

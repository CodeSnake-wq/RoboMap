using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using EntityFrameworkCore.Scaffolding.Handlebars;
using System.Globalization;
using HandlebarsDotNet;
using System.Text.RegularExpressions;
using System.Collections.Generic;

//public class ScaffoldingDesignTimeServices : IDesignTimeServices
//{
//    public void ConfigureDesignTimeServices(IServiceCollection services)
//    {
//        services.AddHandlebarsScaffolding(options =>
//        {
//            options.ReverseEngineerOptions = ReverseEngineerOptions.DbContextAndEntities;
//            options.TemplateData = new Dictionary<string, object>
//            {
//                {"models-namespace", "robot_controller_api.Models" }
//            };
//        });

//         Register custom helpers
//        HandlebarsHelpers.Register();
//    }

//     Inner static class to register Handlebars helpers
//    private static class HandlebarsHelpers
//    {
//        public static void Register()
//        {
//             Register camelCase helper
//            HandlebarsDotNet.Handlebars.RegisterHelper("camelCase", (writer, context, parameters) =>
//            {
//                var value = parameters[0] as string;
//                writer.WriteSafeString(ToCamelCase(value));
//            });

//             Register PascalCase helper
//            HandlebarsDotNet.Handlebars.RegisterHelper("PascalCase", (writer, context, parameters) =>
//            {
//                var value = parameters[0] as string;
//                writer.WriteSafeString(ToPascalCase(value));
//            });
//        }

//         Method to convert snake_case to camelCase
//        private static string ToCamelCase(string text)
//        {
//             Remove any extra white spaces
//            text = text.Trim();

//             Convert snake_case to camelCase
//            text = Regex.Replace(text, @"_(\w)", m => m.Groups[1].Value.ToUpper());

//             Convert first character to lowercase
//            return char.ToLowerInvariant(text[0]) + text.Substring(1);
//        }

//         Method to convert snake_case to PascalCase
//        private static string ToPascalCase(string text)
//        {
//             Remove any extra white spaces
//            text = text.Trim();

//             Convert snake_case to PascalCase
//            text = Regex.Replace(text, @"(?:^|_)(\w)", m => m.Groups[1].Value.ToUpper());

//             Convert first character to uppercase
//            return char.ToUpperInvariant(text[0]) + text.Substring(1);
//        }
//    }
//}

public class ScaffoldingDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
        services.AddHandlebarsScaffolding(options =>
        {
            options.ReverseEngineerOptions = ReverseEngineerOptions.DbContextAndEntities;
            options.TemplateData = new Dictionary<string, object>
            {
                {"models-namespace", "robot_controller_api.Models"}
            };
        });

        // Register custom Handlebars helpers
        RegisterHandlebarsHelpers();
    }

    private void RegisterHandlebarsHelpers()
    {
        HandlebarsDotNet.Handlebars.RegisterHelper("camelCase", (writer, context, parameters) =>
        {
            writer.WriteSafeString(ToCamelCase(parameters[0] as string));
        });

        HandlebarsDotNet.Handlebars.RegisterHelper("PascalCase", (writer, context, parameters) =>
        {
            writer.WriteSafeString(ToPascalCase(parameters[0] as string));
        });
    }

    private string ToCamelCase(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        text = text.Trim();
        var words = text.Split('_');
        for (int i = 1; i < words.Length; i++)
        {
            if (words[i].Length > 0)
                words[i] = char.ToUpperInvariant(words[i][0]) + words[i].Substring(1);
        }

        var camelCase = string.Join(string.Empty, words);
        return char.ToLowerInvariant(camelCase[0]) + camelCase.Substring(1);
    }

    private string ToPascalCase(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        text = text.Trim();
        var words = text.Split('_');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 0)
                words[i] = char.ToUpperInvariant(words[i][0]) + words[i].Substring(1);
        }

        return string.Join(string.Empty, words);
    }
}
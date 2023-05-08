// Copyright (c) 2023  RAYSAPPS PTY LTD Licensed under the MIT License.
//  See LICENSE in the project root for license information.
//  This file is strictly confidential and internal to the company.

using System.Text.Json;

namespace RaysApps.Services.PlugAndPlayFormat;

public record RaysIndexModel(string Name, string? DisplayName, string? Description, IReadOnlyDictionary<string, string>? DisplayNames, IReadOnlyDictionary<string, string>? DescriptionNames);
public class PnPIndexModel
{
    public static RaysIndexModel GetPnPIndexModel(JsonProperty prop)
    {
        _ = setProperty(prop.Value, "displayName", out var displayName, out var displayNamesVal);
        _ = setProperty(prop.Value, "description", out var description, out var discriptionNamesVal);
        return new RaysIndexModel(prop.Name, displayName, description, displayNamesVal, discriptionNamesVal);
    }

    private static readonly string[] s_engLang = new[] { "en", "en-au", "en-bz", "en-ca", "en-cb", "en-gb", "en-in", "en-ie", "en-jm","en-nz", "en-ph",
    "en-za","en-tt", "en-us"};
    private static bool setProperty(JsonElement model, string propTypeName, out string? propStringVal, out Dictionary<string, string> propDicVal)
    {
        propStringVal = null;
        propDicVal = new();
        if (!model.TryGetProperty(propTypeName, out var propTypeNameValue))
        {
            return false;
        }

        switch (propTypeNameValue.ValueKind)
        {
            case JsonValueKind.String:
                {
                    propStringVal = propTypeNameValue.GetString();
                    return true;
                }
            case JsonValueKind.Object:
                {

                    foreach (var item in propTypeNameValue.EnumerateObject())
                    {
                        // default first property value
                        if (string.IsNullOrEmpty(propStringVal))
                        {
                            propStringVal = item.Value.ToString();
                        }
                        // Add display Names to the Array.

                        var val = item.Value.ToString();
                        propDicVal.Add(item.Name, val);
                        // check for available english language and assign it to the display name. 
                        if (s_engLang.Contains(item.Name.ToLower()))
                        {
                            propStringVal = item.Value.GetString();
                        }
                    }
                    return true;
                }
            default: return false;

        }
    }
}

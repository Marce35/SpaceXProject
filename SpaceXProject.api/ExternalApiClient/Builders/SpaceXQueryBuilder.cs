using SpaceXProject.api.Data.Enums;
using SpaceXProject.api.Data.Models.SpaceXApi.Requests;
using SpaceXProject.api.Shared.Helpers;
using System.Linq.Expressions;

namespace SpaceXProject.api.ExternalApiClient.Builders;

public class SpaceXQueryBuilder<T>
{
    private readonly SpaceXQueryRequest _request = new();

    public SpaceXQueryBuilder()
    {
        _request.Options.Limit = 10; //TODO: Add to constants
        _request.Options.Page = 1;
    }

    public SpaceXQueryBuilder<T> WithPagination(int page, int limit)
    {
        _request.Options.Page = page < 1 ? 1 : page;
        _request.Options.Limit = limit < 1 ? 10 : limit;
        return this;
    }

    public SpaceXQueryBuilder<T> SortBy(Expression<Func<T, object?>> fieldSelector, SortDirection direction)
    {
        var jsonName = JsonPropertyHelper.GetJsonPropertyName(fieldSelector);

        var sortObj = new Dictionary<string, string>
        {
            { jsonName, direction == SortDirection.Asc ? "asc" : "desc" }
        };

        _request.Options.Sort = sortObj;
        return this;
    }

    public SpaceXQueryBuilder<T> Search(Expression<Func<T, object?>> fieldSelector, string? term)
    {
        if (string.IsNullOrWhiteSpace(term)) return this;

        var jsonName = JsonPropertyHelper.GetJsonPropertyName(fieldSelector);
        var queryDict = EnsureQueryDictionary();

        queryDict[jsonName] = new Dictionary<string, string>
        {
            { "$regex", term },
            { "$options", "i" }
        };
        return this;
    }

    public SpaceXQueryBuilder<T> FilterByStatus(LaunchStatusFilter status)
    {
        var queryDict = EnsureQueryDictionary();
        switch (status)
        {
            case LaunchStatusFilter.Upcoming:
                queryDict["upcoming"] = true;
                break;
            case LaunchStatusFilter.Past:
                queryDict["upcoming"] = false;
                break;
            default:
                if (queryDict.ContainsKey("upcoming")) queryDict.Remove("upcoming");
                break;
        }
        return this;
    }

    public SpaceXQueryBuilder<T> Populate(Expression<Func<T, object?>> fieldSelector, string[]? selectFields = null)
    {
        var jsonPath = JsonPropertyHelper.GetJsonPropertyName(fieldSelector);

        object populateItem;

        if (selectFields != null && selectFields.Length > 0)
        {
            var selectionDict = new Dictionary<string, int>();
            foreach (var field in selectFields)
            {
                selectionDict[field] = 1;
            }

            populateItem = new
            {
                path = jsonPath,
                select = selectionDict
            };
        }
        else
        {
            populateItem = jsonPath;
        }

        var currentList = _request.Options.Populate.ToList();
        currentList.Add(populateItem);
        _request.Options.Populate = currentList.ToArray();

        return this;
    }

    public SpaceXQueryRequest Build() => _request;

    private Dictionary<string, object> EnsureQueryDictionary()
    {
        if (_request.Query is not Dictionary<string, object> dict)
        {
            dict = new Dictionary<string, object>();
            _request.Query = dict;
        }
        return dict;
    }
}
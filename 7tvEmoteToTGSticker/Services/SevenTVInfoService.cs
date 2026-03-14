using _7tvEmoteToTGSticker.Models;
using _7tvEmoteToTGSticker.Models.GlobalSearchModel;
using _7tvEmoteToTGSticker.Models.SevenTVModel;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;

namespace _7tvEmoteToTGSticker.Services;

public class SevenTVInfoService : IDisposable
{
    private readonly GraphQLHttpClient _graphQLClient;
    private const string GraphQLEndpoint = "https://7tv.io/v4/gql";

    public SevenTVInfoService()
    {
        _graphQLClient = new GraphQLHttpClient(GraphQLEndpoint, new SystemTextJsonSerializer());
    }

    public async Task<List<UserDbo>?> GetUsers(string search)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(search);

        var query = new GraphQLRequest
        {
            Query = """
                    query GlobalSearch($query: String!) {
                      search {
                        all(query: $query, page: 1, perPage: 20) {
                          users {
                            items {
                              id
                              mainConnection {
                                platformDisplayName
                                platformAvatarUrl
                              }
                            }
                          }
                        }
                      }
                    }
                    """,
            OperationName = "GlobalSearch",
            Variables = new { query = search }
        };

        var res = await _graphQLClient.SendQueryAsync<GlobalSearchData>(query);

        if (res.Errors?.Any() == true)
        {
            throw new InvalidOperationException($"GraphQL query failed: {string.Join(", ", res.Errors.Select(e => e.Message))}");
        }

        return res.Data?.search?.all?.users?.items?
            .Select(item => new UserDbo
            {
                Id = item.id,
                Username = item.mainConnection?.platformDisplayName ?? string.Empty,
                UrlAvatar = item.mainConnection?.platformAvatarUrl ?? string.Empty
            })
            .ToList();
    }

    public async Task<User?> GetUser(string userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        var query = new GraphQLRequest
        {
            Query = """
                    query UserQuery($id: Id!) {
                      users {
                        user(id: $id) {
                          id
                          mainConnection {
                            platformId
                            platform
                            platformDisplayName
                            platformAvatarUrl
                          }
                          style {
                            activeEmoteSet {
                              id
                            }
                          }
                        }
                      }
                    }
                    """,
            OperationName = "UserQuery",
            Variables = new { id = userId }
        };
        var res = await _graphQLClient.SendQueryAsync<UserSearch>(query);
        if (res.Errors?.Any() == true)
        {
            throw new InvalidOperationException($"GraphQL query failed: {string.Join(", ", res.Errors.Select(e => e.Message))}");
        }
        return res.Data.users.user;
    }
    public async Task<List<ItemEmote>?> GetUserEmotes(string userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        var query = new GraphQLRequest
        {
            Query = """
                    query UserQuery($id: Id!) {
                      users {
                        user(id: $id) {
                          id
                          style {
                            activeEmoteSet {
                              id
                              emotes(query: " ", page: 0, perPage: 1000) {
                                items {
                                  id
                                  alias
                                  emote{
                                    id
                                    defaultName
                                    images{
                                      url
                                      width
                                      height
                                    }
                                  }
                                }
                                totalCount
                              }
                            }
                          }
                        }
                      }
                    }
                    """,
            OperationName = "UserQuery",
            Variables = new { id = userId }
        };
        var res = await _graphQLClient.SendQueryAsync<UserSearch>(query);
        if (res.Errors?.Any() == true)
        {
            throw new InvalidOperationException($"GraphQL query failed: {string.Join(", ", res.Errors.Select(e => e.Message))}");
        }
        return res.Data.users.user.style.activeEmoteSet.emotes.items;
    }

    public void Dispose()
    {
        _graphQLClient?.Dispose();
    }
}
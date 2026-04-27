namespace NashFridayStore.BFF.Features.Tokens.GetTokens;

public sealed record Response(IEnumerable<ClaimResponse> Claims, TokensResponse Tokens);
public sealed record ClaimResponse(string Type, string Name);
public sealed record TokensResponse(string? AccessToken, string? IdToken, string? RefreshToken);

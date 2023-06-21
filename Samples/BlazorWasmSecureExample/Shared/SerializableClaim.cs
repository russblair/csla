using System.Security.Claims;

namespace BlazorWasmSecureExample.Shared
{
  /// <summary>
  /// Claim type that can be serialized
  /// </summary>
  public class SerializableClaim
  {
    public SerializableClaim()
    {
    }

    public SerializableClaim(Claim claim)
    {
      Issuer = claim.Issuer;
      OriginalIssuer = claim.OriginalIssuer;
      Type = claim.Type;
      Value = claim.Value;
      ValueType = claim.ValueType;
      Properties = new Dictionary<string, string>(claim.Properties);
    }

    public string Issuer { get; set; } = string.Empty;

    public string OriginalIssuer { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string ValueType { get; set; } = string.Empty;

    public Dictionary<string, string> Properties { get; set; } = new();
  }
}

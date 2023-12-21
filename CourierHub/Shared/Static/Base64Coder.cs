using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CourierHub.Shared.Static {
    public static class Base64Coder {
        public static string Encode(string input) {
            if (input.IsNullOrEmpty()) { return input; }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

        public static string Decode(string input) {
            if (input.IsNullOrEmpty()) { return input; }
            return Encoding.UTF8.GetString(Convert.FromBase64String(input));
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RedisTst.Models {
    public class TstModel {
        public int Id { get; set; }

        public string? Value { get; set; } = string.Empty;
    }
}

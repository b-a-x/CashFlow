using System.Collections.Generic;

namespace WebClient.Model.Entities
{
    public class Income
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int OrderNumber { get; set; }
        public string UserId { get; set; }
        /*
        public override bool Equals(object obj)
        {
            if (!(obj is Income other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<string>.Default.Equals(Id, other.Id);
        }

        public static bool operator ==(Income a, Income b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Income a, Income b) => !(a == b);

        public override int GetHashCode() => EqualityComparer<string>.Default.GetHashCode(Id);
        */
    }
}

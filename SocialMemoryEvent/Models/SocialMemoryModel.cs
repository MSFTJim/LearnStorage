using System;

namespace SocialMemoryEvent.Models
{
    public class Item : IComparable<Item>, IEquatable<Item>
    {
        public int Id { get; set; }        
        public string? Name { get; set; } //= null;
        public decimal Price { get; set; }
        public decimal Rating { get; set; }
        public string? ImagePath { get; set; } //= string.Empty;

         public bool Equals(Item? other)
        {
            if (other == null) return false;
            return (this.Id.Equals(other.Id));
        }
        public int CompareTo(Item? compareItem)
        {
            // A null value means that this object is greater.
            if (compareItem == null)
                return 1;

            else
                return this.Id.CompareTo(compareItem.Id);
        }       

        public override string ToString()
        {
            return "Id: " + Id + ", Name: " + Name + ", Price: " + Price + ", Rating: " + Rating;
        }


    }

}
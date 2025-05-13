namespace AgriEnergyConnect.Web.Models
{
    public class Field
    {
        public int Id { get; set; }
        public required string Name { get; set; }             // C# 11 “required”
        public double AreaHectares { get; set; }
    }
}


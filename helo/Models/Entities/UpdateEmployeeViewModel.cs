namespace helo.Models.Entities
{
    public class UpdateEmployeeViewModel
    {
        public Guid id { get; set; }

        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }

        public string Email { get; set; }

        public string Department { get; set; }
    }
}

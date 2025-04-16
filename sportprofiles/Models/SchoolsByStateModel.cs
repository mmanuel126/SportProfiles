using System;

namespace sportprofiles.Models
{
    public class SchoolsByStateModel
    {
        public string? SchoolId { get; set; }
        public string? SchoolName { get; set; }
    }

    public class StatesModel
    {
        public string? name { get; set; }
        public string? abbreviation { get; set; }
    }

    public class Item
    {
        public string? Id { get; set; }
        public string? Text { get; set; }
        public string? Description { get; set; }
        public string? name { get; set; }
    }

}

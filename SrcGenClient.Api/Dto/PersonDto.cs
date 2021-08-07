using MapTo;
using SrcGenClient.Api.Entities;

namespace SrcGenClient.Api.Dto
{
    [MapFrom(typeof(Person))]
    public partial class PersonDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

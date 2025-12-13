using System.ComponentModel.DataAnnotations;
using SplitEase.Model;

namespace SplitEase.DTO
{
    public class GroupDtoResponse
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public Guid CreatedByUserId { get; set; }
        //public Usermodel? CreatedByUser { get; set; }


        public DateTime CreatedDate { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProjectAPI.Models
{
    [Table("HistoryImageColorized")]
    public class HistoryImageColorized
    {
        [Key]
        public int HistoryImageColorizedID { get; set; }
        public string ImageOriginName { get; set; } = string.Empty;
        public string ImageColorizedName { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }


    }
}

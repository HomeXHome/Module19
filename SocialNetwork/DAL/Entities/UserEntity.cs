using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.DAL.Entities;

public class UserEntity
{
    [Column("id")]
    public int id { get; set; }
    [Column("firstname")]
    public string firstname { get; set; }
    [Column("lastname")]
    public string lastname { get; set; }
    [Column("password")]
    public string password { get; set; }
    [Column("email")]
    public string email { get; set; }
    [Column("photo")]
    public string photo { get; set; }
    [Column("favorite_movie")]
    public string favorite_movie { get; set; }
    [Column("favorite_book")]
    public string favorite_book { get; set; }
}
